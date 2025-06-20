using Godot;
using System;

public partial class RoundLabel : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameManager.Instance.NewRound += UpdateRoundNumber;
	}

	private void UpdateRoundNumber(int roundNumber)
	{
		GD.Print("Updating Round Label to ", roundNumber);
		GetNode<Label>("RoundLabel").Text = roundNumber.ToString();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

	
}
