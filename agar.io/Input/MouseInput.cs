using agar.io.Core.Types;
using agar.io.Input.Interfaces;
using agar.io.Objects;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace agar.io.Input;

public class MouseInput : IInput
{
	private View camera;

	public MouseInput(View camera)
	{
		this.camera = camera;
	}
	
	public Vector2f GetDirection(Window window)
	{
		Vector2i mousePosition = Mouse.GetPosition(window);
		Vector2f center = new Vector2f(GameConfiguration.WindowWidth / 2f, GameConfiguration.WindowHeight / 2f);
		Vector2f mouseOffset = new Vector2f(mousePosition.X - center.X, mousePosition.Y - center.Y);
		
		return mouseOffset + camera.Center;
	}
}