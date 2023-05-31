﻿using agar.io.Engine.Config;
using agar.io.Engine.Interfaces;
using agar.io.Engine.Types;
using agar.io.Game.Core.Types;
using agar.io.Game.Input;
using agar.io.Game.Input.Bot;
using agar.io.Game.Input.Interfaces;
using agar.io.Game.Objects;
using SFML.Graphics;
using SFML.System;

namespace agar.io.Game.Core;

public class Game : BaseGame
{
	public static Random Random = new();
	
	public static List<Player> Players = new();
	public static List<Food> FoodList = new();

	public static View Camera;
	public static float CurrentCameraZoom = 1f;
	
	private Scoreboard scoreboard = new();
	
	public Game()
	{
		Camera = new View(new FloatRect(0f, 0f, EngineConfiguration.WindowWidth, EngineConfiguration.WindowHeight));
	}

	/// <summary>
	/// Initializes the game, clears the player list and food list and adds a new player and bots
	/// </summary>
	/// <exception cref="NullReferenceException"></exception>
	public override void Initialize()
	{
		base.Initialize();
		
		Players.Clear();
		FoodList.Clear();

		try
		{
			Player player = CreatePlayer(new PlayerInput(Camera));

			Players.Add(player ?? throw new NullReferenceException());
		
			for (int i = 0; i < GameConfiguration.MaxBots; i++)
			{
				Player bot = CreatePlayer(new AdvancedBotInput ());

				Players.Add(bot ?? throw new NullReferenceException());
			}

			for (int i = 0; i < GameConfiguration.MaxFood; i++)
			{
				var food = CreateFood ();

				FoodList.Add(food ?? throw new NullReferenceException());
			}
			
			Engine.RegisterActor(scoreboard);
		}
		catch (NullReferenceException e)
		{
			Console.WriteLine("Failed to initialize game, try again");
			Initialize ();
		}

	}

	protected override void OnFrameStart()
	{
		io.Engine.Engine.Window.SetView(Camera);
		CheckZoom();
	}

	protected override void OnFrameEnd()
	{
		for (var p = 0; p < Players.Count; p++)
		{
			CheckCollisionWithFood(Players[p]);

			CheckCollisionWithPlayer(Players[p]);
		}
		UpdateCamera (Player.LocalPlayer ?? Players[0] ?? throw new NullReferenceException());
	}

	/// <summary>
	/// Check if player collided with food
	/// </summary>
	/// <param name="attacker">ID of player in Players list</param>
	/// <exception cref="NullReferenceException"></exception>
	private void CheckCollisionWithFood(Player attacker)
	{
		for (var foodId = 0; foodId < FoodList.Count; foodId++)
		{
			if (CheckCollision(attacker.PlayerBlob.Shape, FoodList[foodId].shape))
			{
				attacker.PlayerBlob.AddMass(1);

				FoodList[foodId].Destroy ();

				Food? food = CreateFood ();
				
				FoodList.Add(food ?? throw new NullReferenceException());
			}
		}
	}
	
	
	/// <summary>
	/// Check if player collided with other player
	/// </summary>
	/// <param name="attacker">ID of player in Players list</param>
	/// <exception cref="NullReferenceException"></exception>
	private void CheckCollisionWithPlayer(Player attacker)
	{
		for (var otherPlayer = 0; otherPlayer < Players.Count; otherPlayer++)
		{
			Player victim = Players[otherPlayer];
			
			if (attacker == victim)
			{
				continue;
			}
			
			if (attacker.CollidesWithPlayer(victim))
			{
				if (Math.Abs(attacker.PlayerBlob.Shape.Radius - victim.PlayerBlob.Shape.Radius) < 0.1f)
					continue;

				if (attacker.CanEat(victim))
				{
					attacker.EatPlayer(victim);
				}
				else
				{
					victim.EatPlayer(attacker);
				}
				
				var bot = CreatePlayer (new AdvancedBotInput ());
				
				Players.Add(bot ?? throw new NullReferenceException());
			}
		}
	}



	/// <summary>
	/// Checking player radius and zooming out if needed
	/// </summary>
	private void CheckZoom()
	{
		float zoomFactor = 1f + (Player.LocalPlayer.PlayerBlob.Radius / GameConfiguration.MaxRadiusUntilZoom) * 0.1f;

		if (Player.LocalPlayer.PlayerBlob.Radius >= GameConfiguration.MaxRadiusUntilZoom &&
		    GameConfiguration.MaxRadiusUntilZoom < GameConfiguration.AbsoluteMaxRadius)
		{
			GameConfiguration.MaxRadiusUntilZoom += GameConfiguration.MaxRadiusIncreaseStep;
			CurrentCameraZoom *= zoomFactor;
			Camera.Zoom(zoomFactor);
		}
	}
	
	
	/// <summary>
	/// Checks if two shapes are colliding
	/// </summary>
	/// <param name="shape1"></param>
	/// <param name="shape2"></param>
	/// <returns>The collision check</returns>
	private bool CheckCollision(Shape shape1, Shape shape2)
	{
		FloatRect rect1 = shape1.GetGlobalBounds();
		FloatRect rect2 = shape2.GetGlobalBounds();
		
		return rect1.Intersects(rect2);
	}


	/// <summary>
	/// Updates the camera position to the player position
	/// </summary>
	/// <param name="player">The target player for Camera</param>
	private void UpdateCamera(Player player)
	{
		Vector2f playerPosition = player.PlayerBlob.Position;

		Camera.Center = playerPosition;
	}
	
	/// <summary>
	/// Returns a random position on the map
	/// </summary>
	/// <returns>A random position on Map</returns>
	public static Vector2f RandomMapPosition()
	{
		return new Vector2f(Random.Next(0, GameConfiguration.MapWidth), Random.Next(0, GameConfiguration.MapHeight));
	}

	/// <summary>
	/// Returns the left top corner of the camera, can be used in UI
	/// </summary>
	/// <returns>The position of left-top corner of camera</returns>
	public static Vector2f GetLeftTopCorner()
	{
		return new Vector2f(Camera.Center.X - Camera.Size.X / 2, Camera.Center.Y - Camera.Size.Y / 2);
	}
	
	private Food? CreateFood()
	{
		Food food = Engine.RegisterActor(new Food(RandomMapPosition ())) as Food;
		return food;
	}
	
	private Player? CreatePlayer(IInput input)
	{
		string nickname = input is PlayerInput ? "Player" : "Bot " + Random.Next(0, 9999).ToString("0000");

		Player? player = Engine.RegisterActor(new Player(input, nickname)) as Player;
		return player;
	}

}