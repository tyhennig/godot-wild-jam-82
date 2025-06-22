using Godot;
using System;

public partial class Shop : CanvasLayer
{
	public String test { get; set; }
	public String currency { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var button = GetNode<Button>("NextRoundButton");
		button.Pressed += ExitShop;

		var shopLabel = GetNode<Label>("ShopLabel");
		shopLabel.Text = test != null ? test : "It failed";
		
		var currencyLabel = GetNode<Label>("CurrencyLabel");
		currencyLabel.Text = test != null ? currency : "It failed";
	}

    private void ExitShop()
    {
		this.QueueFree();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
