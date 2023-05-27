using SFML.Graphics;

namespace agar.io.Core.Types;

public static class GameConfiguration
{
	public const int DefaultPlayerRadius = 10;
	public static float MaxRadiusUntilZoom = 200f;
	public static float MaxRadiusIncreaseStep = 70f;
	public static float AbsoluteMaxRadius = 1000f;

	public const int MapWidth = 5000;
	public const int MapHeight = 5000;

	public const int MaxBots = 20;
	public const int MaxFood = 800;
	
	public static Font Font = new Font(Path.Combine(Directory.GetCurrentDirectory (), "Fonts", "arial.ttf"));
	
	public static Color darkGreen = new Color(0, 255, 0);
	public static Color darkRed = new Color(255, 0, 0);
}