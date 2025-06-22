using Godot;
using Godot.Collections;
using static Godot.Vector2I;
using System;
using GridWreck;

public partial class CursorShape3x3 : CursorShape
{
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override Array<Vector2I> GetCursorCells(Vector2I vec2)
    {
		return [
				vec2 + Left,
				vec2 + Right,
				vec2 + Up,
				vec2 + Down,
				vec2 + Left + Up,
				vec2 + Left + Down,
				vec2 + Right + Up,
				vec2 + Right + Down,
				vec2
		];
    }

    public override string GetSceneLocation()
    {
		return "res://scenes/cursor_shapes/quad_3x3.tscn";
    }

    public override string GetDescription()
    {
		return "QUAD 3x3";
    }

}
