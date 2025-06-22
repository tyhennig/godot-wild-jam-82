using Godot;
using System;

public partial class Shop : CanvasLayer
{
	[Signal]
	public delegate void ShopClosedEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var button = GetNode<Button>("NextRoundButton");
		button.Pressed += ExitShop;

		var shopLabel = GetNode<Label>("ShopLabel");
	}

	private void ExitShop()
	{
		this.Hide();
		EmitSignal(SignalName.ShopClosed);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
