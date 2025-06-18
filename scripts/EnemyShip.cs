using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyShip : Node2D
{
	public List<Vector2I> GridLocations { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Grid grid = GetNode<Grid>("/root/Main/World/Grid");
		grid.GridClicked += HandleGridClicked;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void HandleGridClicked(Vector2I cell)
	{
		if (GridLocations.Contains(cell))
		{
			GD.Print("Clicked Enemy Ship!");
		}
	}
	
}
