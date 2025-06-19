using Godot;
using System;
using Rect2 = Godot.Rect2;
using System.Runtime.ConstrainedExecution;

public partial class Grid : Node2D
{
	[Signal]
	public delegate void GridClickedEventHandler(Vector2I cell);

	private readonly int GridWidth = 10;
	private readonly int GridHeight = 10;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Called when an mouse input event occurs and converts to a screen coordinate
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton)
		{
			if (eventMouseButton.Pressed && eventMouseButton.ButtonIndex.Equals(MouseButton.Left))
			{
				// Get the sprite's size to determine clickable area
				var sprite = GetNode<Sprite2D>("GridSprite"); // Adjust the node name and path as needed
				if (sprite != null)
				{
					Vector2 spriteSize = sprite.Texture.GetSize() * sprite.Scale;
					Vector2 position = sprite.Position - (spriteSize / 2);

					// Create a Rect2 representing the sprite's bounds
					var spriteBounds = new Rect2(position, spriteSize);

					// Check if the mouse position is within the bounds
					if (spriteBounds.HasPoint(ToLocal(eventMouseButton.Position)))
					{
						Vector2I gridCell = GetGridCell(eventMouseButton.Position);
						GD.Print("Clicked Grid Coordinate: ", gridCell);
						EmitSignal(SignalName.GridClicked, gridCell );
					}
				}
			}
		}
	}

	private Vector2I GetGridCell(Vector2 mousePosition)
	{
		var sprite = GetNode<Sprite2D>("GridSprite");
		Vector2 spriteSize = sprite.Texture.GetSize() * sprite.Scale;
		Vector2 cellSize = new Vector2(spriteSize.X / GridWidth, spriteSize.Y / GridHeight);
    
		// Convert to local coordinates relative to grid origin
		Vector2 localPos = ToLocal(mousePosition);
		Vector2 adjustedPos = localPos + (spriteSize / 2);
    
		return (Vector2I)(adjustedPos / cellSize);
	}

}
