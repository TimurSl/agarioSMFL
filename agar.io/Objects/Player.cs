using agar.io.Core;
using agar.io.Core.Types;
using agar.io.Input.Interfaces;
using agar.io.Objects.Interfaces;
using SFML.Graphics;
using SFML.System;
using Time = agar.io.Core.Types.Time;

namespace agar.io.Objects;

public class Player : IDrawable, IUpdatable
{
	public readonly CircleShape Shape;
	public readonly bool IsPlayer = false;
	public Vector2f Position;
	public readonly Text NickNameLabel;
	public readonly string NickName = "Player";

	public int ZIndex { get; set; } = 1;

	private readonly float movementSpeed = 200f;
	public readonly IInput input;


	public Player(Vector2f pos, int radius, IInput input, bool isPlayer = false, Text nickName = null)
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
		
		
		this.Position = pos;
		this.input = input;
		this.IsPlayer = isPlayer;
		this.NickNameLabel = nickName;
		this.NickName = nickName?.GetMessage () ?? "Player";
	}
	
	~Player()
	{
		Shape.Dispose();
		NickNameLabel.TextClass.Dispose ();
	}


	public void Draw(RenderTarget target)
	{
		target.Draw(Shape);
		NickNameLabel.Draw(target);
	}

	public void Update()
	{
		// Shape.Radius += 0.1f;
		Shape.Origin = new Vector2f(Shape.Radius, Shape.Radius);
		
		UpdateMovement();

		NickNameLabel.SetPosition(new Vector2f(Shape.Position.X, Shape.Position.Y - Shape.Radius - 30));
	}

	private void UpdateMovement()
	{
		Vector2f targetPosition = input.GetDirection(Game.Window);
		Vector2f direction = targetPosition - Shape.Position;

		if (direction != new Vector2f(0, 0))
		{
			float magnitude = MathF.Sqrt((direction.X * direction.X) + (direction.Y * direction.Y));
			direction /= magnitude;

			Position += direction * movementSpeed * Time.DeltaTime;
		}
		
		ClampMovement ();

		Shape.Position = Position;
	}

	private void ClampMovement()
	{
		if (Position.X < Shape.Radius)
			Position.X = Shape.Radius;
		else if (Position.X > GameConfiguration.MapWidth - Shape.Radius)
			Position.X = GameConfiguration.MapWidth - Shape.Radius;

		if (Position.Y < Shape.Radius)
			Position.Y = Shape.Radius;
		else if (Position.Y > GameConfiguration.MapWidth - Shape.Radius)
			Position.Y = GameConfiguration.MapWidth - Shape.Radius;
	}
	
	public void AddMass(float mass)
	{
		if (Shape.Radius + mass > GameConfiguration.AbsoluteMaxRadius)
			return;
		
		Shape.Radius += mass;
		Shape.Radius = Math.Clamp(Shape.Radius, 0, GameConfiguration.AbsoluteMaxRadius);
		
		Shape.Origin = new Vector2f(Shape.Radius, Shape.Radius);
		Shape.Radius = MathF.Floor(Shape.Radius);
	}
	
}