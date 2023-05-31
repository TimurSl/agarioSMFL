using SFML.Graphics;

namespace agar.io.Game.Core.Types;

public static class GameConfiguration
{
	public const int DefaultPlayerRadius = 10;
	public static float MaxRadiusUntilZoom = 200f;
	public static float MaxRadiusIncreaseStep = 70f;
	public static float AbsoluteMaxRadius = 1000f;

	public const int MapWidth = 10000;
	public const int MapHeight = 10000;

	public const int MaxBots = 20;
	public const int MaxFood = 800;
	
	public static Font Font = new Font(Path.Combine(Directory.GetCurrentDirectory (), "Fonts", "Pusia.otf"));
	
	public static Color darkGreen = new Color(0, 255, 0);
	public static Color darkRed = new Color(255, 0, 0);
	
	public const float MovementSpeed = 200f;
}