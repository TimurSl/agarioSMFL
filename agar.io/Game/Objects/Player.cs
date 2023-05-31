﻿using System.Diagnostics;
using agar.io.Engine.Interfaces;
using agar.io.Game.Core.Types;
using agar.io.Game.Input;
using agar.io.Game.Input.Interfaces;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Time = agar.io.Engine.Types.Time;

namespace agar.io.Game.Objects;

public class Player : BaseObject, IDrawable, IUpdatable
{
	public static Player LocalPlayer { get; set; }
	public bool IsPlayer = false;

	public IInput input;
	public Blob PlayerBlob;

	public new bool IsInitialized { get; set; }
	public int ZIndex { get; set; } = 1;
	
	private Vector2f tempPosition;
	private float movementSpeed = GameConfiguration.MovementSpeed;


	public Player(IInput input, string nickName)
	{
		PlayerBlob = new Blob(nickName);
		tempPosition = PlayerBlob.Position;

		if (input is PlayerInput)
		{
			LocalPlayer = this;
			IsPlayer = true;
		}

		this.input = input;
		this.input.ControllerPlayer = this;

		BindKeys ();

		this.IsPlayer = this.input is PlayerInput;
	}

	public void Draw(RenderTarget target)
	{
		target.Draw(PlayerBlob.Shape);
		PlayerBlob.NickNameLabel.Draw(target);
	}


	public void Update()
	{
		input.HandleInput(Engine.Engine.Window);
		
		if (this != LocalPlayer)
		{
			PlayerBlob.Shape.OutlineThickness = 3;

			if (PlayerBlob.Radius > LocalPlayer.PlayerBlob.Radius)
			{
				PlayerBlob.Shape.OutlineColor = GameConfiguration.darkRed;
			}
			else if (PlayerBlob.Radius < LocalPlayer.PlayerBlob.Radius)
			{
				PlayerBlob.Shape.OutlineColor = GameConfiguration.darkGreen;
			}
			else
			{
				PlayerBlob.Shape.OutlineThickness = 0;
			}
		}
		else
		{
			PlayerBlob.Shape.OutlineThickness = 3;
			PlayerBlob.Shape.OutlineColor = Color.Black;
		}
		

		UpdateMovement();

		PlayerBlob.NickNameLabel.SetPosition(PlayerBlob.Position + new Vector2f(0, -PlayerBlob.Radius - 20));
		
		movementSpeed = 200f - (PlayerBlob.Radius / 10f);
	}

	/// <summary>
	/// Updates the movement of the player.
	/// </summary>
	private void UpdateMovement()
	{
		Vector2f targetPosition = input.GetTargetPosition(Engine.Engine.Window);
		Vector2f direction = targetPosition - PlayerBlob.Position;
		
		if (direction != new Vector2f(0, 0))
		{
			float magnitude = MathF.Sqrt((direction.X * direction.X) + (direction.Y * direction.Y));
			direction /= magnitude;

			tempPosition += direction * movementSpeed * Time.DeltaTime;
		}
		
		ClampMovement ();

		PlayerBlob.Position = tempPosition;
	}

	/// <summary>
	/// Clamps the movement of the player.
	/// </summary>
	private void ClampMovement()
	{
		Vector2f position = tempPosition;
		
		if (position.X < PlayerBlob.Radius)
			position.X = PlayerBlob.Radius;
		else if (position.X > GameConfiguration.MapWidth - PlayerBlob.Radius)
			position.X = GameConfiguration.MapWidth - PlayerBlob.Radius;

		if (position.Y < PlayerBlob.Radius)
			position.Y = PlayerBlob.Radius;
		else if (position.Y > GameConfiguration.MapWidth - PlayerBlob.Radius)
			position.Y = GameConfiguration.MapWidth - PlayerBlob.Radius;
		
		tempPosition = position;
	}

	/// <summary>
	/// Adds mass to the player. Radius is increased by the mass.
	/// </summary>
	/// <param name="mass">The amount of mass</param>
	public void AddMass(float mass)
	{
		if (PlayerBlob.Radius + mass > GameConfiguration.AbsoluteMaxRadius)
			return;
		
		PlayerBlob.Radius += mass;
		PlayerBlob.Radius = Math.Clamp(PlayerBlob.Radius, 0, GameConfiguration.AbsoluteMaxRadius);
		
		PlayerBlob.Shape.Origin = new Vector2f(PlayerBlob.Radius, PlayerBlob.Radius);
		PlayerBlob.Radius = MathF.Floor(PlayerBlob.Radius);
	}

	private void ChangeSoul()
	{
		Player oldPlayer = this;
		Player newPlayer = Core.Game.Players[Core.Game.Random.Next(0, Core.Game.Players.Count)];

		while (newPlayer == oldPlayer)
		{
			newPlayer = Core.Game.Players[Core.Game.Random.Next(0, Core.Game.Players.Count)];
		}


		(oldPlayer.PlayerBlob, newPlayer.PlayerBlob) = (newPlayer.PlayerBlob, oldPlayer.PlayerBlob);
		(oldPlayer.tempPosition, newPlayer.tempPosition) = (newPlayer.tempPosition, oldPlayer.tempPosition);
		
		newPlayer.PlayerBlob.Shape.OutlineColor = Color.Black;
	}

	/// <summary>
	/// Destroys the player. Removes it from the game.
	/// </summary>
	public new void Destroy()
	{
		if (this == LocalPlayer)
		{
			ChangeSoul ();
			Console.WriteLine("You died, but soul was changed! You are now " + LocalPlayer.PlayerBlob.NickName);
			return;
		}

		base.Destroy();
		
		Console.WriteLine($"Player {PlayerBlob.NickName} died!");

		Core.Game.Players.Remove(this);
	}

	private void BindKeys()
	{
		if (input is PlayerInput playerInput)
		{
			playerInput.BindKey(Keyboard.Key.R, ChangeSoul);
		}
	}
	
	public bool CanEat(Player player)
	{
		return PlayerBlob.Radius > player.PlayerBlob.Radius;
	}
	
	public void EatPlayer(Player player)
	{
		float mass = player.PlayerBlob.Radius;
		player.Destroy();
		AddMass(mass);
	}
}