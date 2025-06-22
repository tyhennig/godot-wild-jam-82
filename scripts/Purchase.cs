using Godot;
using System;

public partial class Purchase : ItemList
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Connect to the ItemSelected signal
		ItemSelected += OnItemSelected;
	}

	private void OnItemSelected(long index)
	{
		// Handle different upgrades based on index
		switch (index)
		{
			case 0:
				// Handle first item selection
				GD.Print($"Selected item at index {index}");
				break;
			case 1:
				// Handle second item selection
				GD.Print($"Selected item at index {index}");
				break;
			case 2:
				// Handle second item selection
				GD.Print($"Selected item at index {index}");
				break;
			case 3:
				// Handle second item selection
				GD.Print($"Selected item at index {index}");
				break;
			// Add more cases as needed
		}
	}

}
