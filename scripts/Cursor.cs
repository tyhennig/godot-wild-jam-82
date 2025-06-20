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
	Sprite2D sprite2D;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite2D = GetNode<Sprite2D>("CursorSprite");
		if (sprite2D == null)
		{
			GD.PushError("Cursor Requires a Sprite!");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		sprite2D.Position = _snapCursor();
	}

	private Vector2 _snapCursor()
	{
		Vector2 mousePos = GetViewport().GetMousePosition().Clamp(MinVec2, MaxVec2);
		// GD.Print("Mouse pos: ", mousePos);
		Vector2 snapLocation = ((mousePos - GridOrigin));
		// GD.Print("minusGrid: ", snapLocation);
		snapLocation = (snapLocation / CellSize).Floor();
		// GD.Print("Floored: ", snapLocation);
		snapLocation = snapLocation * CellSize;
		// GD.Print("times cell size: ", snapLocation);
		snapLocation = snapLocation + (CellSize / 2) + GridOrigin + Vector2.One; // Add one for pixel correction
		// GD.Print("plus half cell: ", snapLocation);
		return snapLocation;
	}
}
