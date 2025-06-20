using Godot;
using System;
using Rect2 = Godot.Rect2;
using System.Runtime.ConstrainedExecution;
using System.Collections.Generic;

public partial class Grid : Node2D
{
	[Signal]
	public delegate void GridClickedEventHandler(Vector2I cell);

	private List<Node> _hitIcons = new List<Node>();
	private Sprite2D _gridSprite;
	private Vector2 _spriteSize;
	private Vector2 _cellSize;


	private readonly int GridWidth = 10;
	private readonly int GridHeight = 10;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SignalBus.Instance.ShipHit += OnShipHit;
		GameManager.Instance.NewRound += OnNewRound;

		_gridSprite = GetNode<Sprite2D>("GridSprite");
		_spriteSize = _gridSprite.Texture.GetSize() * _gridSprite.Scale;
		_cellSize = new Vector2(_spriteSize.X / GridWidth, _spriteSize.Y / GridHeight);
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public override void _Process(double delta)
	{
	}

	// Called when an mouse input event occurs and converts to a screen coordinate
	public override void _Input(InputEvent @event)
	{
		if (GameManager.Instance.CurrentState is not (GameManager.States.Firing or GameManager.States.Scanning))
		{
			// If we aren't firing or scanning just skip input processing
			return;
		}
		
		if (@event is InputEventMouseButton eventMouseButton)
		{
			if (eventMouseButton.Pressed && eventMouseButton.ButtonIndex.Equals(MouseButton.Left))
			{
				// Get the sprite's size to determine clickable area
				if (_gridSprite != null)
				{
					Vector2 position = _gridSprite.Position - (_spriteSize / 2);

					// Create a Rect2 representing the sprite's bounds
					var spriteBounds = new Rect2(position, _spriteSize);

					// Check if the mouse position is within the bounds
					if (spriteBounds.HasPoint(ToLocal(eventMouseButton.Position)))
					{
						Vector2I gridCell = GetGridCell(eventMouseButton.Position);
						GD.Print("Clicked Grid Coordinate: ", gridCell);
						EmitSignal(SignalName.GridClicked, gridCell);
					}
				}
			}
		}
	}

	private Vector2I GetGridCell(Vector2 mousePosition)
	{

		// Convert to local coordinates relative to grid origin
		Vector2 localPos = ToLocal(mousePosition);
		Vector2 adjustedPos = localPos + (_spriteSize / 2);

		return (Vector2I)(adjustedPos / _cellSize);
	}

	private Vector2I _gridVecToLocalVec(Vector2I cell)
	{
		Vector2 localVec = (cell * _cellSize) - (_spriteSize / 2);
		return (Vector2I)localVec;
    }


	private void OnShipHit(Vector2I cell)
	{
		var hitIconScene = GD.Load<PackedScene>("res://scenes/fire.tscn");
		Node2D hitIcon = hitIconScene.Instantiate() as Node2D;
		hitIcon.Position = _gridVecToLocalVec(cell) + (_cellSize / 2);

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

		_hitIcons.Clear();
    }
}
