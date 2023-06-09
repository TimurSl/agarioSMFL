using agar.io.Engine.Types;
using agar.io.Game.Core.Types;
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
		
		float distanceToFood = nearestFood.Position.Distance(player.PlayerBlob.Position);
		float distanceToPlayer = nearestPlayer.PlayerBlob.Position.Distance(player.PlayerBlob.Position);
		distanceToPlayer -= nearestPlayer.PlayerBlob.Radius;
		distanceToPlayer -= player.PlayerBlob.Radius;
		
		Vector2f position = new Vector2f(0, 0);
		
		if (distanceToPlayer < distanceToFood)
		{
			if (player.CanEat(nearestPlayer))
			{
				position = nearestPlayer.PlayerBlob.Position;
			}
			else if (player.IsEqual(nearestPlayer))
			{
				position = nearestFood.Position;
			}
			else
			{
				position = GetSafePosition(player, nearestPlayer);
			}
		}
		else if (distanceToPlayer < distanceToFood && !player.CanEat(nearestPlayer))
		{
			position = nearestFood.Position;
		}
		else
		{
			position = nearestFood.Position;
		}

		return position;
	}

	private static Player GetNearestPlayer(Player attacker)
	{
		Player nearestPlayer = null;
		float nearestPlayerDistance = float.MaxValue;
		
		foreach (var player in Core.Game.Players)
		{
			if (player != attacker)
			{
				float distance = player.PlayerBlob.Position.Distance(attacker.PlayerBlob.Position);
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
			float distance = food.Position.Distance(attacker.PlayerBlob.Position);
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
		float distance = victim.PlayerBlob.Position.Distance(attacker.PlayerBlob.Position);
		
		if (distance < GameConfiguration.SafeZoneDistance)
		{
			safePosition = victim.PlayerBlob.Position.GetPositionAwayFrom(attacker.PlayerBlob.Position, GameConfiguration.SafeZoneDistance);
			Gizmos.DrawLine(victim.PlayerBlob.Position, attacker.PlayerBlob.Position, Color.Green);
		}
		
		return safePosition;
	}
}