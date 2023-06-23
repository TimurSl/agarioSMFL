﻿using agar.io.Game.Animations;
using agar.io.Game.Core.Types;
using agar.io.Game.Input;
using agar.io.Game.Input.Interfaces;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using ZenisoftGameEngine;
using ZenisoftGameEngine.Interfaces;
using ZenisoftGameEngine.Sound;
using ZenisoftGameEngine.Types;

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
	
	private VisualEffect explosionEffect;
	private VisualEffect portalEffect;
	

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
		
		portalEffect = new VisualEffect(PlayerBlob.Position, Directory.GetFiles(
			Path.Combine(Directory.GetCurrentDirectory (), "Game", "Animations", "Clips", "Portal"), "*.png"));
		portalEffect.Animation.Loop = false;
		
		explosionEffect = new VisualEffect(tempPosition,
			Directory.GetFiles(
				Path.Combine(Directory.GetCurrentDirectory (), "Game", "Animations", "Clips", "Explosion"), "*.png"));

	}

	public void Draw(RenderTarget target)
	{
		target.Draw(PlayerBlob.Shape);
		PlayerBlob.NickNameLabel.Draw(target);
	}


	public void Update()
	{
		input.HandleInput(Engine.Window);
		
		UpdateOutline ();
		
		UpdateMovement();

		PlayerBlob.NickNameLabel.SetPosition(PlayerBlob.Position + new Vector2f(0, -PlayerBlob.Radius - 20));
		
		movementSpeed = GameConfiguration.MovementSpeed - (PlayerBlob.Radius);
		if (movementSpeed < GameConfiguration.MinimumMovementSpeed)
		{
			movementSpeed = GameConfiguration.MinimumMovementSpeed;
		}

		UpdateCheats ();

		LocalPlayer.ZIndex = 9;
		LocalPlayer.PlayerBlob.NickNameLabel.ZIndex = 10;
	}

	private void UpdateCheats()
	{
		if (GameConfiguration.EnableCheats)
		{
			foreach(Player player in Game.Core.Game.Players)
			{
				if (player != LocalPlayer)
				{
					if (LocalPlayer.CanEat(player))
					{
						Gizmos.DrawLine(LocalPlayer.PlayerBlob.Position, player.PlayerBlob.Position, 
							new Color(0, 255, 0, 255));
					}
				}
			}
		}
	}

	private void UpdateOutline()
	{
		if (this != LocalPlayer)
		{
			PlayerBlob.Shape.OutlineThickness = 3;

			if (PlayerBlob.Radius > LocalPlayer.PlayerBlob.Radius)
			{
				PlayerBlob.Shape.OutlineColor = UIConfiguration.darkRed;
			}
			else if (PlayerBlob.Radius < LocalPlayer.PlayerBlob.Radius)
			{
				PlayerBlob.Shape.OutlineColor = UIConfiguration.darkGreen;
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
	}

	/// <summary>
	/// Updates the movement of the player.
	/// </summary>
	private void UpdateMovement()
	{
		Vector2f targetPosition = input.GetTargetPosition(Engine.Window);
		Vector2f direction = targetPosition - PlayerBlob.Position;
		
		if (direction != new Vector2f(0, 0))
		{
			float magnitude = MathF.Sqrt((direction.X * direction.X) + (direction.Y * direction.Y));
			direction /= magnitude;

			tempPosition += direction * movementSpeed * ZenisoftGameEngine.Types.Time.DeltaTime;
		}
		
		ClampMovement ();

		PlayerBlob.Position = tempPosition;
		Gizmos.DrawLine(PlayerBlob.Position, targetPosition, Color.Red);
		portalEffect.Shape.Position = PlayerBlob.Position;
		portalEffect.Shape.Size = new Vector2f(PlayerBlob.Radius * 2.1f, PlayerBlob.Radius * 2.1f);
		portalEffect.Shape.Origin = new Vector2f(portalEffect.Shape.Size.X / 2, portalEffect.Shape.Size.Y / 2);
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

	private void ChangeSoul()
	{
		AudioPlayer.PlayAudioClip("soul_swap");

		Core.Game.Instance.RegisterActor(portalEffect);

		AnimationKeyFrame animationKeyFrame = portalEffect.Animation.KeyFrames[^7];
		animationKeyFrame.OnAnimationKeyFrame += () => { SoulSwap (); };
		
		portalEffect.Animation.KeyFrames[^7] = animationKeyFrame;

		portalEffect.Animation.OnAnimationEnd = () =>
		{
			portalEffect.Animation.Stop ();
		};
		
		portalEffect.Animation.AnimationSpeedMultiplier = 0.5f;
		portalEffect.Animation.Start();
	}

	private void SoulSwap()
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
		Core.Game.Instance.RegisterActor(explosionEffect);

		explosionEffect.Animation.OnAnimationEnd += () =>
		{
			explosionEffect.Destroy();
		};
		
		explosionEffect.Shape.Size = new Vector2f(PlayerBlob.Shape.Radius * 3, PlayerBlob.Shape.Radius * 3);
		
		explosionEffect.Animation.Start();
		
		if (this == LocalPlayer)
		{
			SoulSwap ();
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
			playerInput.BindKey(Keyboard.Key.P, RandomSpectate);
			playerInput.BindKey(Keyboard.Key.O, ReturnSpecate);
			playerInput.BindKey(Keyboard.Key.T, () => { PlayerBlob.AddMass(100); });
		}
	}
	
	public bool CanEat(Player player)
	{
		return PlayerBlob.Radius > player.PlayerBlob.Radius;
	}
	
	public bool IsEqual(Player player)
	{
		return Math.Abs(PlayerBlob.Radius - player.PlayerBlob.Radius) < 0.1f;
	}
	
	public void EatPlayer(Player player)
	{
		float mass = player.PlayerBlob.Radius / 2f;
		player.Destroy();
		PlayerBlob.AddMass(mass);
	}
	
	public bool CollidesWithPlayer(Player player)
	{
		float distance = player.PlayerBlob.Position.Distance(PlayerBlob.Position);
		float radius = player.PlayerBlob.Radius + PlayerBlob.Radius;
		return distance <= radius;
	}

	private void RandomSpectate()
	{
		Player player = Core.Game.Players[Core.Game.Random.Next(0, Core.Game.Players.Count)];
		
		while (player == this)
		{
			player = Core.Game.Players[Core.Game.Random.Next(0, Core.Game.Players.Count)];
		}
		
		LocalPlayer = player;
	}

	private void ReturnSpecate()
	{
		foreach (Player player in Core.Game.Players)
		{
			if (player.IsPlayer)
			{
				LocalPlayer = player;
				return;
			}
		}
	}
	
	
}