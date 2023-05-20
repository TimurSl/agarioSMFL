using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace agar.io;

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
		Window = new RenderWindow(videoMode, "Agar.io", Styles.Titlebar | Styles.Close);
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
		
		CreatePlayer(new Vector2f(GameConfiguration.MapWidth / 2, GameConfiguration.MapHeight / 2), new MouseInput ());
		for (int i = 0; i < 20; i++)
		{
			CreatePlayer(new BotInput ());
		}

		for (int i = 0; i < 1000; i++)
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
			UpdateObjects();

			for (var i = 0; i < players.Count; i++)
			{
				CheckCollisionWithFood(i);

				CheckCollisionWithPlayer(i);
			}

			// Window.SetView(Window.DefaultView);

			Window.Display();
		}

	}

	private void CheckCollisionWithFood(int i)
	{
		for (var i1 = 0; i1 < food.Count; i1++)
		{
			if (CheckCollision(players[i].Shape, food[i1].shape))
			{
				players[i].Shape.Radius += 1;

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
					if (players[i].Shape.Radius < GameConfiguration.MaxRadius)
						players[i].Shape.Radius += players[otherPlayer].Shape.Radius / 2;

					DeletePlayer(players[otherPlayer]);
				}
				else
				{
					if (players[otherPlayer].Shape.Radius < GameConfiguration.MaxRadius)
						players[otherPlayer].Shape.Radius += players[i].Shape.Radius / 2;

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
		
		foreach (IDrawable drawable in drawables)
		{
			drawable.Draw(Window);
		}
	}
	
	private void UpdateObjects()
	{
		foreach (IUpdatable updatable in updatables)
		{
			updatable.Update();
		}
	}
	
	private void CreatePlayer(IInput input)
	{
		Vector2f position = new Vector2f(Random.Next(0, (int) GameConfiguration.MapWidth), Random.Next(0, (int) GameConfiguration.MapHeight));
		Player player = new Player(position, GameConfiguration.DefaultPlayerRadius, input, input is MouseInput);
		RegisterActor(player, player);
		players.Add(player);
	}
	
	private void CreatePlayer(Vector2f pos, IInput input)
	{
		Player player = new Player(pos, GameConfiguration.DefaultPlayerRadius, input, input is MouseInput);
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
		drawables.Remove(player);
		players.Remove(player);
	}

	private void UpdateCamera()
	{
		mainPlayer = players.Find(player => player.IsPlayer) ?? players[0];
		Vector2f playerPosition = mainPlayer.position;

		camera.Center = playerPosition;
	}
}