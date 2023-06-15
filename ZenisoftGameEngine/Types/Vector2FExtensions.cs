using SFML.System;

namespace ZenisoftGameEngine.Types;

public static class Vector2FExtensions
{
	public static float Distance(this Vector2f vector, Vector2f other)
	{
		float vectorX = vector.X - other.X;
		float vectorY = vector.Y - other.Y;
		return MathF.Sqrt((vectorX * vectorX) + (vectorY * vectorY));
	}
	
	public static float DistanceSquared(this Vector2f vector, Vector2f other)
	{
		float vectorX = vector.X - other.X;
		float vectorY = vector.Y - other.Y;
		return (vectorX * vectorX) + (vectorY * vectorY);
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