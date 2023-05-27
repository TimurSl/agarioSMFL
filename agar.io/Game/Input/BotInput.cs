using agar.io.Game.Core.Types;
using agar.io.Game.Input.Interfaces;
using SFML.System;
using SFML.Window;

namespace agar.io.Game.Input;

public class BotInput : IInput
{
	private Clock clock = new Clock();
	private float waitTime = 1;
	private Vector2f targetPosition = new Vector2f(0, 0);
	
	public Vector2f GetDirection(Window window)
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
}