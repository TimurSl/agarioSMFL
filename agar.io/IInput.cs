using SFML.System;
using SFML.Window;

namespace agar.io;

public interface IInput
{
	public Vector2f GetFinalPosition(Window window);
}