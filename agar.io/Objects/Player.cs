using agar.io.Core;
using agar.io.Core.Types;
using agar.io.Input.Interfaces;
using agar.io.Objects.Interfaces;
using SFML.Graphics;
using SFML.System;

namespace agar.io.Objects;

public class Player : IDrawable, IUpdatable
{
	public CircleShape Shape;
	public bool IsPlayer = false;
	public Vector2f position;

	private float movementSpeed = 2f;
	private IInput input;

	public Player(Vector2f pos, int radius, IInput input, bool isPlayer = false)
	{
		Color randomColor = new Color((byte)Game.Random.Next(0, 255), (byte)Game.Random.Next(0, 255), (byte)Game.Random.Next(0, 255));

		Shape = new CircleShape(radius);
		Shape.Position = pos;
		Shape.FillColor = randomColor;
		Shape.Origin = new Vector2f(radius, radius);
		
		if (isPlayer)
		{
			Shape.OutlineColor = Color.Black;
			Shape.OutlineThickness = 3f;
		}
		
		
		this.position = pos;
		this.input = input;
		this.IsPlayer = isPlayer;
	}
	
	
	public void Draw(RenderTarget target)
	{
		target.Draw(Shape);
	}

	public void Update()
	{
		// Shape.Radius += 0.1f;
		Shape.Origin = new Vector2f(Shape.Radius, Shape.Radius);
		
		UpdateMovement();
	}

	private void UpdateMovement()
	{
		Vector2f targetPosition = input.GetDirection(Game.Window);
		Vector2f direction = targetPosition - Shape.Position;

		if (direction != new Vector2f(0, 0))
		{
			float magnitude = MathF.Sqrt((direction.X * direction.X) + (direction.Y * direction.Y));
			direction /= magnitude;

			position += direction * movementSpeed;
		}
		
		ClampMovement ();

		Shape.Position = position;
	}

	private void ClampMovement()
	{
		if (position.X < Shape.Radius)
			position.X = Shape.Radius;
		else if (position.X > GameConfiguration.MapWidth - Shape.Radius)
			position.X = GameConfiguration.MapWidth - Shape.Radius;

		if (position.Y < Shape.Radius)
			position.Y = Shape.Radius;
		else if (position.Y > GameConfiguration.MapWidth - Shape.Radius)
			position.Y = GameConfiguration.MapWidth - Shape.Radius;
	}
	
	public void AddMass(float mass)
	{
		if (Shape.Radius + mass > GameConfiguration.MaxRadius)
			return;
		
		Shape.Radius += mass;
		Shape.Origin = new Vector2f(Shape.Radius, Shape.Radius);
	}
}