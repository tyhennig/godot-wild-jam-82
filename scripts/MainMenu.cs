using Godot;
using System;

public partial class MainMenu : Control
{
    // Preload the target scenes for fast transitions.
    private PackedScene gameplayScene;
    // private PackedScene settingsScene;
    // private PackedScene creditsScene;

    private Button startButton;
    private Button settingsButton;
    private Button creditsButton;
    private Button quitButton;


    public override void _Ready()
    {
        GD.Print("Initializing main menu...");

        // Locate Menu Buttons
        startButton = GetNode<Button>("MarginContainer/ButtonLayout/StartButton");
        settingsButton = GetNode<Button>("MarginContainer/ButtonLayout/SettingsButton");
        creditsButton = GetNode<Button>("MarginContainer/ButtonLayout/CreditsButton");
        quitButton = GetNode<Button>("MarginContainer/ButtonLayout/QuitButton");

        startButton.GrabFocus(); // Set focus on the Start button when the menu is ready

        // Preloading scenes reduces delays during transitions.
        gameplayScene = GD.Load<PackedScene>("res://scenes/grid.tscn");
        // settingsScene = (PackedScene)GD.Load("res://Scenes/Settings.tscn");
        // creditsScene = (PackedScene)GD.Load("res://Scenes/Credits.tscn");


        // Set the button text
        startButton.Text = "Start Game";
        settingsButton.Text = "Settings";
        creditsButton.Text = "Credits";
        quitButton.Text = "Quit";

        GD.Print("Main menu is ready.");
    }

    /// <summary>
    /// This method is called when the start button is pressed.
    /// 
    /// Transitions to the gameplay scene.
    /// </summary>
    private void OnStartButtonPressed()
    {
        GD.Print("Start button pressed. Loading game scene...");
        
        GetTree().ChangeSceneToPacked(gameplayScene);
    }

    /// <summary>
    /// This method is called when the settings button is pressed.
    /// 
    /// It opens the settings dialog or transitions to the settings scene.
    /// </summary>
    private void OnSettingsButtonPressed()
    {
        // Load the settings scene when the settings button is pressed
        ConfirmationDialog settingsConfirmationDialog = GetNode<ConfirmationDialog>("MarginContainer/ButtonLayout/SettingsButton/SettingsDialog");

        GD.Print("Opening settings dialog...");


        //      For example, you can show a confirmation dialog for settings changes
         settingsConfirmationDialog.PopupCentered();
        //      For example, you can set the dialog's title and text
         settingsConfirmationDialog.Title = "Settings";

        // Music Settings
         settingsConfirmationDialog.DialogText = "Music";
        //     Add a button to turn on the music
         settingsConfirmationDialog.AddButton("ON", true);
        //     Add a button to turn off the music
         settingsConfirmationDialog.AddButton("OFF", false);

        // More Settings
        // ...


        //      Connect the confirmed signal to a method to handle the confirmation
         settingsConfirmationDialog.Confirmed += OnSettingsConfirmationDialogConfirmed;
        //  settingsConfirmationDialog.Cancelled += OnSettingsConfirmationDialogCancelled;
    }
    private void OnSettingsConfirmationDialogConfirmed()
    {
        GD.Print("Settings confirmed. Applying changes...");

        // Ask the player if they want to apply the settings changes
        //  settingsConfirmationDialog.Text = "Do you want to apply the settings changes?";


        // Apply settings changes here
        // For example, save settings to a file or apply them to the game
    }
    private void OnSettingsConfirmationDialogCancelled()
    {
        GD.Print("Settings changes cancelled. Returning to main menu.");
        // Handle cancellation of settings changes here
        // For example, revert any temporary changes made in the settings dialog
    }

    /// <summary>
    /// This method is called when the credits button is pressed.
    /// 
    /// It transitions to the credits scene.
    /// </summary>
    private void OnCreditsButtonPressed()
    {
        // GetTree().ChangeSceneToPacked(creditsScene);
    }

    /// <summary>
    /// This method is called when the quit button is pressed.
    /// 
    /// It shows a confirmation dialog before quitting the game.
    /// </summary>
    private void OnQuitButtonPressed()
    {
        GD.Print("Attempting to quit the game...");
        GetNode<ConfirmationDialog>("ConfirmationDialog").PopupCentered();
        // GetNode<ConfirmationDialog>("ConfirmationDialog").Text = "Are you sure you want to quit the game?";
        GetNode<ConfirmationDialog>("ConfirmationDialog").Confirmed += OnQuitConfirmationDialogConfirmed;
        // GetNode<ConfirmationDialog>("ConfirmationDialog").Cancelled += OnQuitConfirmationDialogCancelled;
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
    /// <summary>
    /// This method is called when the confirmation dialog is cancelled for quitting the game.
    /// </summary>
    private void OnQuitConfirmationDialogCancelled()
    {
        GD.Print("Quit confirmation dialog cancelled. Returning to main menu.");
    }

    /// <summary>
    /// This method is called to handle input events in the main menu.
    /// It can be used to handle keyboard input, mouse clicks, or other input events.
    /// </summary>
    /// <param name="event"></param>
    public override void _Input(InputEvent @event)
    {
        // Handle input events if needed
        // if (@event is InputEventKey eventKey && eventKey.Pressed)
        // {
        //     if (eventKey.KeyCode == Key.Escape)
        //     {
        //         // Exit the game when the Escape key is pressed
        //         GetTree().Quit();
        //     }
        // }
    }

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
            // GD.Print("Mouse is hovering over the Start button.");
            startButton.Modulate = new Color(1, 1, 1); // Change color to white for hover effect 

            // Change button size only when hovered over
            // startButton.RectSize = new Vector2(200, 50); // Example size change
            
            // Change button text color when hovered
            // startButton.AddThemeFontOverride("font_color", new Color(1, 0, 0)); // Change text color to red
        }
        else
        {
            // GD.Print("Mouse is not hovering over the Start button.");
        }

        if (settingsButton.GetRect().HasPoint(mousePosition))
        {
            // GD.Print("Mouse is hovering over the Settings button.");
            settingsButton.Modulate = new Color(1, 1, 1); // Change color to white for hover effect 

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
            // GD.Print("Mouse is hovering over the Credits button.");
            creditsButton.Modulate = new Color(1, 1, 1); // Change color to white for hover effect 

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
            // GD.Print("Mouse is hovering over the Quit button.");
            quitButton.Modulate = new Color(1, 1, 1); // Change color to white for hover effect 
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
            GetNode<ConfirmationDialog>("ConfirmationDialog").PopupCentered();
            // GetNode<ConfirmationDialog>("ConfirmationDialog").Text = "Are you sure you want to quit the game?";
            GetNode<ConfirmationDialog>("ConfirmationDialog").Confirmed += OnQuitConfirmationDialogConfirmed;
            // GetNode<ConfirmationDialog>("ConfirmationDialog").Cancelled += OnQuitConfirmationDialogCancelled;

            GetTree().Quit(); // Exit the game when the Escape key is pressed
        }
    }
}
