using Godot;
using Godot.Collections;
using GridWreck;
using System;
using System.Collections.Generic;

public partial class CursorShape1x1 : CursorShape
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override Array<Vector2I> GetCursorCells(Vector2I vec2)
    {
		return [vec2];
    }

    public override string GetSceneLocation()
    {
		return "res://scenes/cursor_shapes/quad_1x1.tscn";
    }

    public override string GetDescription()
    {
		return "QUAD 1x1";
    }

}
