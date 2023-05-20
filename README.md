# Agar.io C#

This repository contains the source code and development progress for the Agar.io game. The game is being developed using the SFML library and follows an object-oriented design approach.

## Contents

The repository includes the following files:

- `Program.cs`: The entry point of the application.
- `Game.cs`: The main game class responsible for managing the game loop, rendering objects, and handling player input.
- `Player.cs`: The class representing the player object.
- `Food.cs`: The class representing the food object.
- `GameConfiguration.cs`: Configuration settings for the game.
- `IDrawable.cs`: An interface for objects that can be drawn on the screen.
- `IInput.cs`: An interface for handling player input.
- `IUpdatable.cs`: An interface for objects that can be updated.
- `MouseInput.cs`: A class implementing the player input interface for mouse control.
- `BotInput.cs`: A class implementing the player input interface for AI-controlled bots.

## Development Progress

During the development, the following tasks were completed:

1. Initialization and setup of the game window, including window size and title.
2. Creation of the player object and its input handling.
3. Creation of AI-Controlled bots with random movement and waiting behavior.
4. Handling collision detection between players and food objects.
5. Implementation of camera movement and player following.
6. Setting up a larger game map and locking player movement within the map boundaries.

## Instructions

To run the Agar.io game:

1. Make sure you have the necessary dependencies installed, including the SFML library.
2. Clone this repository to your local machine.
3. Build the project using your preferred development environment.
4. Run the compiled executable to start the game.

## Future Enhancements

The following features and improvements can be considered for future development:

- Implementing a scoring system and leaderboard.
- Adding different game modes and levels.
- Enhancing the game visuals and adding animations.
- Optimizing performance and memory usage.
- Fixing bug with mouse input.

Feel free to contribute to the project by submitting pull requests or opening issues for any bugs or suggestions.

## Credits

This project was developed by Zenisoft
