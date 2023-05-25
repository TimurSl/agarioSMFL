using agar.io.Core.Types;
using agar.io.Engine.Interfaces;
using SFML.System;

namespace agar.io.Objects;

public class Text : IDrawable
{
	public int ZIndex { get; set; } = 999;
	public SFML.Graphics.Text TextClass;
	
	public Text(string message, uint size, SFML.Graphics.Color color, Vector2f position)
	{
		TextClass = new SFML.Graphics.Text(message, GameConfiguration.Font, size);
		TextClass.Position = position;
		TextClass.FillColor = color;
		TextClass.OutlineColor = SFML.Graphics.Color.Black;
		TextClass.OutlineThickness = 1f;
		TextClass.Origin = new Vector2f(TextClass.GetLocalBounds().Width / 2, TextClass.GetLocalBounds().Height / 2);
	}

	public void Draw(SFML.Graphics.RenderTarget target)
	{
		target.Draw(TextClass);
	}
	
	public void SetPosition(Vector2f position)
	{
		TextClass.Position = position;
	}
	
	public void SetMessage(string message)
	{
		TextClass.DisplayedString = message;
	}
	
	public void SetColor(SFML.Graphics.Color color)
	{
		TextClass.FillColor = color;
	}
	
	public string GetMessage()
	{
		return TextClass.DisplayedString;
	}
}