using Godot;
using System;

public partial class Credits : PanelContainer
{
    private Button backButton;

    private PackedScene mainMenuScene;

    public override void _Ready()
    {
        GD.Print("Initializing credits...");

        // Locate the Back button
        backButton = GetNode<Button>("CreditsLayout/BackButton");
        backButton.GrabFocus(); // Set focus on the Back button when the credits are ready

        // Save reference to the Main Menu scene
        mainMenuScene = GD.Load<PackedScene>("res://scenes/main_menu.tscn");

        GD.Print("Credits are ready.");
    }

    private void OnBackButtonPressed()
    {
        GD.Print("Back button pressed. Returning to main menu...");

        // Return to the main menu scene
        GetTree().ChangeSceneToPacked(mainMenuScene);
    }
}