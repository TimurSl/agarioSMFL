using agar.io.Game.Input.Interfaces;
using agar.io.Game.Objects;
using SFML.System;
using SFML.Window;

namespace agar.io.Game.Input.Bot;

public class AdvancedBotInput : IInput
{
	public Objects.Player ControllerPlayer { get; set; }
	public Vector2f GetTargetPosition(Window window)
	{
		Vector2f direction = BotUtilites.GetNearestVictim(ControllerPlayer);
		return direction;
	}

	public void HandleInput(Window window)
	{
		
	}
}