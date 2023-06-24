using System.Diagnostics;
using SFML.System;

namespace ZenisoftGameEngine.Types;

public static class Time
{
	private static Stopwatch stopwatch;
	private static Clock clock = new Clock();

	/// <summary>
	/// The time between the last frame and the current frame
	/// </summary>
	public static float DeltaTime { get; private set; }
	
	public static float TotalTime => (float)clock.ElapsedTime.AsSeconds();

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