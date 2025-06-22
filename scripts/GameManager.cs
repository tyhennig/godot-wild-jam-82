using System;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;

public partial class GameManager : Node
{
	[Signal]
	public delegate void NewRoundEventHandler(int roundNumber);
	[Signal]
	public delegate void FiredOnGridEventHandler(Array<Vector2I> cells);
	[Signal]
	public delegate void ScannedGridEventHandler(Array<Vector2I> cells);
	[Signal]
	public delegate void GameOverEventHandler();


	public static GameManager Instance { get; private set; }
	public int RoundNumber { get; private set; } = 0;
	public int Currency { get; private set; }
	public int StartingScans { get; private set; }
	public int StartingEnergy { get; private set; }
	public int CurrentScans { get; private set; } = 5;
	public int CurrentEnergy { get; private set; } = 5;
	public int ShipsDetected { get; set; }
	public PackedScene ActiveCursorShape { get; set; }
	
	public States CurrentState { get; private set; } = States.Nothing;
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

		SignalBus.Instance.GridCellsSelected += OnGridClicked;
		SignalBus.Instance.ShipScanned += OnShipScanned;

		var mainMenuNode = GetNode<MainMenu>("/root/Main/UI/MainMenu");
		mainMenuNode.StartGame += OnStartGame;

		var enemyManagerNode = GetNode<EnemyShipManager>("/root/Main/World/Grid/EnemyShipManager");
		enemyManagerNode.AllShipsDestroyed += OnRoundEnd;

		GetNode<Button>("/root/Main/UI/ThreeCursor").Pressed += ActivateThree;
		GetNode<Button>("/root/Main/UI/OneCursor").Pressed += ActivateOne;
		GetNode<Button>("/root/Main/UI/ThreeThreeCursor").Pressed += ActivateThreeThreef;

	}



    private void ActivateThreeThreef()
    {
		ActiveCursorShape = GD.Load<PackedScene>("res://scenes/cursor_shapes/cursor_shape_3x3.tscn");
    }


    private void ActivateOne()
    {
		ActiveCursorShape = GD.Load<PackedScene>("res://scenes/cursor_shapes/cursor_shape_1x1.tscn");
    }


    private void ActivateThree()
    {
		ActiveCursorShape = GD.Load<PackedScene>("res://scenes/cursor_shapes/cursor_shape_3x1.tscn");
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
		StartingEnergy = 5;
		CurrentScans = StartingScans;
		CurrentEnergy = StartingEnergy;
		_initializeNewRound();
	}
	private void _initializeNewRound()
	{
		GD.Print("Initializing New Round!");
		ShipsDetected = 0;
		RoundNumber++;
		StartingEnergy++;
		StartingScans++;
		CurrentScans = StartingScans;
		CurrentEnergy = StartingEnergy;
		CurrentState = States.Nothing;
		EmitSignal(SignalName.NewRound, RoundNumber);
	}

 	private void OnShipScanned(Vector2I cell)
    {
		ShipsDetected++;
    }

	private void OnGridClicked(Array<Vector2I> cells)
	{
		GD.Print("Energy: ", CurrentEnergy);
		switch (CurrentState)
		{
			case States.Firing:
				GD.Print("Firing on ", cells);
				if (CurrentEnergy > 0)
				{
					CurrentEnergy--;
				}
				if (CurrentEnergy == 0)
				{
					CallDeferred("CheckGameOver");
				}

				EmitSignal(SignalName.FiredOnGrid, cells);
				break;
			case States.Scanning:
				GD.Print("Scanning on ", cells);
				ShipsDetected = 0;
				if (CurrentScans >= 1) CurrentScans--;
				EmitSignal(SignalName.ScannedGrid, cells);
				break;
			case States.Nothing:
				break;
		}
	}

	private async void OnRoundEnd()
	{
		// WHEN ROUND IS SUCCESSFUL
		GD.Print("OnRoundEnd!");
		await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);

		var shopScene = GD.Load<PackedScene>("res://scenes/shop.tscn");
		Shop shop = shopScene.Instantiate() as Shop;
		shop.test = "Shop Screen!";

		shop.TreeExited += _initializeNewRound;

		AddChild(shop);
	}

	private void OnFirePressed()
	{
		GD.Print("Fire Pressed");
		CurrentState = States.Firing;
	}

	private void OnScanPressed()
	{
		GD.Print("Scan Pressed");
		CurrentState = States.Scanning;
	}

	private async void CheckGameOver()
	{
		// THIS IS SO JANKY
		await ToSignal(GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);
		GD.Print("Checking Game over, ships left: ", GetTree().GetNodeCountInGroup("enemy_ships"));
		if (GetTree().GetNodeCountInGroup("enemy_ships") > 0)
		{
			CurrentState = States.Nothing;

			// Load and Display the Game over screen, wait 1s and then go to the menu via GameOver Signal
			var gameOverScene = GD.Load<PackedScene>("res://scenes/game_over.tscn");
			Node gameOverNode = gameOverScene.Instantiate();

			AddChild(gameOverNode);

			await ToSignal(GetTree().CreateTimer(2.0f), SceneTreeTimer.SignalName.Timeout);

			gameOverNode.QueueFree();

			// Do game over logic
			EmitSignal(SignalName.GameOver);
		}
	}
}
