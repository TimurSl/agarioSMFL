using agar.io.Game.Objects;
using SFML.System;
using SFML.Window;

namespace agar.io.Game.Input.Interfaces;

public interface IInput
{
	public Player ControllerPlayer { get; set; }
	public Vector2f GetTargetPosition(Window window);
	public void HandleInput(Window window);
}