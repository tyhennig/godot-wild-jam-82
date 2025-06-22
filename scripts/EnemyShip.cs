using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class EnemyShip : Node2D
{


	[Signal]
	public delegate void ShipScannedEventHandler(Vector2I cell);
	[Signal]
	public delegate void ShipDestroyedEventHandler(EnemyShip ship);

	public List<Vector2I> GridLocations { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameManager.Instance.FiredOnGrid += OnFired;
		GameManager.Instance.ScannedGrid += OnScanned;

		// For DEBUGGING ONLY:
		// Grid gridNode = GetNode<Grid>("/root/Main/World/Grid");
		// Texture2D overlayTexture = GD.Load<Texture2D>("res://assets/ship_overlay.png");
		// foreach (var cell in GridLocations)
		// {
		// 	Sprite2D sprite = new();

		// 	sprite.GlobalPosition = gridNode.ToGlobal(gridNode.GridVecToLocalVec(cell));
		// 	sprite.Texture = overlayTexture;
		// 	AddChild(sprite);
		// }
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
				SignalBus.Instance.EmitSignal(SignalBus.SignalName.ShipScanned, cell);
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
				SignalBus.Instance.EmitSignal(SignalBus.SignalName.ShipHit, cell);
			}
		}

		if (GridLocations.Count == 0)
		{
			GD.Print("Destroyed Ship!");
			// EmitSignal(SignalName.ShipDestroyed, this);
			QueueFree();
		}
	}
}
