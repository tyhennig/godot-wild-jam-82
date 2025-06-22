using Godot;
using System;

public partial class MainMenu : Control
{

    [Signal]
    public delegate void StartGameEventHandler();

    // Scenes, but as nodes
    private PanelContainer credits;


    // Define the buttons in the main menu
    public Button startButton { get; private set; }
    private bool isGameStarted = false; // Track if the start button has been pressed
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

        // Register to game over signal
        GameManager.Instance.GameOver += OnGameOver;

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

        // Logic for changing start-game to continue
        if (isGameStarted)
        {
            GD.Print("Start button was already pressed. Continuing the game...");
        }
        else
        {
            EmitSignal(SignalName.StartGame);
            isGameStarted = true; // Set the flag to indicate the start button has been pressed
            startButton.Text = "Continue"; // Change the button text to "Continue"
        }

        Hide();
    }

    public void OnGameOver()
    {
        startButton.Text = "Start Game";
        isGameStarted = false;
        Show();
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
        GD.Print("Opening settings dialog...");
        GetNode<ConfirmationDialog>("%SettingsDialog").PopupCentered();
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

        credits = GetNode<PanelContainer>("%Credits");
        credits.Visible = true; // Show the credits panel
        
        Button creditsBackButton = credits.GetNode<Button>("%CreditsBackButton");
        creditsBackButton.GrabFocus(); // Set focus on the Back button in credits
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

        GetNode<ConfirmationDialog>("%QuitConfirmationDialog").PopupCentered();
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


        /*
        * Check if the Escape key is pressed while the main menu is visible - Will assume quitting the game
        *   - This is useful for allowing players to exit the game quickly from the main menu
        */
        if (Input.IsKeyPressed(Key.Escape))
        {
            if (!this.Visible)
            {
                // Bring up the menu if trying to escape while the main menu is not visible
                GD.Print("Escape key pressed while in game. Popping-up the main menu.");
                this.Show();
                startButton.GrabFocus(); // Set focus on the Start button when the menu is ready   
            }
        }
    }
}
