using SFML.Graphics;
using SFML.System;
using ZenisoftGameEngine.Interfaces;
using ZenisoftGameEngine.Types;

namespace agar.io.Game.Objects;

public class Food : BaseObject, IDrawable 
{
	public CircleShape shape;
	public int ZIndex { get; set; } = 0;

	private float radius = 5f;
	
	public Vector2f Position
	{
		get => shape.Position;
		set => shape.Position = value;
	}


	public Food(Vector2f position)
	{
		Color randomColor = new Color((byte)Core.Game.Random.Next(0, 255), (byte)Core.Game.Random.Next(0, 255), (byte)Core.Game.Random.Next(0, 255));

		shape = new CircleShape(radius);
		shape.Position = position;
		shape.Origin = new Vector2f(radius, radius);
		shape.FillColor = randomColor;
		
		IsInitialized = true;
	}

	public void Draw(RenderTarget target)
	{
		target.Draw(shape);
	}

	public new void Destroy()
	{
		base.Destroy();
		Core.Game.FoodList.Remove(this);
	}
}