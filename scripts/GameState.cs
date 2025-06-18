using Godot;
using System;

public partial class GameState : Node
{
	public static GameState Instance { get; private set; }
	public int RoundNumber { get; private set; }
	public int Currency { get; private set; }
	public int StartingScans { get; private set; }
	public int StartingEnergy { get; private set; }
	public int CurrentScans { get; private set; }
	public int CurrentEnergy { get; private set; }

	private EnemyPlacer enemyPlacer;
	private States currentState;
	public enum States
	{
		Firing,
		Scanning,
		Shopping,
		Nothing
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;

		//Signal Setup
		var actionNode = GetNode<Actions>("/root/Main/UI/Actions");
		actionNode.FirePressed += OnFirePressed;
		actionNode.ScanPressed += OnScanPressed;

		var gridNode = GetNode<Grid>("/root/Main/World/Grid");
		gridNode.GridClicked += OnGridClicked;

		//Node References
		enemyPlacer = GetNode<EnemyPlacer>("/root/Main/World/EnemyPlacer");


		_initializeNewGameData();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey eventKey)
		{
			if (eventKey.Pressed && eventKey.Keycode == Key.N)
			{
				_initializeNewRound();
			}
		}
    }


	private void _initializeNewGameData()
	{
		RoundNumber = 0;
		Currency = 0;
		StartingScans = 3;
		StartingEnergy = 10;
		CurrentScans = StartingScans;
		CurrentEnergy = StartingEnergy;
		_initializeNewRound();
	}
	private void _initializeNewRound()
	{
		RoundNumber++;
		CurrentScans = StartingScans;
		CurrentEnergy = StartingEnergy;
		currentState = States.Nothing;
		enemyPlacer.PlaceEnemyShips(RoundNumber);
	}

	private void OnGridClicked(Vector2I cell)
    {
        switch (currentState)
		{
			case States.Firing:
				GD.Print("Firing on ", cell);
				if (CurrentEnergy >= 1) CurrentEnergy--;
				break;
			case States.Scanning:
				GD.Print("Scanning on ", cell);
				if (CurrentScans >= 1) CurrentScans--;
				break;
			case States.Nothing:
				break;
		}
    }


	private void OnFirePressed()
	{
		currentState = States.Firing;
	}

	private void OnScanPressed()
	{
		currentState = States.Scanning;
	}
}
