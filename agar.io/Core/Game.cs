using agar.io.Core.Types;
using agar.io.Engine.Config;
using agar.io.Input;
using agar.io.Objects;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Text = agar.io.Objects.Text;

namespace agar.io.Core;

public class Game
{
	private Engine.Engine Engine = new();

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
	private void Initialize()
	{
		Engine.DestroyAll ();
		Players.Clear();
		FoodList.Clear();

#pragma warning disable CS8600
		Player player = Engine.RegisterActor(
			new Player(RandomMapPosition (), 
				GameConfiguration.DefaultPlayerRadius, 
				new MouseInput(Camera), 
				true, 
				new Text(
					"Player",
					20, 
					Color.White, 
					new Vector2f(0f, 0f)
					)
				)
			) 
			as Player;

		Players.Add(player ?? throw new NullReferenceException());
		
		for (int i = 0; i < GameConfiguration.MaxBots; i++)
		{
			Player bot = Engine.RegisterActor(
				new Player(
					RandomMapPosition (),
					GameConfiguration.DefaultPlayerRadius, 
					new BotInput(), 
					false, 
					new Text(
						"Bot " + Random.Next(0, 1000).ToString("0000"),
						20, 
						Color.White, 
						new Vector2f(0f, 0f))
					)
				) 
				as Player;
			
			
			Players.Add(bot ?? throw new NullReferenceException());
		}

		for (int i = 0; i < GameConfiguration.MaxFood; i++)
		{
			Food food = Engine.RegisterActor(new Food(RandomMapPosition ())) as Food;

			FoodList.Add(food ?? throw new NullReferenceException());
		}
		
#pragma warning restore CS8600
		
		Engine.RegisterActor(scoreboard);


	}
	
	public void Run()
	{
		Initialize ();
		
		Engine.OnFrameStart += OnFrameStart;
		Engine.OnFrameEnd += OnFrameEnd;
		
		Engine.Run();
	}
	
	private void OnFrameEnd()
	{
		for (var pId = 0; pId < Players.Count; pId++)
		{
			CheckCollisionWithFood(pId);

			CheckCollisionWithPlayer(pId);
		}
		UpdateCamera (Player.LocalPlayer ?? Players[0] ?? throw new NullReferenceException());

	}
	private void OnFrameStart()
	{
		io.Engine.Engine.Window.SetView(Camera);
		CheckZoom();
	}

	/// <summary>
	/// Check if player collided with food
	/// </summary>
	/// <param name="playerId">ID of player in Players list</param>
	/// <exception cref="NullReferenceException"></exception>
	private void CheckCollisionWithFood(int playerId)
	{
		for (var foodId = 0; foodId < FoodList.Count; foodId++)
		{
			if (CheckCollision(Players[playerId].Shape, FoodList[foodId].shape))
			{
				Players[playerId].AddMass(1);

				FoodList[foodId].Destroy ();

				Food? food = Engine.RegisterActor(new Food(RandomMapPosition ())) as Food;
				
				FoodList.Add(food ?? throw new NullReferenceException());
			}
		}
	}
	
	
	/// <summary>
	/// Check if player collided with other player
	/// </summary>
	/// <param name="playerId">ID of player in Players list</param>
	/// <exception cref="NullReferenceException"></exception>
	private void CheckCollisionWithPlayer(int playerId)
	{
		for (var otherPlayer = 0; otherPlayer < Players.Count; otherPlayer++)
		{
			if (otherPlayer == playerId)
			{
				continue;
			}

			if (CheckCollision(Players[playerId].Shape, Players[otherPlayer].Shape))
			{
				if (Players[playerId].Shape.Radius > Players[otherPlayer].Shape.Radius)
				{
					Players[playerId].AddMass(Players[otherPlayer].Shape.Radius / 2);

					Players[otherPlayer].Destroy ();
				}
				else
				{
					Players[otherPlayer].AddMass(Players[playerId].Shape.Radius / 2);

					Players[playerId].Destroy ();
				}
				
				Player? bot = Engine.RegisterActor(
						new Player(
							RandomMapPosition (),
							GameConfiguration.DefaultPlayerRadius, 
							new BotInput(), 
							false, 
							new Text(
								"Bot " + Random.Next(0, 1000).ToString("0000"),
								20, 
								Color.White, 
								new Vector2f(0f, 0f)
							)
						)
					) 
					as Player;
				
				Players.Add(bot ?? throw new NullReferenceException());
			}
		}
	}

	/// <summary>
	/// Checking player radius and zooming out if needed
	/// </summary>
	private void CheckZoom()
	{
		float zoomFactor = 1f + (Player.LocalPlayer.Radius / GameConfiguration.MaxRadiusUntilZoom) * 0.1f;

		if (Player.LocalPlayer.Radius >= GameConfiguration.MaxRadiusUntilZoom &&
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
		Vector2f playerPosition = player.Position;

		Camera.Center = playerPosition;
	}
	
	/// <summary>
	/// Returns a random position on the map
	/// </summary>
	/// <returns>A random position on Map</returns>
	private Vector2f RandomMapPosition()
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

}