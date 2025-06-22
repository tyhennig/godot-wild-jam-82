using Godot;
using System;
using Rect2 = Godot.Rect2;
using System.Runtime.ConstrainedExecution;
using System.Collections.Generic;
using Godot.Collections;
using System.Threading.Tasks;

public partial class Grid : Node2D
{
	[Signal]
	public delegate void GridClickedEventHandler(Vector2I cell);

	[Export]
	private PackedScene _defaultCursorShape;

	private List<Node> _hitIcons = new List<Node>();
	private Godot.Collections.Dictionary<Vector2I, Label> _scanIcons = new();
	private Sprite2D _gridSprite;
	private Area2D _area2D;
	private Vector2 _spriteSize;
	private Vector2 _cellSize;


	private readonly int GridWidth = 10;
	private readonly int GridHeight = 10;

	public int GetGridWidth()
	{
		return GridWidth;
	}
	public int GetGridHeight()
	{
		return GridHeight;
	}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SignalBus.Instance.ShipHit += OnShipHit;
		GameManager.Instance.ScannedGrid += OnGridCellsSelectedAsync;
		GameManager.Instance.NewRound += OnNewRound;

		_gridSprite = GetNode<Sprite2D>("GridSprite");

		_spriteSize = _gridSprite.Texture.GetSize() * _gridSprite.Scale;
		_cellSize = new Vector2(_spriteSize.X / GridWidth, _spriteSize.Y / GridHeight);

		_area2D = GetNode<Area2D>("GridArea");
		_area2D.MouseEntered += OnMouseEnteredGrid;
		_area2D.MouseExited += OnMouseExitedGrid;

		if (_defaultCursorShape == null)
		{
			GD.PushError("Must Define Default Cursor Shape");
		}
	}

	// Method for displaying the scanned count in the grid
	private async void OnGridCellsSelectedAsync(Array<Vector2I> cells)
	{

		if (GameManager.Instance.CurrentState is not GameManager.States.Scanning)
			return;

		// Wait for all the ships to be removed
		await ToSignal(GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);

		foreach(var cell in cells)
		{
			Label scanIcon;
			if (_scanIcons.ContainsKey(cell))
			{
				scanIcon = _scanIcons[cell];
			}
			else
			{
				scanIcon = new();
				_scanIcons.Add(cell, scanIcon);
				AddChild(scanIcon);
			}

			scanIcon.Text = GameManager.Instance.ShipsDetected.ToString();
			scanIcon.Position = GridVecToLocalVec(cell) - (_cellSize / 2);
		}
	}

    private void OnMouseExitedGrid()
	{
		GD.Print("Removing Cursor");
    }


	private void OnMouseEnteredGrid()
	{
		// Create the Cursor
		PackedScene shapeScene = GameManager.Instance.ActiveCursorShape;
		CursorShape cursorShape;
		if (shapeScene == null)
		{
			cursorShape = _defaultCursorShape.Instantiate() as CursorShape;
		}
		else
		{
			cursorShape = shapeScene.Instantiate() as CursorShape;
		}

		Cursor cursor = GD.Load<PackedScene>("res://scenes/cursor.tscn").Instantiate() as Cursor;
		cursor.CursorShape = cursorShape;

		_area2D.MouseExited += cursor.OnMouseExited;

		AddChild(cursor);
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public override void _Process(double delta)
	{
		
	}

	private Vector2I GetGridCell(Vector2 mousePosition)
	{

		// Convert to local coordinates relative to grid origin
		Vector2 localPos = ToLocal(mousePosition);
		Vector2 adjustedPos = localPos + (_spriteSize / 2);

		return (Vector2I)(adjustedPos / _cellSize);
	}

	public Vector2I GridVecToLocalVec(Vector2I cell)
	{
		Vector2 localVec = (cell * _cellSize) - (_spriteSize / 2);
		return (Vector2I)((Vector2I)localVec + (_cellSize / 2));
    }

	private void OnShipHit(Vector2I cell)
	{
		var hitIconScene = GD.Load<PackedScene>("res://scenes/fire.tscn");
		Node2D hitIcon = hitIconScene.Instantiate() as Node2D;
		hitIcon.Position = GridVecToLocalVec(cell);

		AddChild(hitIcon);
		this._hitIcons.Add(hitIcon);
	}

	private void OnNewRound(int roundNumber)
	{
		GD.Print("Cleaning up hit icons!");
		foreach (var item in _hitIcons)
		{
			item.QueueFree();
		}

		foreach (var item in _scanIcons.Values)
		{
			item.QueueFree();
		}

		_hitIcons.Clear();
		_scanIcons.Clear();
    }
}
