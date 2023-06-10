using agar.io.Game.Core.Types;
using agar.io.Game.Input.Interfaces;
using agar.io.Game.Objects;
using SFML.System;
using SFML.Window;


namespace agar.io.Game.Input.Bot;

public class SimpleBotInput : IInput
{
	private Clock clock = new Clock();
	private float waitTime = 1;
	private Vector2f targetPosition = new Vector2f(0, 0);
	public Objects.Player ControllerPlayer { get; set; }

	public Vector2f GetTargetPosition(Window window)
	{
		if (clock.ElapsedTime.AsSeconds() >= waitTime)
		{
			int randomX = Core.Game.Random.Next(0, (int) GameConfiguration.MapWidth);
			int randomY = Core.Game.Random.Next(0, (int) GameConfiguration.MapHeight);
		
			targetPosition = new Vector2f(randomX, randomY);
			waitTime = Core.Game.Random.Next(1, 10);
			clock.Restart();
		}

		return targetPosition;
	}

	public void HandleInput(Window window)
	{
		// Do nothing
	}
}