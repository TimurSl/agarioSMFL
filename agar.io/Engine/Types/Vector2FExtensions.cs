using SFML.System;

namespace agar.io.Engine.Types;

public static class Vector2FExtensions
{
	public static float GetDistance(this SFML.System.Vector2f vector, SFML.System.Vector2f other)
	{
		return MathF.Sqrt(MathF.Pow(vector.X - other.X, 2) + MathF.Pow(vector.Y - other.Y, 2));
	}
}