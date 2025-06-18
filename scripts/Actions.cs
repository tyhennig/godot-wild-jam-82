using Godot;
using System;

public partial class Actions : Control
{
	[Signal]
	public delegate void FirePressedEventHandler();

	[Signal]
	public delegate void ScanPressedEventHandler();

	[Export]
	private Button _fireButton;

	[Export]
	private Button _scanButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_fireButton.Pressed += FireButtonPressed;
		_scanButton.Pressed += ScanButtonPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	private void FireButtonPressed()
	{
		EmitSignal(SignalName.FirePressed);
	}

	private void ScanButtonPressed()
	{
		EmitSignal(SignalName.ScanPressed);
	}
}
