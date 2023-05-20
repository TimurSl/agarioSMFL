using SFML.Graphics;

namespace agar.io.Core.Types;

public class GameConfiguration
{
	public const int DefaultPlayerRadius = 10;
	public const float MaxRadius = 200f;

	public const int WindowWidth = 1280;
	public const int WindowHeight = 720;

	public const float MapWidth = 5000f;
	public const float MapHeight = 5000f;

	public const int MaxPlayers = 20;
	public const int MaxFood = 2000;
	
	public static Font Font = new Font(Path.Combine(Directory.GetCurrentDirectory (), "Fonts", "arial.ttf"));
}