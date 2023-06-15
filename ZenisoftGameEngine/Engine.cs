using SFML.Graphics;
using SFML.Window;
using ZenisoftGameEngine.Config;
using ZenisoftGameEngine.Interfaces;
using ZenisoftGameEngine.Types;

namespace ZenisoftGameEngine;

public class Engine
{
	public static RenderWindow Window;
	
	public static List<IDrawable> drawables = new();
	public static List<IUpdatable> updatables = new();
	
	public Action OnFrameStart;
	public Action OnFrameEnd;

	public Engine()
	{
		VideoMode videoMode = new VideoMode(EngineConfiguration.WindowWidth, EngineConfiguration.WindowHeight);
		
		Styles styles = Styles.Titlebar | Styles.Close;
		
		if (EngineConfiguration.Fullscreen)
			styles |= Styles.Fullscreen;
		
		Window = new RenderWindow(videoMode, EngineConfiguration.WindowTitle, styles);
		Window.SetFramerateLimit(EngineConfiguration.FrameRateLimit);
	}
	
	public void Run()
	{
		while (Window.IsOpen)
		{
			Window.DispatchEvents();
			Window.Clear(EngineConfiguration.BackgroundColor);
			
			OnFrameStart?.Invoke();
			
			Draw ();
			Update ();
			
			OnFrameEnd?.Invoke();
			
			Window.Display();
		}
	}

	/// <summary>
	/// Draws all the objects in the game
	/// </summary>
	private void Draw()
	{
		Window.DispatchEvents();
		Window.Clear(Color.White);
		
		drawables.Sort((drawable, drawable1) => drawable.ZIndex.CompareTo(drawable1.ZIndex));
		
		foreach (IDrawable drawable in drawables)
		{
			if (drawable is BaseObject baseObject && !baseObject.IsInitialized)
			{
				Console.WriteLine($"Object {baseObject} is not initialized yet!");
				continue;
			}
			drawable.Draw(Window);
		}
	}
	
	/// <summary>
	/// Updates all the objects in the game
	/// </summary>
	private void Update()
	{
		Time.Update ();

		foreach (IUpdatable updatable in updatables)
		{
			if (updatable is BaseObject baseObject && !baseObject.IsInitialized)
			{
				Console.WriteLine($"Object {baseObject} is not initialized yet!");
				continue;
			}
			updatable.Update();
		}
	}
	
	/// <summary>
	/// Registers an actor to the game, adds him to lists and sets IsInitialized to true
	/// </summary>
	/// <param name="actor">The object that must be initialized</param>
	/// <returns>The object that successfully initialized</returns>
	public BaseObject RegisterActor(BaseObject actor)
	{
		if (actor is IUpdatable updatable)
		{
			updatables.Add(updatable);
		}
		
		if (actor is IDrawable drawable)
		{
			drawables.Add(drawable);
		}
		
		actor.IsInitialized = true;
		
		return actor;
	}

	/// <summary>
	/// Clears all the objects in the game
	/// </summary>
	public void DestroyAll()
	{
		List<BaseObject> objects = new();
		
		foreach (IUpdatable updatable in updatables)
		{
			if (updatable is BaseObject baseObject && !objects.Contains(baseObject))
			{
				objects.Add(baseObject);
			}
		}
		
		foreach (IDrawable drawable in drawables)
		{
			if (drawable is BaseObject baseObject && !objects.Contains(baseObject))
			{
				objects.Add(baseObject);
			}
		}
		
		foreach (BaseObject baseObject in objects)
		{
			baseObject.IsInitialized = false;
			baseObject.Destroy();
		}
	}
}