using SFML.Graphics;

namespace agar.io.Objects.Interfaces;

public interface IDrawable
{
	public int ZIndex { get; set; }
	public void Draw(RenderTarget target);
}