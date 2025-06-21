using Godot;
using Godot.Collections;
using System;

public abstract partial class CursorShape : Node2D
{
	public abstract Array<Vector2I> GetCursorCells(Vector2I vec2);
}
