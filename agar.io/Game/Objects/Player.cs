﻿using agar.io.Engine.Interfaces;
using agar.io.Game.Core.Types;
using agar.io.Game.Input.Interfaces;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Time = agar.io.Engine.Types.Time;

namespace agar.io.Game.Objects;

public class Player : BaseObject, IDrawable, IUpdatable
{
	public readonly CircleShape Shape;
	public bool IsPlayer = false;
	public Vector2f Position;
	public readonly Text NickNameLabel;
	public readonly string NickName = "Player";

	public int ZIndex { get; set; } = 1;

	private float movementSpeed = 200f;
	public IInput input;
	
	public static Player LocalPlayer { get; set; }
	
	public float Radius
	{
		get => Shape.Radius;
		set => Shape.Radius = value;
	}

	public new bool IsInitialized { get; set; }


	public Player(Vector2f pos, int radius, IInput input, bool isPlayer = false, Text nickName = null)
	{
		Color randomColor = new Color((byte)Core.Game.Random.Next(0, 255), (byte)Core.Game.Random.Next(0, 255), (byte)Core.Game.Random.Next(0, 255));

		Shape = new CircleShape(radius);
		Shape.Position = pos;
		Shape.FillColor = randomColor;
		Shape.Origin = new Vector2f(radius, radius);

		if (isPlayer)
		{
			Shape.OutlineColor = Color.Black;
			Shape.OutlineThickness = 3f;
			LocalPlayer = this;
		}
		
		
		this.Position = pos;
		this.input = input;
		this.IsPlayer = isPlayer;
		this.NickNameLabel = nickName;
		this.NickName = nickName?.GetMessage () ?? "Player";
		
		IsInitialized = true;
	}


	public void Draw(RenderTarget target)
	{
		target.Draw(Shape);
		NickNameLabel.Draw(target);
	}

	
	public void Update()
	{
		if (this != LocalPlayer)
		{
			Shape.OutlineThickness = 3;

			if (Radius > LocalPlayer.Radius)
			{
				Shape.OutlineColor = GameConfiguration.darkRed;
			}
			else if (Shape.Radius < LocalPlayer.Shape.Radius)
			{
				Shape.OutlineColor = GameConfiguration.darkGreen;
			}
			else
			{
				Shape.OutlineThickness = 0;
			}
		}
		else
		{
			if (Keyboard.IsKeyPressed(Keyboard.Key.R))
			{
				ChangeSoul ();
			}
		}
		
		UpdateMovement();

		NickNameLabel.SetPosition(new Vector2f(Shape.Position.X, Shape.Position.Y - Shape.Radius - 30));
		
		movementSpeed = 200f - (Shape.Radius / 10f);
	}

	/// <summary>
	/// Updates the movement of the player.
	/// </summary>
	private void UpdateMovement()
	{
		Vector2f targetPosition = input.GetDirection(Engine.Engine.Window);
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

	/// <summary>
	/// Clamps the movement of the player.
	/// </summary>
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

	/// <summary>
	/// Adds mass to the player. Radius is increased by the mass.
	/// </summary>
	/// <param name="mass">The amount of mass</param>
	public void AddMass(float mass)
	{
		if (Shape.Radius + mass > GameConfiguration.AbsoluteMaxRadius)
			return;
		
		Shape.Radius += mass;
		Shape.Radius = Math.Clamp(Shape.Radius, 0, GameConfiguration.AbsoluteMaxRadius);
		
		Shape.Origin = new Vector2f(Shape.Radius, Shape.Radius);
		Shape.Radius = MathF.Floor(Shape.Radius);
	}

	private void ChangeSoul()
	{
		Player oldPlayer = this;
		Player newPlayer = Core.Game.Players[Core.Game.Random.Next(0, Core.Game.Players.Count)];

		while (newPlayer == oldPlayer)
		{
			newPlayer = Core.Game.Players[Core.Game.Random.Next(0, Core.Game.Players.Count)];
		}

		(oldPlayer.Shape.Position, newPlayer.Shape.Position) = (newPlayer.Shape.Position, oldPlayer.Shape.Position);
		(oldPlayer.Radius, newPlayer.Radius) = (newPlayer.Radius, oldPlayer.Radius);
		(oldPlayer.Shape.FillColor, newPlayer.Shape.FillColor) = (newPlayer.Shape.FillColor, oldPlayer.Shape.FillColor);
		(oldPlayer.input, newPlayer.input) = (newPlayer.input, oldPlayer.input);
		
		oldPlayer.Shape.Origin = new Vector2f(oldPlayer.Shape.Radius, oldPlayer.Shape.Radius);
		newPlayer.Shape.Origin = new Vector2f(newPlayer.Shape.Radius, newPlayer.Shape.Radius);
		
		oldPlayer.IsPlayer = false;
		newPlayer.IsPlayer = true;
		
	}

	/// <summary>
	/// Destroys the player. Removes it from the game.
	/// </summary>
	public new void Destroy()
	{
		base.Destroy();
		
		if (this == LocalPlayer)
		{
			ChangeSoul ();
			return;
		}
		
		Console.WriteLine($"Player {NickName} died!");
		
		Core.Game.Players.Remove(this);
	}

}