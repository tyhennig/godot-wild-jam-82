using Godot;
using System;

public partial class MainMenu : Control
{
    // Preload the target scenes for fast transitions.
    // private PackedScene gameplayScene;
    // private PackedScene settingsScene;
    // private PackedScene creditsScene;


    public override void _Ready()
    {
        GD.Print("Initializing main menu...");

        GetNode<Button>("MenuLayout/ButtonLayout/StartButton").GrabFocus(); // Set focus on the Start button when the menu is ready

        // Preloading scenes reduces delays during transitions.
        // gameplayScene = (PackedScene)GD.Load("res://Scenes/Gameplay.tscn");
        // settingsScene = (PackedScene)GD.Load("res://Scenes/Settings.tscn");
        // creditsScene = (PackedScene)GD.Load("res://Scenes/Credits.tscn");

        // Set the main menu layout size and position
        // MenuLayout.RectSize = new Vector2(800, 600); // Set the size of the main menu layout
        // MenuLayout.RectPosition = new Vector2((GetViewport().Size.X - menuLayout.RectSize.X) / 2, (GetViewport().Size.Y - menuLayout.RectSize.Y) / 2); // Center the main menu layout
        // GD.Print("MenuLayout size set to: " + menuLayout.RectSize + ", position set to: " + menuLayout.RectPosition);

        // Set the label text for the main menu
        Label titleLabel = GetNode<Label>("MenuLayout/TitleLabel");
        titleLabel.Text = "Welcome to the Game!";
        // titleLabel.AddThemeFontOverride("font_size", 36); // Set the font size for the title label
        // titleLabel.AddThemeFontOverride("font_color", new Color(1, 1, 1)); // Set the font color for the title label
        titleLabel.AddThemeFontOverride("font", (Font)GD.Load("res://fonts/CustomFont.tres")); // Set a custom font for the title label
        GD.Print("Title label set to: " + titleLabel.Text);

        // Set the background color for the main menu
        ColorRect background = GetNode<ColorRect>("MenuLayout/Background");
        background.Color = new Color(0.1f, 0.1f, 0.1f); // Set a dark gray background color
        GD.Print("Background color set to: " + background.Color);

        // Locate Menu Buttons
        Button startButton = GetNode<Button>("MenuLayout/ButtonLayout/StartButton");
        Button settingsButton = GetNode<Button>("MenuLayout/ButtonLayout/SettingsButton");
        Button creditsButton = GetNode<Button>("MenuLayout/ButtonLayout/CreditsButton");
        Button quitButton = GetNode<Button>("MenuLayout/ButtonLayout/QuitButton");

        // Set the button text
        startButton.Text = "Start Game";
        settingsButton.Text = "Settings";
        creditsButton.Text = "Credits";
        quitButton.Text = "Quit";

        // Subscribe each menu button to their respective signal
        startButton.Pressed += OnStartButtonPressed;
        settingsButton.Pressed += OnSettingsButtonPressed;
        creditsButton.Pressed += OnCreditsButtonPressed;
        quitButton.Pressed += OnQuitButtonPressed;

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
        
        // GetTree().ChangeSceneToPackedScene(gameplayScene);
    }

    /// <summary>
    /// This method is called when the settings button is pressed.
    /// 
    /// It opens the settings dialog or transitions to the settings scene.
    /// </summary>
    private void OnSettingsButtonPressed()
    {
        // Load the settings scene when the settings button is pressed

        GD.Print("Opening settings dialog...");



        //      For example, you can show a confirmation dialog for settings changes
        GetNode<ConfirmationDialog>("SettingsDialog").PopupCentered();
        //      For example, you can set the dialog's title and text
        GetNode<ConfirmationDialog>("SettingsDialog").Title = "Settings";

        // Music Settings
        // GetNode<ConfirmationDialog>("SettingsDialog").Text = "Music";
        //     Add a button to turn on the music
        GetNode<ConfirmationDialog>("SettingsDialog").AddButton("ON", true);
        //     Add a button to turn off the music
        GetNode<ConfirmationDialog>("SettingsDialog").AddButton("OFF", false);

        // More Settings
        // ...


        //      Connect the confirmed signal to a method to handle the confirmation
        GetNode<ConfirmationDialog>("SettingsDialog").Confirmed += OnSettingsConfirmationDialogConfirmed;
        // GetNode<ConfirmationDialog>("SettingsDialog").Cancelled += OnSettingsConfirmationDialogCancelled;
    }
    private void OnSettingsConfirmationDialogConfirmed()
    {
        GD.Print("Settings confirmed. Applying changes...");

        // Ask the player if they want to apply the settings changes
        // GetNode<ConfirmationDialog>("SettingsDialog").Text = "Do you want to apply the settings changes?";


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
        //GetTree().ChangeSceneToPackedScene(creditsScene);
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
            GD.Print("Mouse is inside the main menu area.");
        }
        else
        {
            // GD.Print("Mouse is outside the main menu area.");
        }

        // Check if the mouse is hovering over the Start button
        //      Change the button color, text color, and button size when hovered
        Button startButton = GetNode<Button>("StartButton");
        if (startButton.GetRect().HasPoint(mousePosition))
        {
            GD.Print("Mouse is hovering over the Start button.");
            startButton.Modulate = new Color(1, 1, 0); // Change color to yellow when hovered

            // Change button size only when hovered over
            // startButton.RectSize = new Vector2(200, 50); // Example size change
            
            // Change button text color when hovered
            // startButton.AddThemeFontOverride("font_color", new Color(1, 0, 0)); // Change text color to red
        }
        else
        {
            // GD.Print("Mouse is not hovering over the Start button.");
        }

        // Example: Check if the mouse is clicked
        // if (Input.IsMouseButtonPressed(MouseButton.Left))
        // {
        //     GD.Print("Left mouse button is pressed in the main menu.");
        // }
        // Example: Check if the mouse is just clicked
        // if (Input.IsMouseButtonJustPressed(MouseButton.Left))
        // {
        //     GD.Print("Left mouse button just pressed in the main menu.");
        // }


        // Example: Check if the mouse is released
        // if (Input.IsMouseButtonJustReleased(MouseButton.Left))
        // {
        //     GD.Print("Left mouse button just released in the main menu.");
        //     // You can trigger some action here if needed
        // }


        // Example: Check if the mouse wheel is scrolled
        if (Input.IsMouseButtonPressed(MouseButton.WheelUp))
        {
            GD.Print("Mouse wheel scrolled up in the main menu.");
        }

        

        
        // Example: Check if the mouse is hovering over the Quit button
        Control quitButton = GetNode<Control>("QuitButton");
        if (quitButton.GetRect().HasPoint(mousePosition))
        {
            GD.Print("Mouse is hovering over the Quit button.");
        }
        else
        {
            GD.Print("Mouse is not hovering over the Quit button.");
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
