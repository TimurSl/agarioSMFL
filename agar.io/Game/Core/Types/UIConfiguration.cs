using SFML.Graphics;

namespace agar.io.Game.Core.Types;

public static class UIConfiguration
{
	public static Font Font = new Font(Path.Combine(Directory.GetCurrentDirectory (), "Fonts", "Pusia.otf"));
	
	public static Color darkGreen = new Color(0, 255, 0);
	public static Color darkRed = new Color(255, 0, 0);
}