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

public static class WindowExtensions
{
	public static Vector2f MapPixelToCoords(this View view, Vector2i pixelPosition, RenderWindow window)
	{
		Vector2f viewCenter = view.Center;
		Vector2f viewSize = view.Size;
		Vector2u windowSize = window.Size;

		float viewLeft = viewCenter.X - (viewSize.X / 2f);
		float viewTop = viewCenter.Y - (viewSize.Y / 2f);

		float scaleX = viewSize.X / windowSize.X;
		float scaleY = viewSize.Y / windowSize.Y;

		float worldX = viewLeft + (pixelPosition.X * scaleX);
		float worldY = viewTop + (pixelPosition.Y * scaleY);

		return new Vector2f(worldX, worldY);
	}
	
	public static Vector2f MapPixelToCoords(this Window window, Vector2i pixelPosition, View view)
	{
		Vector2f viewCenter = view.Center;
		Vector2f viewSize = view.Size;
		Vector2u windowSize = window.Size;

		float viewLeft = viewCenter.X - (viewSize.X / 2f);
		float viewTop = viewCenter.Y - (viewSize.Y / 2f);

		float scaleX = viewSize.X / windowSize.X;
		float scaleY = viewSize.Y / windowSize.Y;

		float worldX = viewLeft + (pixelPosition.X * scaleX);
		float worldY = viewTop + (pixelPosition.Y * scaleY);

		return new Vector2f(worldX, worldY);
	}
}