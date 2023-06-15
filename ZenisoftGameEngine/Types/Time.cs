using System.Diagnostics;

namespace ZenisoftGameEngine.Types;

public static class Time
{
	private static Stopwatch stopwatch;

	/// <summary>
	/// The time between the last frame and the current frame
	/// </summary>
	public static float DeltaTime { get; private set; }

	static Time()
	{
		stopwatch = new Stopwatch();
		stopwatch.Start();
	}
	
	/// <summary>
	/// Must be called every frame, updates the DeltaTime
	/// </summary>
	public static void Update()
	{
		DeltaTime = (float)stopwatch.Elapsed.TotalSeconds;
		stopwatch.Restart();
	}
}