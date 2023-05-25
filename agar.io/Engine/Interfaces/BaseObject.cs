namespace agar.io.Engine.Interfaces;

public abstract class BaseObject
{
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