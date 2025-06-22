using Godot;
using Godot.Collections;
using GridWreck;
using static Godot.Vector2I;
using System;

public partial class Circle_5x5 : CursorShape
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override string GetDescription()
    {
        return "CIRCLE 5x5";

    }

    public override Array<Vector2I> GetCursorCells(Vector2I vec2)
    {
		Array<Vector2I> rv = [];
		for (int x = -2; x <= 2; x++)
		{
			for (int y = -2; y <= 2; y++)
			{
				if (x == -2 && y == -2)
					continue;
				if (x == 2 && y == -2)
					continue;
				if (x == -2 && y == 2)
					continue;
				if (x == 2 && y == 2)
					continue;

				rv.Add(vec2 + new Vector2I(x, y));
			}
		}
		return rv;
    }

    public override string GetSceneLocation()
    {
		return "res://scenes/cursor_shapes/circle_5x5.tscn";

    }

}
