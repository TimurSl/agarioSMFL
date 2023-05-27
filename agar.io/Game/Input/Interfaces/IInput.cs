using SFML.System;
using SFML.Window;

namespace agar.io.Game.Input.Interfaces;

public interface IInput
{
	public Vector2f GetDirection(Window window);
}