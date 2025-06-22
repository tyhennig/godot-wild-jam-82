using Godot;
using System;

public partial class Resources : Control
{
	[Export]
	private Label ammoLabel;

	[Export]
	private Label scanLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		UpdateAmmo(GameManager.Instance.CurrentBullets);
		UpdateScans(GameManager.Instance.CurrentScans);
	}

	public void UpdateAmmo(int ammo)
	{
		ammoLabel.Text = "x " + ammo;
	}

	public void UpdateScans(int scans)
	{
		scanLabel.Text = "x " + scans;
	}
}
