using Godot;
using System;

public partial class ResourceList : ItemList
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Connect to the ItemSelected signal
		ItemSelected += OnItemSelected;
	}

	private void OnItemSelected(long index)
	{
		if (GameManager.Instance.Currency < 1)
			return;
		GD.Print("Item selected. Index: ", index);
		// Handle different upgrades based on index
		switch (index)
		{
			case 0:
				// Handle first item selection
				// Increase Bullets
				GameManager.Instance.StartingBullets++;
				_onPurchasePressed();
				break;
			case 1:
				// Handle second item selection
				// Increase Scans
				GameManager.Instance.StartingScans++;
				_onPurchasePressed();
				break;
		}
	}

	private void _onPurchasePressed()
	{
		if (GameManager.Instance.Currency > 0)
		{
			GameManager.Instance.Currency--;
		}
	}

}
