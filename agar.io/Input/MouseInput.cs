using SFML.System;
using SFML.Window;

namespace agar.io;

public class MouseInput : IInput
{
	public Vector2f GetFinalPosition(Window window)
	{
		Vector2f mousePos = new Vector2f(Mouse.GetPosition().X, Mouse.GetPosition().Y);
		return mousePos;
	}
}