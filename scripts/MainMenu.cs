using Godot;
using System;

public partial class MainMenu : Control
{
    // Define packed scenes to transition to from the main menu
    private PackedScene gameplayScene;
    private PackedScene creditsScene;


    // Define the buttons in the main menu
    private Button startButton;
    private Button settingsButton;
    private Button creditsButton;
    private Button quitButton;


    public override void _Ready()
    {
        GD.Print("Initializing main menu...");

        // Locate Menu Buttons
        startButton = GetNode<Button>("%StartButton");
        settingsButton = GetNode<Button>("%SettingsButton");
        creditsButton = GetNode<Button>("%CreditsButton");
        quitButton = GetNode<Button>("%QuitButton");

        startButton.GrabFocus(); // Set focus on the Start button when the menu is ready

        // Preloading scenes reduces delays during transitions
        gameplayScene = GD.Load<PackedScene>("res://scenes/grid.tscn");
        creditsScene = GD.Load<PackedScene>("res://scenes/credits.tscn");

        GD.Print("Main menu is ready.");
    }


    #region Start Logic
    /// <summary>
    /// This method is called when the start button is pressed.
    /// 
    /// Transitions to the gameplay scene.
    /// </summary>
    private void OnStartButtonPressed()
    {
        GD.Print("Start button pressed. Loading game scene...");
        // GetTree().ChangeSceneToPacked(gameplayScene);
        Hide();
    }
    #endregion Start Logic

    #region Settings Logic
    /// <summary>
    /// This method is called when the settings button is pressed.
    /// 
    /// It opens the settings dialog or transitions to the settings scene.
    /// </summary>
    private void OnSettingsButtonPressed()
    {
        // Load the settings scene when the settings button is pressed
        ConfirmationDialog settingsDialog = GetNode<ConfirmationDialog>("%SettingsDialog");

        GD.Print("Opening settings dialog...");
        settingsDialog.Confirmed += OnSettingsConfirmationDialogConfirmed;
    }
    private void OnSettingsConfirmationDialogConfirmed()
    {
        GD.Print("Settings confirmed. Applying changes...");

        // Apply settings changes here
        // For example, save settings to a file or apply them to the game
    }
    #endregion Settings Logic

    #region Credits Logic
    /// <summary>
    /// This method is called when the credits button is pressed.
    /// 
    /// It transitions to the credits scene.
    /// </summary>
    private void OnCreditsButtonPressed()
    {
        GD.Print("Credits button pressed. Loading credits scene...");
        GetTree().ChangeSceneToPacked(creditsScene);
    }
    #endregion Credits Logic

    #region Quit Logic
    /// <summary>
    /// This method is called when the quit button is pressed.
    /// 
    /// It shows a confirmation dialog before quitting the game.
    /// </summary>
    private void OnQuitButtonPressed()
    {
        GD.Print("Attempting to quit the game...");

        var quitConfirmationDialog = GetNode<ConfirmationDialog>("%QuitConfirmationDialog");
        quitConfirmationDialog.Confirmed += OnQuitConfirmationDialogConfirmed;
    }
    
    /// <summary>
    /// This method is called when the confirmation dialog is confirmed for quitting the game.
    /// 
    /// It performs the necessary cleanup and exits the game.
    /// </summary>
    private void OnQuitConfirmationDialogConfirmed()
    {
        // If you have a game state manager, you might want to save the save game state or release game resources here.
        // Make sure the game can resume correctly next time.
        // ...

        GD.Print("Confirmed exit. Quitting the game...");
        GetTree().Quit(); // This will close the game window and exit the application
    }
    #endregion Quit Logic


    /// <summary>
    /// This method is called every frame to process input events and update the main menu.
    /// 
    /// It checks for mouse position, button hover states, and key presses.
    /// </summary>
    /// <param name="delta"></param>
    public override void _Process(double delta)
    {
        // Example: Check if the mouse is inside the main menu area
        Vector2 mousePosition = GetViewport().GetMousePosition();
        if (GetRect().HasPoint(mousePosition))
        {
            // GD.Print("Mouse is inside the main menu area.");
        }
        else
        {
            // GD.Print("Mouse is outside the main menu area.");
        }

        // Check if the mouse is hovering over the Start button
        //      Change the button color, text color, and button size when hovered
        if (startButton.GetRect().HasPoint(mousePosition))
        {
            GD.Print("Mouse is hovering over the Start button.");
            // startButton.Modulate = new Color(1, 1, 1); // Change color to white for hover effect 

            // // Set background color
            // var styleBox = new StyleBoxFlat
            // {
            //     BgColor = new Color(0.2f, 0.6f, 0.8f) // Example color
            // };
            // startButton.AddThemeStyleboxOverride("normal", styleBox);

            // // Set font color
            // startButton.AddThemeColorOverride("font_color", new Color(1, 1, 1));
        }
        else
        {
            // GD.Print("Mouse is not hovering over the Start button.");
        }


        if (settingsButton.GetRect().HasPoint(mousePosition))
        {
            GD.Print("Mouse is hovering over the Settings button.");
            // settingsButton.Modulate = new Color(1, 1, 1); // Change color to white for hover effect 

            // Change button size only when hovered over
            // settingsButton.RectSize = new Vector2(200, 50); // Example size change

            // Change button text color when hovered
            // settingsButton.AddThemeFontOverride("font_color", new Color(1, 0, 0)); // Change text color to red
        }
        else
        {
            // GD.Print("Mouse is not hovering over the Settings button.");
        }

        if (creditsButton.GetRect().HasPoint(mousePosition))
        {
            GD.Print("Mouse is hovering over the Credits button.");
            // creditsButton.Modulate = new Color(1, 1, 1); // Change color to white for hover effect 

            // Change button size only when hovered over
            // creditsButton.RectSize = new Vector2(200, 50); // Example size change

            // Change button text color when hovered
            // creditsButton.AddThemeFontOverride("font_color", new Color(1, 0, 0)); // Change text color to red
        }
        else
        {
            // GD.Print("Mouse is not hovering over the Credits button.");
        }




        // Example: Check if the mouse is hovering over the Quit button
        if (quitButton.GetRect().HasPoint(mousePosition))
        {
            GD.Print("Mouse is hovering over the Quit button.");
            // quitButton.Modulate = new Color(1, 1, 1); // Change color to white for hover effect 
        }
        else
        {
            // GD.Print("Mouse is not hovering over the Quit button.");
        }


        // Example: Check if a specific key is pressed
        if (Input.IsKeyPressed(Key.Escape))
        {
            GD.Print("Escape key pressed in the main menu. Exiting game.");

            // Confirm the exit action
            // GetNode<ConfirmationDialog>("ConfirmationDialog").PopupCentered();
            // GetNode<ConfirmationDialog>("ConfirmationDialog").Text = "Are you sure you want to quit the game?";
            // GetNode<ConfirmationDialog>("ConfirmationDialog").Confirmed += OnQuitConfirmationDialogConfirmed;
            // GetNode<ConfirmationDialog>("ConfirmationDialog").Cancelled += OnQuitConfirmationDialogCancelled;

            GetTree().Quit(); // Exit the game when the Escape key is pressed
        }
    }
}
