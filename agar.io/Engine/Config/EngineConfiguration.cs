using SFML.Graphics;

namespace agar.io.Engine.Config;

public class EngineConfiguration
{
	public const int WindowWidth = 1280;
	public const int WindowHeight = 720;
	public const int FrameRateLimit = 120;
	
	public static Color BackgroundColor = new Color(255, 255, 255);
	
	public static Font Font = new Font(Path.Combine(Directory.GetCurrentDirectory (), "Fonts", "arial.ttf"));
}