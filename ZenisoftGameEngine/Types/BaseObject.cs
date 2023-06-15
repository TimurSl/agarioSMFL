using ZenisoftGameEngine.Interfaces;

namespace ZenisoftGameEngine.Types;

public abstract class BaseObject
{
	public bool IsInitialized { get; set; }
	
	/// <summary>
	/// Destroy this object, can be overriden, but make sure to call base.Destroy() at the end.
	/// </summary>
	public void Destroy()
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