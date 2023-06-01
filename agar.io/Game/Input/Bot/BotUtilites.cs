using agar.io.Engine.Types;
using agar.io.Game.Objects;
using SFML.Graphics;
using Vector2f = SFML.System.Vector2f;

namespace agar.io.Game.Input.Bot;

public class BotUtilites
{
	/// <summary>
	/// Gets the nearest food position to the player, or the nearest player position if the player can eat them.
	/// </summary>
	/// <param name="player">The player to check</param>
	/// <returns>Position of food, or victim, if player can eat them</returns>
	public static Vector2f GetNearestVictim(Player player)
	{
		Food nearestFood = GetNearestFood(player);
		Player nearestPlayer = GetNearestPlayer(player);
		
		float distanceToFood = nearestFood.Position.GetDistance(player.PlayerBlob.Position);
		float distanceToPlayer = nearestPlayer.PlayerBlob.Position.GetDistance(player.PlayerBlob.Position);
		
		if (distanceToPlayer < distanceToFood)
		{
			if (player.CanEat(nearestPlayer))
			{
				return nearestPlayer.PlayerBlob.Position;
			}

			return GetSafePosition(player, nearestPlayer);
		}

		return nearestFood.Position;
	}

	private static Player GetNearestPlayer(Player attacker)
	{
		Player nearestPlayer = null;
		float nearestPlayerDistance = float.MaxValue;
		
		foreach (var player in Core.Game.Players)
		{
			if (player != attacker)
			{
				float distance = player.PlayerBlob.Position.GetDistance(attacker.PlayerBlob.Position);
				if (distance < nearestPlayerDistance)
				{
					nearestPlayerDistance = distance;
					nearestPlayer = player;
				}
			}
		}
		
		return nearestPlayer;
	}

	private static Food GetNearestFood(Player attacker)
	{
		Food nearestFood = null;
		float nearestFoodDistance = float.MaxValue;
		
		foreach (var food in Core.Game.FoodList)
		{
			float distance = food.Position.GetDistance(attacker.PlayerBlob.Position);
			if (distance < nearestFoodDistance)
			{
				nearestFoodDistance = distance;
				nearestFood = food;
			}
		}
		
		return nearestFood;
	}

	private static Vector2f GetSafePosition(Player victim, Player attacker)
	{
		Vector2f safePosition = victim.PlayerBlob.Position;
		float distance = victim.PlayerBlob.Position.GetDistance(attacker.PlayerBlob.Position);
		
		if (distance < 200)
		{
			safePosition = victim.PlayerBlob.Position.GetPositionAwayFrom(attacker.PlayerBlob.Position, 200);
			Gizmos.DrawLine(victim.PlayerBlob.Position, attacker.PlayerBlob.Position, Color.Green);
		}
		
		return safePosition;
	}
}