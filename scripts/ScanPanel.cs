using Godot;
using System;
using System.Collections.Generic;

public partial class ScanPanel : Control
{
	[Signal]
	public delegate void ScanPressedEventHandler();

	private ItemList itemList;
	private List<GridWreck.CursorShape> cursorShapes = new();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Button>("%ScanButton").Pressed += ScanButtonPressed;
		itemList = GetNode<ItemList>("%ItemList");
		itemList.ItemSelected += OnItemSelected;

		SignalBus.Instance.ScanCursorPurchased += AddCursor;

		AddCursor(new CursorShape3x3());
	}

	public void AddCursor(GridWreck.CursorShape shape)
	{
		cursorShapes.Add(shape);
		itemList.AddItem(shape.GetDescription());
	}

	private void OnItemSelected(long index)
	{
		GD.Print("Selected Scan Item ", index);

		var selectedItem = cursorShapes[(int)index];
		PackedScene shapeScene = GD.Load<PackedScene>(selectedItem.GetSceneLocation());
		SignalBus.Instance.EmitSignal(SignalBus.SignalName.ScanShapeSelected, shapeScene);
	}

	private void ScanButtonPressed()
	{
		EmitSignal(SignalName.ScanPressed);
	}
}
