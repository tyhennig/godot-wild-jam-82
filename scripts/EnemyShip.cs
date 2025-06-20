using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Numerics;

public partial class EnemyShip : Node2D
{
	[Signal]
	public delegate void ShipHitEventHandler(Vector2I cell);

	[Signal]
	public delegate void ShipScannedEventHandler(Vector2I cell);

	public List<Vector2I> GridLocations { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameManager.Instance.FiredOnGrid += OnFired;
		GameManager.Instance.ScannedGrid += OnScanned;
	}

	public override void _ExitTree()
	{
		GameManager.Instance.FiredOnGrid -= OnFired;
		GameManager.Instance.ScannedGrid -= OnScanned;
    }



    // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnScanned(Array<Vector2I> cells)
	{
		foreach (var cell in cells)
		{
			if (GridLocations.Contains(cell))
			{
				GD.Print("Scanned Enemy Ship!");
				EmitSignal(SignalName.ShipScanned, cell);
			}
		}
	}

	private void OnFired(Array<Vector2I> cells)
	{
		foreach (var cell in cells)
		{
			if (GridLocations.Contains(cell))
			{
				GD.Print("Hit Enemy Ship!");
				GridLocations.Remove(cell);
				EmitSignal(SignalName.ShipHit, cell);
			}
		}

		if (GridLocations.Count == 0)
		{
			GD.Print("Destroyed Ship!");
			this.QueueFree();
		}
	}
	
}
