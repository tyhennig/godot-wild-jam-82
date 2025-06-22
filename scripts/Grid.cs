using Godot;
using GridWreck;
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
	private PackedScene _defaultFireShape;

	[Export]
	private PackedScene _defaultScanShape;
	private List<Node> _hitIcons = new List<Node>();
	private Godot.Collections.Dictionary<Vector2I, Label> _scanIcons = new();
	private Sprite2D _gridSprite;
	// private Area2D _area2D;
	private Vector2 _spriteSize;
	private Vector2 _cellSize;
	private bool _mouseOverGrid;


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

		// _area2D = GetNode<Area2D>("GridArea");
		// _area2D.MouseEntered += OnMouseEnteredGrid;
		// _area2D.MouseExited += OnMouseExitedGrid;

		if (_defaultFireShape == null || _defaultScanShape == null)
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
		GetNodeOrNull("Cursor")?.QueueFree();
    }


	private void OnMouseEnteredGrid()
	{
		GameManager.States currentState = GameManager.Instance.CurrentState;
		GD.Print("Mouse entered grid, current state: ", currentState);
		if (currentState is not (GameManager.States.Firing or GameManager.States.Scanning))
			return;
			
		// Create the Cursor
		PackedScene shapeScene = currentState is GameManager.States.Firing
			? GameManager.Instance.ActiveFireShape : GameManager.Instance.ActiveScanShape;
		CursorShape cursorShape;
		if (shapeScene == null)
		{
			cursorShape = currentState is GameManager.States.Firing
				? _defaultFireShape.Instantiate() as CursorShape : _defaultScanShape.Instantiate() as CursorShape;
		}
		else
		{
			cursorShape = shapeScene.Instantiate() as CursorShape;
		}

		Cursor cursor = GD.Load<PackedScene>("res://scenes/cursor.tscn").Instantiate() as Cursor;
		cursor.CursorShape = cursorShape;

		AddChild(cursor);
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public override void _Process(double delta)
	{
		if (_isMouseOverGrid())
		{

			if (_mouseOverGrid)
			{
				// The mouse is grid and it was previously
				return;
			}
			else
			{
				// Mouse is on grid and it was not previously
				_mouseOverGrid = true;
				OnMouseEnteredGrid();
			}
		}
		else
		{
			// Mouse is not and grid, but it was previously
			if (_mouseOverGrid)
			{
				_mouseOverGrid = false;
				OnMouseExitedGrid();
			}
			_mouseOverGrid = false;
		}
	}

	private bool _isMouseOverGrid()
	{
		var mousePos = GetGlobalMousePosition();
		var rect = new Rect2(_gridSprite.GlobalPosition - (_spriteSize / 2), _spriteSize);
		return rect.HasPoint(mousePos);
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
