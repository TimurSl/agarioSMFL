namespace agar.io.Engine.Interfaces;

public abstract class BaseObject
{
	public bool IsInitialized { get; set; }
	internal void Destroy()
	{
		if (this is IUpdatable updatable)
		{
			Engine.updatables.Remove(updatable);
		}
		
		if (this is IDrawable drawable)
		{
			Engine.drawables.Remove(drawable);
		}
	}
}