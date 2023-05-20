using System.Diagnostics;

namespace agar.io.Core.Types;

public static class Time
{
	private static Stopwatch stopwatch;

	public static float DeltaTime { get; private set; }

	static Time()
	{
		stopwatch = new Stopwatch();
		stopwatch.Start();

	}

	public static void Update()
	{
		DeltaTime = (float)stopwatch.Elapsed.TotalSeconds;
		stopwatch.Restart();
	}
}