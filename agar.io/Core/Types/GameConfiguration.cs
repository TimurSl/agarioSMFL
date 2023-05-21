using SFML.Graphics;

namespace agar.io.Core.Types;

public static class GameConfiguration
{
	public const int DefaultPlayerRadius = 10;
	public static float MaxRadius = 200f;
	public static float MaxRadiusIncreaseStep = 50f;
	public static float AbsoluteMaxRadius = 2000f;

	public const int WindowWidth = 1280;
	public const int WindowHeight = 720;

	public const float MapWidth = 5000f;
	public const float MapHeight = 5000f;

	public const int MaxBots = 20;
	public const int MaxFood = 800;
	
	public static Font Font = new Font(Path.Combine(Directory.GetCurrentDirectory (), "Fonts", "arial.ttf"));
	
	public static Color darkGreen = new Color(0, 255, 0);
	public static Color darkRed = new Color(255, 0, 0);
}