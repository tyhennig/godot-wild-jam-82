using Godot;
using System;
using System.Runtime.ConstrainedExecution;

public partial class Grid : Node2D
{
	[Signal]
	public delegate void GridClickedEventHandler(Vector2I cell);

	private readonly int GridWidth = 16;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton)
		{
			if (eventMouseButton.Pressed && eventMouseButton.ButtonIndex.Equals(MouseButton.Left))
			{
				// If the mouse is currently in the grid space
				Vector2 localMouseVec2 = ToLocal(eventMouseButton.Position);
				if (localMouseVec2.Abs().X <= 160 && localMouseVec2.Abs().Y <= 160)
				{
					Vector2I gridCell = _localMouseVec2ToGridVec2(localMouseVec2);
					GD.Print("Clicked Grid Coordinate: ", gridCell);
					EmitSignal(SignalName.GridClicked, gridCell);
				}
			}
		}
	}

	private Vector2I _localMouseVec2ToGridVec2(Vector2 mouseVec2)
	{
		// Shift coordinate into positive space
		// Local coordinate plus 
		Vector2 posMouseVec2 = mouseVec2 + new Vector2(160, 160);

		Vector2I gridVec2 = (Vector2I)(posMouseVec2 / 20);
		gridVec2 = gridVec2.Clamp(0, 15);

		// If we want to use a single integer to represent the cells use this:
		//return gridVec2.X + (gridVec2.Y * GridWidth);

		return gridVec2;
	}

}
