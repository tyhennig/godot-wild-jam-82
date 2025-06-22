using Godot;
using Godot.Collections;
using System;

namespace GridWreck
{
	public abstract partial class CursorShape : Node2D
	{
		public abstract String GetDescription();
		public abstract Array<Vector2I> GetCursorCells(Vector2I vec2);
		public abstract String GetSceneLocation();
	}

}
