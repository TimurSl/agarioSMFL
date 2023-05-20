using SFML.Graphics;

namespace agar.io.Core.Types;

public static class GameConfiguration
{
	public const int DefaultPlayerRadius = 10;
	public static float MaxRadius = 200f;
	public static float MaxRadiusIncreaseStep = 50f;
	public static float AbsoluteMaxRadius = 1000f;

	public const int WindowWidth = 1280;
	public const int WindowHeight = 720;

	public const float MapWidth = 5000f;
	public const float MapHeight = 5000f;

	public const int MaxBots = 20;
	public const int MaxFood = 2000;
	
	public static Font Font = new Font(Path.Combine(Directory.GetCurrentDirectory (), "Fonts", "arial.ttf"));
}