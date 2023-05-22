using agar.io.Core.Types;
using agar.io.Input;
using agar.io.Input.Interfaces;
using agar.io.Objects;
using agar.io.Objects.Interfaces;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Text = agar.io.Objects.Text;

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
		VideoMode videoMode = new VideoMode(GameConfiguration.WindowWidth, GameConfiguration.WindowHeight);
		Window = new RenderWindow(videoMode, "Agar.io Clone", Styles.Titlebar | Styles.Close);
		Window.SetFramerateLimit(120);
		
		camera = new View(new FloatRect(0f, 0f, GameConfiguration.WindowWidth, GameConfiguration.WindowHeight));
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

			float zoomFactor = 1f + (mainPlayer.Shape.Radius / GameConfiguration.MaxRadius) * 0.1f;

			if (mainPlayer.Shape.Radius >= GameConfiguration.MaxRadius &&
			    GameConfiguration.MaxRadius < GameConfiguration.AbsoluteMaxRadius)
			{
				GameConfiguration.MaxRadius += GameConfiguration.MaxRadiusIncreaseStep;
				camera.Zoom(zoomFactor);
			}

			for (var i = 0; i < players.Count; i++)
			{
				
				CheckCollisionWithFood(i);

				CheckCollisionWithPlayer(i);
			}

			Window.Display();
		}

	}

	private void CheckCollisionWithFood(int i)
	{
		for (var i1 = 0; i1 < food.Count; i1++)
		{
			if (CheckCollision(players[i].Shape, food[i1].shape))
			{
				players[i].AddMass(1);

				DeleteFood(food[i1]);

				CreateFood ();
			}
		}
	}

	private void CheckCollisionWithPlayer(int i)
	{
		for (var otherPlayer = 0; otherPlayer < players.Count; otherPlayer++)
		{
			if (otherPlayer == i)
			{
				continue;
			}

			if (CheckCollision(players[i].Shape, players[otherPlayer].Shape))
			{
				if (players[i].Shape.Radius > players[otherPlayer].Shape.Radius)
				{
					players[i].AddMass(players[otherPlayer].Shape.Radius / 2);

					DeletePlayer(players[otherPlayer]);
				}
				else
				{
					players[otherPlayer].AddMass(players[i].Shape.Radius / 2);

					DeletePlayer(players[i]);
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

		drawables.Sort((drawable, drawable1) => drawable.ZIndex.CompareTo(drawable1.ZIndex));
		
		foreach (IDrawable drawable in drawables)
		{
			drawable.Draw(Window);
		}
		
		foreach(Player player in players)
		{
			if (player.IsPlayer)
				continue;
			
			player.Shape.OutlineThickness = 3;

			if (player.Shape.Radius > mainPlayer.Shape.Radius)
			{
				player.Shape.OutlineColor = GameConfiguration.darkRed;
			}
			else if (player.Shape.Radius < mainPlayer.Shape.Radius)
			{
				player.Shape.OutlineColor = GameConfiguration.darkGreen;
			}
			else
			{
				player.Shape.OutlineThickness = 0;
			}
		}
	}
	
	private void Update()
	{
		Types.Time.Update ();
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
		mainPlayer = players.Find(player => player.IsPlayer) ?? players[0];
		Vector2f playerPosition = mainPlayer.Position;

		camera.Center = playerPosition;
	}
	
}