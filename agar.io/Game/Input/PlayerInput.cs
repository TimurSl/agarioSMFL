using agar.io.Game.Input.Interfaces;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using ZenisoftGameEngine.Config;

namespace agar.io.Game.Input;

public class PlayerInput : IInput
{
	private View camera;
	private Dictionary<PlayerInputKey, Action> keyActions = new Dictionary<PlayerInputKey, Action> ();
	public Objects.Player ControllerPlayer { get; set; } = Objects.Player.LocalPlayer;

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
		foreach (KeyValuePair<PlayerInputKey, Action> keyAction in keyActions)
		{
			if (keyAction.Key.GetKeyDown ())
			{
				keyAction.Value.Invoke();
			}
		}
	}

	public void BindKey(Keyboard.Key key, Action action)
	{
		PlayerInputKey playerInputKey = new PlayerInputKey(key);
		keyActions.TryAdd(playerInputKey, action);
	}
}