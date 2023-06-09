using System.Reflection;
using System.Text;
using agar.io.Engine.Extensions;
using SFML.Graphics;

namespace agar.io.Game.Core.Types;

public static class GameConfiguration
{
	public static int DefaultPlayerRadius = 10;
	public static float MaxRadiusUntilZoom = 200f;
	public static float MaxRadiusIncreaseStep = 70f;
	public static float AbsoluteMaxRadius = 1000f;

	public static int MapWidth = 10000;
	public static int MapHeight = 10000;

	public static int MaxBots = 20;
	public static int MaxFood = 800;

	public static float MovementSpeed = 200f;
	
	public static float SafeZoneDistance = 400f;
	public static bool EnableCheats = false;

	static GameConfiguration()
	{
		Load ();
	}

	public static void Save()
	{
		StringBuilder cfg = new StringBuilder ();
		var fields = typeof(GameConfiguration).GetFields ();

		foreach(FieldInfo fieldInfo in fields)
		{
			if (fieldInfo.IsSaveable ())
				cfg.AppendLine($"{fieldInfo.Name}:{fieldInfo.GetValue(null)}");
		}
		
		File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory (), "game.cfg"), cfg.ToString ());
	}

	public static void Load()
	{
		if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory (), "game.cfg")))
		{
			Save ();
			Console.WriteLine("Config file not found, creating new one.");
			return;
		}
		
		var lines = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory (), "game.cfg"));

		foreach (string line in lines)
		{
			var split = line.Split (":");
			var field = typeof(GameConfiguration).GetField (split[0]);
			
			var value = Convert.ChangeType (split[1], field.FieldType);
			
			field.SetValue (null, value);
			
			// write all current values of variables
			Console.WriteLine($"{field.Name} = {field.GetValue(null)}");
		}
	}
}