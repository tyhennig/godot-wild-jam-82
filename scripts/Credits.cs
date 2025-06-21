using Godot;
using System;

public partial class Credits : PanelContainer
{
    private Button startButton;

    public override void _Ready()
    {
        GD.Print("Initializing credits...");

        startButton = GetNode<Button>("/root/Main/UI/MainMenu/PanelContainer/ButtonLayout/StartButton");

        GD.Print("Credits are ready.");
    }

    private void OnBackButtonPressed()
    {
        GD.Print("Back button pressed in credits. Returning to main menu...");

        this.Visible = false; // Hide the credits panel

        startButton.GrabFocus();
    }
}