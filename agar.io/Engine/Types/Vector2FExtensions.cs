using SFML.System;

namespace agar.io.Engine.Types;

public static class Vector2FExtensions
{
	public static float GetDistance(this SFML.System.Vector2f vector, SFML.System.Vector2f other)
	{
		return MathF.Sqrt(MathF.Pow(vector.X - other.X, 2) + MathF.Pow(vector.Y - other.Y, 2));
	}

	public static Vector2f GetPositionAwayFrom(this Vector2f vector, Vector2f other, float distance)
	{
		Vector2f direction = vector - other;
		direction.Normalize();
		return vector + direction * distance;
	}
	
	public static void Normalize(this ref Vector2f vector)
	{
		float length = MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
		if (length != 0)
		{
			vector.X /= length;
			vector.Y /= length;
		}
	}
}