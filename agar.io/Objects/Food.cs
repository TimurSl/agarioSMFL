using agar.io.Core;
using agar.io.Objects.Interfaces;
using SFML.Graphics;
using SFML.System;

namespace agar.io.Objects;

public class Food : IDrawable
{
	public CircleShape shape;
	public int ZIndex { get; set; } = 0;

	private float radius = 5f;


	public Food(Vector2f position)
	{
		Color randomColor = new Color((byte)Game.Random.Next(0, 255), (byte)Game.Random.Next(0, 255), (byte)Game.Random.Next(0, 255));

		shape = new CircleShape(radius);
		shape.Position = position;
		shape.Origin = new Vector2f(radius, radius);
		shape.FillColor = randomColor;
	}

	public void Draw(RenderTarget target)
	{
		target.Draw(shape);
	}
}