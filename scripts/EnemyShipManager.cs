using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Contains the logic / algorithm for deciding how many and where to place enemy ships on the grid
/// </summary>
public partial class EnemyShipManager : Node
{
	[Signal]
	public delegate void AllShipsDestroyedEventHandler();

	[Export]
	public int ShipsCount; // Maximum number of ships to place per round
	[Export]
	public int ShipsAddedPerRound; // Incrementor for number of ships to place per victory

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

		for (int i = 0; i < ShipsCount; i++)
		{
			// Place the ships in random locations on the grid
			List<Vector2I> ShipSpots = RandomLocationPicker();
			
			EnemyShip instance = enemyScene.Instantiate() as EnemyShip;
			instance.ShipDestroyed += OnShipDestroyed;

			instance.GridLocations = [ShipSpots[0], ShipSpots[1], ShipSpots[2]];
			GD.Print("Placing Enemy Ship at: ", ShipSpots[0], ShipSpots[1], ShipSpots[2]);
			AddChild(instance);
		}
	}

	/// <summary>
	/// Randomly picks a location on the grid to place a ship.
	/// The ship is assumed to be 3 spots wide, so it will check if there is enough space to place the ship or call itself again to retry.
	/// If there is an overlap with existing ships, it will call itself recursively to try again.
	/// </summary>
	/// <returns>Ship grid cells as a list of 2D Vectors</returns>
	private List<Vector2I> RandomLocationPicker()
	{
		// Random location for first ship-spot to be placed (assuming all ships are 3 spots wide)
		int randomX = (int)GD.RandRange(0, 9);
		int randomY = (int)GD.RandRange(0, 9);
		int axis = (int)GD.RandRange(0, 1); // 0 for X, 1 for Y

		List<Vector2I> ShipSpots = new List<Vector2I>();

		if (axis == 0) // X axis
		{
			if (randomX + 2 < 10) // Check if the ship will extend beyond the grid
			{
				if (CheckShipOverlap(ShipSpots, randomX, randomY))
				{
					return RandomLocationPicker(); // Try again if there is an overlap
				}
				else
				{
					ShipSpots.Add(new Vector2I(randomX, randomY)); // First spot
					ShipSpots.Add(new Vector2I(randomX + 1, randomY)); // Second spot
					ShipSpots.Add(new Vector2I(randomX + 2, randomY)); // Third spot

					return ShipSpots;
				}
			}
			else
			{
				GD.Print("Cannot place ship at X: ", randomX, " Y: ", randomY);

				// Call this function recursively to try again
				return RandomLocationPicker();
			}
		}
		else // Y axis
		{
			if (randomY + 2 < 10) // Check if the ship will extend beyond the grid
			{
				if (CheckShipOverlap(ShipSpots, randomX, randomY))
				{
					return RandomLocationPicker(); // Try again if there is an overlap
				}
				else
				{
					ShipSpots.Add(new Vector2I(randomX, randomY)); // First spot
					ShipSpots.Add(new Vector2I(randomX, randomY + 1)); // Second spot
					ShipSpots.Add(new Vector2I(randomX, randomY + 2)); // Third spot

					return ShipSpots;
				}
			}
			else
			{
				GD.Print("Cannot place ship at X: ", randomX, " Y: ", randomY);

				// Call this function recursively to try again
				return RandomLocationPicker();
			}
		}
	}

	/// <summary>
	/// Check if there is a ship spot already at the given coordinates
	/// </summary>
	/// <param name="shipSpots">Existing Ship Spots</param>
	/// <param name="randomX">Current X contendor</param>
	/// <param name="randomY">Current Y contendor</param>
	/// <returns></returns>
	private bool CheckShipOverlap(List<Vector2I> shipSpots, int randomX, int randomY)
	{
		foreach (var spot in shipSpots)
		{
			if (spot.X == randomX && spot.Y == randomY)
			{
				return true; // Overlap found
			}
		}
		return false; // No overlap found		
	}

	private int CheckRounds()
	{
		// Check what the round number is in the GameManager
		return GameManager.Instance.RoundNumber;
	}

	public void OnShipDestroyed(EnemyShip ship)
	{
		int remainingShips = GetTree().GetNodeCountInGroup("enemy_ships");

		ship.QueueFree();
		// If this is the last ship to be destroyed, send signal
		if (remainingShips <= 1)
		{
			EmitSignal(SignalName.AllShipsDestroyed);
			ShipsCount += ShipsAddedPerRound; // Increment the number of ships for the next round
		}
	}
}
