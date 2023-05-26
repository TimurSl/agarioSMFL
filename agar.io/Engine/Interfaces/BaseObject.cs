namespace agar.io.Engine.Interfaces;

public abstract class BaseObject
{
	public bool IsInitialized { get; set; }
	
	/// <summary>
	/// Destroy this object, can be overriden, but make sure to call base.Destroy() at the end.
	/// </summary>
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
		
		IsInitialized = false;
	}
}