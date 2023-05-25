﻿using agar.io.Core.Types;
using agar.io.Engine.Config;
using agar.io.Engine.Interfaces;
using agar.io.Input;
using agar.io.Input.Interfaces;
using agar.io.Objects;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Text = agar.io.Objects.Text;
using Time = agar.io.Engine.Types.Time;

namespace agar.io.Core;

public class Game
{
	public static RenderWindow Window;
	public static Random Random = new Random();
	
	private List<IDrawable> drawables = new();
	private List<IUpdatable> updatables = new();
	
	private List<Player> players = new();
	private List<Food> food = new();

	private View camera;

	private Player mainPlayer;
	
	public Game()
	{
		VideoMode videoMode = new VideoMode(EngineConfiguration.WindowWidth, EngineConfiguration.WindowHeight);
		Window = new RenderWindow(videoMode, "Agar.io Clone", Styles.Titlebar | Styles.Close);
		Window.SetFramerateLimit(120);
		
		camera = new View(new FloatRect(0f, 0f, EngineConfiguration.WindowWidth, EngineConfiguration.WindowHeight));
		mainPlayer = players.Find(player => player.IsPlayer);
	}

	private void Initialize()
	{
		drawables.Clear();
		updatables.Clear();
		players.Clear();
		food.Clear();
		
		CreatePlayer(new MouseInput (camera));
		for (int i = 0; i < GameConfiguration.MaxBots; i++)
		{
			CreatePlayer(new BotInput ());
		}

		for (int i = 0; i < GameConfiguration.MaxFood; i++)
		{
			CreateFood ();
		}
	}
	
	public void Run()
	{
		Initialize ();
		Window.Closed += (sender, args) => Window.Close();
		while (Window.IsOpen)
		{
			UpdateCamera();
			
			Draw();
			Update();

			for (var pId = 0; pId < players.Count; pId++)
			{
				CheckCollisionWithFood(pId);

				CheckCollisionWithPlayer(pId);
			}

			Window.Display();
		}

	}

	private void CheckCollisionWithFood(int playeId)
	{
		for (var i1 = 0; i1 < food.Count; i1++)
		{
			if (CheckCollision(players[playeId].Shape, food[i1].shape))
			{
				players[playeId].AddMass(1);

				DeleteFood(food[i1]);

				CreateFood ();
			}
		}
	}

	private void CheckCollisionWithPlayer(int playerId)
	{
		for (var otherPlayer = 0; otherPlayer < players.Count; otherPlayer++)
		{
			if (otherPlayer == playerId)
			{
				continue;
			}

			if (CheckCollision(players[playerId].Shape, players[otherPlayer].Shape))
			{
				if (players[playerId].Shape.Radius > players[otherPlayer].Shape.Radius)
				{
					players[playerId].AddMass(players[otherPlayer].Shape.Radius / 2);

					DeletePlayer(players[otherPlayer]);
				}
				else
				{
					players[otherPlayer].AddMass(players[playerId].Shape.Radius / 2);

					DeletePlayer(players[playerId]);
				}
				
				
				CreatePlayer(new BotInput ());
			}
		}
	}

	private void RegisterActor(IDrawable? drawable = null, IUpdatable? updatable = null)
	{
		if (drawable != null && !drawables.Contains(drawable))
		{
			drawables.Add(drawable);
		}
		
		if (updatable != null && !updatables.Contains(updatable))
		{
			updatables.Add(updatable);
		}
	}
	
	private void Draw()
	{
		Window.DispatchEvents();
		Window.Clear(Color.White);
		Window.SetView(camera);

		// sort drawables by z-index, so that the ones with the highest z-index are drawn last
		drawables.Sort((drawable, drawable1) => drawable.ZIndex.CompareTo(drawable1.ZIndex));
		
		foreach (IDrawable drawable in drawables)
		{
			drawable.Draw(Window);
		}

		// zoom out if player is too big
		float zoomFactor = 1f + (Player.LocalPlayer.Radius / GameConfiguration.MaxRadiusUntilZoom) * 0.1f;

		if (Player.LocalPlayer.Shape.Radius >= GameConfiguration.MaxRadiusUntilZoom &&
		    GameConfiguration.MaxRadiusUntilZoom < GameConfiguration.AbsoluteMaxRadius)
		{
			GameConfiguration.MaxRadiusUntilZoom += GameConfiguration.MaxRadiusIncreaseStep;
			camera.Zoom(zoomFactor);
		}
	}
	
	private void Update()
	{
		Time.Update ();
		foreach (IUpdatable updatable in updatables)
		{
			updatable.Update();
		}
	}
	
	private void CreatePlayer(IInput input)
	{
		string name = $"Player {Random.Next(1000).ToString("0000")}";
		if (input is BotInput)
		{
			name = $"Bot {Random.Next(1000).ToString("0000")}";
		}
		
		Text text = new Text(name, 20, Color.White, new Vector2f(0, 0));
		RegisterActor(text);
		
		Vector2f position = new Vector2f(Random.Next(0, (int) GameConfiguration.MapWidth), Random.Next(0, (int) GameConfiguration.MapHeight));
		Player player = new Player(position, GameConfiguration.DefaultPlayerRadius, input, input is MouseInput, text);
		
		RegisterActor(player, player);
		players.Add(player);
	}

	private void CreateFood()
	{
		Food food = new Food(new Vector2f((float) Random.NextDouble () * GameConfiguration.MapWidth,
			(float) Random.NextDouble () * GameConfiguration.MapHeight));
		RegisterActor(food);
		this.food.Add(food);
	}
	
	private bool CheckCollision(Shape shape1, Shape shape2)
	{
		FloatRect rect1 = shape1.GetGlobalBounds();
		FloatRect rect2 = shape2.GetGlobalBounds();
		
		return rect1.Intersects(rect2);
	}
	
	private void DeleteFood(Food food)
	{
		drawables.Remove(food);
		this.food.Remove(food);
	}
	
	private void DeletePlayer(Player player)
	{
		Console.WriteLine($"Player {player.NickName} died");
		drawables.Remove(player);
		drawables.Remove(player.NickNameLabel);
		
		players.Remove(player);
		
		player = null;
	}

	private void UpdateCamera()
	{
		if (Player.LocalPlayer == null)
			return;
		
		Vector2f playerPosition = Player.LocalPlayer.Position;

		camera.Center = playerPosition;
	}
	
}