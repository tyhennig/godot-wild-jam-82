using Godot;
using System;

public partial class FireList : ItemList
{
// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Connect to the ItemSelected signal
		ItemSelected += OnItemSelected;
	}

	private void OnItemSelected(long index)
	{
		GD.Print("Item selected. Index: ", index);
		// Handle different upgrades based on index
		switch (index)
		{
			case 0:
				// quad 3x1

				if (_onPurchasePressed(2, index))
				{
					SignalBus.Instance.EmitSignal(SignalBus.SignalName.FireCursorPurchased, new CursorShape3x1());
					SetItemDisabled((int)index, true);
				}
				break;
			case 1:
				// quad 3x3
				if (_onPurchasePressed(5, index))
				{
					SignalBus.Instance.EmitSignal(SignalBus.SignalName.FireCursorPurchased, new CursorShape3x3());
					SetItemDisabled((int)index, true);
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
