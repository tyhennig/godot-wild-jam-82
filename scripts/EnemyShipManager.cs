using Godot;
using System;

/// <summary>
/// Contains the logic / algorithm for deciding how many and where to place enemy ships on the grid
/// </summary>
public partial class EnemyShipManager : Node
{
	[Signal]
	public delegate void AllShipsDestroyedEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameManager.Instance.NewRound += PlaceEnemyShips;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void PlaceEnemyShips(int roundNumber)
	{
		GD.Print("Placing Enemy Ships!");
		var enemyScene = GD.Load<PackedScene>("res://scenes/enemy_ship.tscn");
		EnemyShip instance = enemyScene.Instantiate() as EnemyShip;
		instance.ShipDestroyed += OnShipDestroyed;

		instance.GridLocations = [new Vector2I(0, 0), new Vector2I(0, 1), new Vector2I(0, 2)];
		AddChild(instance);
	}

	public void OnShipDestroyed(EnemyShip ship)
	{
		int remainingShips = GetTree().GetNodeCountInGroup("enemy_ships");

		ship.QueueFree();
		// If this is the last ship to be destroyed, send signal
		if (remainingShips <= 1)
		{
			EmitSignal(SignalName.AllShipsDestroyed);
		}
	}

}
