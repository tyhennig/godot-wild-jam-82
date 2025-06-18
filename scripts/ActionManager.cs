using Godot;
using System;

public partial class ActionManager : Node
{
	private int ammoCount;
	private int scanCount;
	private States currentState;
	public enum States
	{
		Firing,
		Scanning,
		Nothing
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var actionNode = GetNode<Actions>("../Actions");
		actionNode.FirePressed += OnFirePressed;
		actionNode.ScanPressed += OnScanPressed;

		var gridNode = GetNode<Grid>("../Grid");
		gridNode.GridClicked += OnGridClicked;

		currentState = States.Nothing;

		ammoCount = 99;
		scanCount = 10;
	}

    private void OnGridClicked(Vector2I cell)
    {
        switch (currentState)
		{
			case States.Firing:
				GD.Print("Firing on ", cell);
				if (ammoCount >= 1)
				{
					GetNode<Resources>("../Resources").UpdateAmmo(--ammoCount);
				}
				break;
			case States.Scanning:
				GD.Print("Scanning on ", cell);
				if (scanCount >= 1)
				{
					GetNode<Resources>("../Resources").UpdateScans(--scanCount);
				}				break;
			case States.Nothing:
				break;
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

	private void OnFirePressed()
	{
		currentState = States.Firing;
	}

	private void OnScanPressed()
	{
		currentState = States.Scanning;
	}
}
