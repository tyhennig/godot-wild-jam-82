using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Numerics;

public partial class GameState : Node
{
	[Signal]
	public delegate void NewRoundEventHandler(int roundNumber);

	[Signal]
	public delegate void FiredOnGridEventHandler(Array<Vector2I> cells);
	[Signal]
	public delegate void ScannedGridEventHandler(Array<Vector2I> cells);

	private PackedScene mainMenuScene;


	public static GameState Instance { get; private set; }
	public int RoundNumber { get; private set; }
	public int Currency { get; private set; }
	public int StartingScans { get; private set; }
	public int StartingEnergy { get; private set; }
	public int CurrentScans { get; private set; }
	public int CurrentEnergy { get; private set; }

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

		var mainMenuNode = GetNode<MainMenu>("/root/Main/UI/MainMenu");
		mainMenuNode.StartGame += OnStartGame;

		var enemyManagerNode = GetNode<EnemyShipManager>("/root/Main/World/Grid/EnemyShipManager");
		enemyManagerNode.AllShipsDestroyed += OnRoundEnd;
	}


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event)
	{
		
    }

    private void OnStartGame()
    {
		_initializeNewGameData();
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
		GD.Print("Initializing New Round!");
		RoundNumber++;
		CurrentScans = StartingScans;
		CurrentEnergy = StartingEnergy;
		CurrentState = States.Nothing;
		EmitSignal(SignalName.NewRound, RoundNumber);
	}

	private void OnGridClicked(Vector2I cell)
    {
		Array<Vector2I> cells;
        switch (CurrentState)
		{
			case States.Firing:
				GD.Print("Firing on ", cell);
				if (CurrentEnergy >= 1) CurrentEnergy--;
				cells = [cell];
				EmitSignal(SignalName.FiredOnGrid, cells);
				break;
			case States.Scanning:
				GD.Print("Scanning on ", cell);
				if (CurrentScans >= 1) CurrentScans--;
				cells = [cell];
				EmitSignal(SignalName.ScannedGrid, cells);
				break;
			case States.Nothing:
				break;
		}
    }

	private void OnRoundEnd()
	{
		var shopScene = GD.Load<PackedScene>("res://scenes/shop.tscn");
		Shop shop = shopScene.Instantiate() as Shop;
		shop.test = "It Succeeded";

		shop.TreeExited += _initializeNewRound;

		AddChild(shop);
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
