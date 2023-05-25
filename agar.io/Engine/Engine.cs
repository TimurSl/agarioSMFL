using agar.io.Engine.Config;
using agar.io.Engine.Interfaces;
using agar.io.Engine.Types;
using SFML.Graphics;
using SFML.Window;

namespace agar.io.Engine;

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
		Window = new RenderWindow(videoMode, "Agar.io Clone", Styles.Titlebar | Styles.Close);
		Window.SetFramerateLimit(EngineConfiguration.FrameRateLimit);
	}
	
	public void Run()
	{
		Window.Closed += (sender, args) => Window.Close();
		
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

	internal void Draw()
	{
		Window.DispatchEvents();
		Window.Clear(Color.White);

		// sort drawables by z-index, so that the ones with the highest z-index are drawn last
		drawables.Sort((drawable, drawable1) => drawable.ZIndex.CompareTo(drawable1.ZIndex));
		
		foreach (IDrawable drawable in drawables)
		{
			drawable.Draw(Window);
		}
	}
	
	internal void Update()
	{
		Time.Update ();
		foreach (IUpdatable updatable in updatables)
		{
			updatable.Update();
		}
	}
	
	public void RegisterActor(BaseObject actor)
	{
		if (actor is IUpdatable updatable)
		{
			updatables.Add(updatable);
		}
		
		if (actor is IDrawable drawable)
		{
			drawables.Add(drawable);
		}
	}
}