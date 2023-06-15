using SFML.Graphics;

namespace ZenisoftGameEngine.Interfaces;

public interface IDrawable
{
	/// <summary>
	/// The ZIndex of the object, the higher the ZIndex, the more it will be drawn on top of other objects.
	/// </summary>
	public int ZIndex { get; set; }
	
	/// <summary>
	/// Draw the object on the screen
	/// </summary>
	/// <param name="target">The RenderTarget for draw</param>
	public void Draw(RenderTarget target);
}