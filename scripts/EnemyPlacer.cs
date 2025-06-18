using Godot;
using System;

/// <summary>
/// Contains the logic / algorithm for deciding how many and where to place enemy ships on the grid
/// </summary>
public partial class EnemyPlacer : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void PlaceEnemyShips(int roundNumber)
	{
		var enemyScene = GD.Load<PackedScene>("res://scenes/enemy_ship.tscn");
		EnemyShip instance = enemyScene.Instantiate() as EnemyShip;
		instance.GridLocations = [new Vector2I(0, 0), new Vector2I(0, 1), new Vector2I(0, 2)];
		AddChild(instance);
	}
}
