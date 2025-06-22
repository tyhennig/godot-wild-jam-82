using Godot;
using System;

public partial class ScanList : ItemList
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
				// quad 5x5
				if (_onPurchasePressed(2, index))
				{
					// SignalBus.Instance.EmitSignal(SignalBus.SignalName.ScanCursorPurchased, );
				}
				break;
			case 1:
				// circle 5x5
				if (_onPurchasePressed(3, index))
				{
					// SignalBus.Instance.EmitSignal(SignalBus.SignalName.ScanCursorPurchased, );
				}
				break;
			case 2:
				// row x3
				if (_onPurchasePressed(5, index))
				{
					// SignalBus.Instance.EmitSignal(SignalBus.SignalName.ScanCursorPurchased, );
				}
				break;
			case 3:
				// column x3
				if (_onPurchasePressed(5, index))
				{
					// SignalBus.Instance.EmitSignal(SignalBus.SignalName.ScanCursorPurchased, );
				}
				break;
		}
	}

	Boolean _onPurchasePressed(int cost, long index)
	{
		if (GameManager.Instance.Currency >= cost)
		{
			SetItemDisabled((int)index, true);
			GameManager.Instance.Currency = GameManager.Instance.Currency - cost;
			return true;
		}
		return false;
	}
}
