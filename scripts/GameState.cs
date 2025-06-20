using Godot;
using System;

public partial class GameState : Node
{
	[Signal]
	public delegate void NewRoundEventHandler(int roundNumber);
	private PackedScene mainMenuScene;


	public static GameState Instance { get; private set; }
	public int RoundNumber { get; private set; }
	public int Currency { get; private set; }
	public int StartingScans { get; private set; }
	public int StartingEnergy { get; private set; }
	public int CurrentScans { get; private set; }
	public int CurrentEnergy { get; private set; }

	private EnemyPlacer enemyPlacer;
	public States CurrentState { get; private set; }
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

		mainMenuScene = GD.Load<PackedScene>("res://scenes/main_menu.tscn");

		//Signal Setup
		var actionNode = GetNode<Actions>("/root/Main/UI/Actions");
		actionNode.FirePressed += OnFirePressed;
		actionNode.ScanPressed += OnScanPressed;

		var gridNode = GetNode<Grid>("/root/Main/World/Grid");
		gridNode.GridClicked += OnGridClicked;
		gridNode.Show(); // TODO: The grid node wasn't showing unless I forced it to show here, investigate why. Didn't see this issue until after pulling the commit before this one.

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
		CurrentState = States.Nothing;
		EmitSignal(SignalName.NewRound, RoundNumber);
	}

	private void OnGridClicked(Vector2I cell)
    {
        switch (CurrentState)
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
		CurrentState = States.Firing;
	}

	private void OnScanPressed()
	{
		CurrentState = States.Scanning;
	}
}
