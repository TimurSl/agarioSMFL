using System.Reflection;
using System.Text;
using SFML.Graphics;
using ZenisoftGameEngine.Extensions;

namespace ZenisoftGameEngine.Config;

public class EngineConfiguration
{
	/// <summary>
	/// The width of the window
	/// </summary>
	public static uint WindowWidth = 1280;
	
	/// <summary>
	/// The height of the window
	/// </summary>
	public static uint WindowHeight = 720;
	
	/// <summary>
	/// The maximum framerate of the game
	/// </summary>
	public static uint FrameRateLimit = 120;
	
	/// <summary>
	/// The title of the window
	/// </summary>
	public const string WindowTitle = "Agar.io Clone";
	
	/// <summary>
	/// The background color of the window
	/// </summary>
	public static Color BackgroundColor = new Color(255, 255, 255);
	
	public static bool DebugMode = true;
	public static bool Fullscreen = false;

	static EngineConfiguration()
	{
		Load ();
	}
	
	public static void Save()
	{
		StringBuilder cfg = new StringBuilder ();
		var fields = typeof(EngineConfiguration).GetFields ();

		foreach(FieldInfo fieldInfo in fields)
		{
			if (fieldInfo.IsSaveable ())
				cfg.AppendLine($"{fieldInfo.Name}:{fieldInfo.GetValue(null)}");
		}

		File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory (), "engine.cfg"), cfg.ToString ());
	}

	public static void Load()
	{
		if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory (), "engine.cfg")))
		{
			Save ();
			return;
		}
		
		var lines = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory (), "engine.cfg"));

		foreach (string line in lines)
		{
			var split = line.Split (":");
			if (split.Length != 2)
			{
				Console.WriteLine($"Invalid config line: {line}");
				continue;
			}
			
			var field = typeof(EngineConfiguration).GetField (split[0]);
			if (field == null)
			{
				Console.WriteLine($"Invalid config line: {line}");
				continue;
			}
			
			var value = Convert.ChangeType (split[1], field.FieldType);
			
			field.SetValue (null, value);
		}
	}
}