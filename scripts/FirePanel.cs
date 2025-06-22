using Godot;
using GridWreck;
using System;
using System.Collections.Generic;

public partial class FirePanel : Control
{
	[Signal]
	public delegate void FirePressedEventHandler();

	private ItemList itemList;
	private List<GridWreck.CursorShape> cursorShapes = new();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Button>("%FireButton").Pressed += FireButtonPressed;
		itemList = GetNode<ItemList>("%ItemList");
		itemList.ItemSelected += OnItemSelected;

		SignalBus.Instance.FireCursorPurchased += AddCursor;

		AddCursor(new CursorShape1x1());
	}

	public void AddCursor(GridWreck.CursorShape shape)
	{
		cursorShapes.Add(shape);
		itemList.AddItem(shape.GetDescription());
	}

	private void OnItemSelected(long index)
	{
		GD.Print("Selected Fire Item ", index);
		var selectedItem = cursorShapes[(int)index];
		PackedScene shapeScene = GD.Load<PackedScene>(selectedItem.GetSceneLocation());
		SignalBus.Instance.EmitSignal(SignalBus.SignalName.FireShapeSelected, shapeScene);
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

	private void FireButtonPressed()
	{
		EmitSignal(SignalName.FirePressed);
	}

	
}
