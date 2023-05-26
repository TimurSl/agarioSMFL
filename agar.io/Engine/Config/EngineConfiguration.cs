using SFML.Graphics;

namespace agar.io.Engine.Config;

public class EngineConfiguration
{
	/// <summary>
	/// The width of the window
	/// </summary>
	public const int WindowWidth = 1280;
	
	/// <summary>
	/// The height of the window
	/// </summary>
	public const int WindowHeight = 720;
	
	/// <summary>
	/// The maximum framerate of the game
	/// </summary>
	public const int FrameRateLimit = 120;
	
	/// <summary>
	/// The title of the window
	/// </summary>
	public const string WindowTitle = "Agar.io Clone";
	
	/// <summary>
	/// The background color of the window
	/// </summary>
	public static Color BackgroundColor = new Color(255, 255, 255);
	
	/// <summary>
	/// The font of the game
	/// </summary>
	public static Font Font = new Font(Path.Combine(Directory.GetCurrentDirectory (), "Fonts", "arial.ttf"));
}