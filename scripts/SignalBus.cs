using Godot;
using Godot.Collections;
using System;

public partial class SignalBus : Node
{
	[Signal]
	public delegate void ShipHitEventHandler(Vector2I cell);

	[Signal]
	public delegate void ShipScannedEventHandler(Vector2I cell);
	
	[Signal]
	public delegate void GridCellsSelectedEventHandler(Array<Vector2I> cells);

	public static SignalBus Instance { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
