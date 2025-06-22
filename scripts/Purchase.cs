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
				// Increase Bullets
				OnPurchasePressed();
				break;
			case 1:
				// Handle second item selection
				GD.Print($"Selected item at index {index}");
				// Increase Scans
				OnPurchasePressed();
				break;
			case 2:
				// Upgrade Cursor Shape
				GD.Print($"Selected item at index {index}");
				OnPurchasePressed();
				break;
			case 3:
				// Upgrade Cursor Size
				GD.Print($"Selected item at index {index}");
				OnPurchasePressed();
				break;
			// Add more cases as needed
		}
	}

	private void OnPurchasePressed()
	{
		if (GameManager.Instance.Currency > 0)
		{
			GameManager.Instance.Currency--;
			
		}
	}

}
