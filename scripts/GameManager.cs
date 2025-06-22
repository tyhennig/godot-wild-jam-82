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
	public int RoundNumber { get; private set; }
	public int Currency { get; set; }
	public int StartingScans { get; set; }
	public int StartingBullets { get; set; }
	public int CurrentScans { get; set; } = 5;
	public int CurrentBullets { get; set; } = 5;
	public int ShipsDetected { get; set; }
	public PackedScene ActiveFireShape { get; set; }
	public PackedScene ActiveScanShape { get; set; }

	private Boolean _isGameOver;
	
	
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

		var firePanel = GetNode<FirePanel>("/root/Main/UI/FirePanel");
		var scanPanel = GetNode<ScanPanel>("/root/Main/UI/ScanPanel");

		firePanel.FirePressed += OnFirePressed;
		scanPanel.ScanPressed += OnScanPressed;

		SignalBus.Instance.GridCellsSelected += OnGridClicked;
		SignalBus.Instance.ShipScanned += OnShipScanned;

		var mainMenuNode = GetNode<MainMenu>("/root/Main/UI/MainMenu");
		mainMenuNode.StartGame += OnStartGame;

		var enemyManagerNode = GetNode<EnemyShipManager>("/root/Main/World/Grid/EnemyShipManager");
		enemyManagerNode.AllShipsDestroyed += OnRoundEnd;

		var shopScene = GetNode<Shop>("/root/Main/UI/Shop");
		shopScene.ShopClosed += _initializeNewRound;

	
		SignalBus.Instance.FireShapeSelected += OnFireShapeSelected;
		SignalBus.Instance.ScanShapeSelected += OnScanShapeSelected;
	}

    private void OnScanShapeSelected(PackedScene shape)
    {
		ActiveScanShape = shape;
    }


    private void OnFireShapeSelected(PackedScene shape)
    {
		ActiveFireShape = shape;
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
		GD.Print("Starting new game!");
		RoundNumber = 0;
		Currency = 0;
		StartingScans = 15;
		StartingBullets = 10;
		CurrentScans = StartingScans;
		CurrentBullets = StartingBullets;
		_isGameOver = false;
		_initializeNewRound();
	}
	private void _initializeNewRound()
	{
		GD.Print("Initializing New Round!");
		ShipsDetected = 0;
		RoundNumber++;
		CurrentScans = StartingScans;
		CurrentBullets = StartingBullets;
		CurrentState = States.Nothing;
		EmitSignal(SignalName.NewRound, RoundNumber);
	}

 	private void OnShipScanned(Vector2I cell)
    {
		ShipsDetected++;
    }

	private void OnGridClicked(Array<Vector2I> cells)
	{
		GD.Print("Bullets: ", CurrentBullets);
		GD.Print("Scans: ", CurrentScans);
		switch (CurrentState)
		{
			case States.Firing:
				GD.Print("Firing on ", cells);
				if (CurrentBullets > 0)
				{
					CurrentBullets--;
				}
				if (CurrentBullets == 0)
				{
					CallDeferred("CheckGameOver");
				}

				EmitSignal(SignalName.FiredOnGrid, cells);
				break;
			case States.Scanning:
				
				ShipsDetected = 0;
				if (CurrentScans >= 1)
				{
					GD.Print("Scanning on ", cells);
					CurrentScans--;
					EmitSignal(SignalName.ScannedGrid, cells);
				}
				break;
			case States.Nothing:
				break;
		}
	}

	private async void OnRoundEnd()
	{
		if (_isGameOver)
			return;
		CurrentState = States.Nothing;
		// WHEN ROUND IS SUCCESSFUL
		GD.Print("OnRoundEnd!");
		await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);

		// var shopScene = GD.Load<PackedScene>("res://scenes/shop.tscn");
		// Shop shop = shopScene.Instantiate() as Shop;
		// shop.test = "Shop";
		// Increase the total points at the end of the round
		Currency += RoundNumber * 2 + 2;

		// shop.TreeExited += _initializeNewRound;

		var shopScene = GetNode<Shop>("/root/Main/UI/Shop");
		shopScene.Show();

		// AddChild(shop);
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
			_isGameOver = true;
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
