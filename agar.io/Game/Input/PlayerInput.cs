using agar.io.Engine.Config;
using agar.io.Game.Input.Interfaces;
using agar.io.Game.Objects;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace agar.io.Game.Input;

public class PlayerInput : IInput
{
	private View camera;
	private Dictionary<Keyboard.Key, Action> keyActions = new Dictionary<Keyboard.Key, Action> ();
	public Player ControllerPlayer { get; set; } = Player.LocalPlayer;

	public PlayerInput(View camera)
	{
		this.camera = camera;
	}

	public Vector2f GetTargetPosition(Window window)
	{
		Vector2i mousePosition = Mouse.GetPosition(window);
		Vector2f center = new Vector2f(EngineConfiguration.WindowWidth / 2f, EngineConfiguration.WindowHeight / 2f);
		Vector2f mouseOffset = new Vector2f(mousePosition.X - center.X, mousePosition.Y - center.Y);

		return mouseOffset + camera.Center;
	}

	public void HandleInput(Window window)
	{
		foreach (KeyValuePair<Keyboard.Key, Action> keyAction in keyActions)
		{
			if (Keyboard.IsKeyPressed(keyAction.Key))
			{
				keyAction.Value.Invoke();
			}
		}
	}

	public void BindKey(Keyboard.Key key, Action action)
	{
		keyActions.TryAdd(key, action);
	}
}