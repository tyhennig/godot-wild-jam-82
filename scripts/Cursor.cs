using Godot;
using System;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class Cursor : Node2D
{
	[Export]
	public Vector2I MinVec2 { get; set; } = new Vector2I(61, 37);
	[Export]
	public Vector2I MaxVec2 { get; set; } = new Vector2I(373, 349);
	[Export]
	public Vector2I CellSize { get; set; } = new Vector2I(32, 32);
	[Export]
	public Vector2I GridOrigin { get; set; } = new Vector2I(61, 37);
	public GridWreck.CursorShape CursorShape { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (CursorShape == null)
		{
			GD.PushError("Cursor Requires a Shape!");
		}
		else
		{
			AddChild(CursorShape);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (IsInstanceValid(CursorShape))
		{
			CursorShape.GlobalPosition = _snapCursor();
		}
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
				Vector2I gridCell = _getMousePositionInGridCoords();
				GD.Print("Clicked Grid Coordinate: ", gridCell);
				SignalBus.Instance.EmitSignal(SignalBus.SignalName.GridCellsSelected, CursorShape.GetCursorCells(gridCell));
			}
		}
	}

	private Vector2 _snapCursor()
	{
		// snapLocation = snapLocation * CellSize;
		Vector2 snapLocation = _getMousePositionInGridCoords() * CellSize;
		// GD.Print("times cell size: ", snapLocation);
		snapLocation = snapLocation + (CellSize / 2) + GridOrigin + new Vector2(-1f, 1f);
		// Add one for pixel correction
		// GD.Print("plus half cell: ", snapLocation);
		return snapLocation;
	}

	private Vector2I _getMousePositionInGridCoords()
	{
		// GD.Print("Mouse Pos: ", GetViewport().GetMousePosition());
		Vector2 clampedMouseVec2 = GetViewport().GetMousePosition().Clamp(MinVec2, MaxVec2);
		// GD.Print("clampedMouseVec2: ", clampedMouseVec2);
		Vector2 mouseVec2WithOriginZero = clampedMouseVec2 - GridOrigin;
		// GD.Print("mouseVec2WithOriginZero: ", mouseVec2WithOriginZero);
		return (Vector2I)(mouseVec2WithOriginZero / CellSize).Floor();
	}

    internal void OnMouseExited()
    {
		QueueFree();
    }

}
