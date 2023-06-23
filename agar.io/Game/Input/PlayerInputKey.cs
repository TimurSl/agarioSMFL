using SFML.Window;

namespace agar.io.Game.Input;

public class PlayerInputKey
{
	public Keyboard.Key Key { get; init; }
	
	private bool wasPressed = false;
	
	public PlayerInputKey(Keyboard.Key key)
	{
		Key = key;
	}
	
	public bool GetKey()
	{
		return Keyboard.IsKeyPressed(Key);
	}
	
	public bool GetKeyDown()
	{
		bool isPressed = Keyboard.IsKeyPressed(Key);
		bool isDown = isPressed && !wasPressed;
		
		wasPressed = isPressed;
		
		return isDown;
	}
}