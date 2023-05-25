using SFML.Graphics;

namespace agar.io.Engine.Interfaces;

public interface IDrawable
{
	public int ZIndex { get; set; }
	public void Draw(RenderTarget target);
}