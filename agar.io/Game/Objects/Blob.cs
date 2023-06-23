using agar.io.Game.Animations;
using agar.io.Game.Core.Types;
using SFML_Animation_Practice.Game.Extensions;
using SFML.Graphics;
using SFML.System;

namespace agar.io.Game.Objects;

public class Blob
{
	public CircleShape Shape;

	public readonly Text NickNameLabel;
	public readonly string NickName = "Player";
	
	public Animation Animation;

	public float Radius
	{
		get => Shape.Radius;
		set => Shape.Radius = value;
	}

	public Vector2f Position
	{
		get => Shape.Position;
		set => Shape.Position = value;
	}
	
	public Blob(string nickName)
	{
		Shape = new CircleShape(GameConfiguration.DefaultPlayerRadius);
		Shape.Position = Core.Game.RandomMapPosition ();
		Shape.FillColor = new Color((byte)Core.Game.Random.Next(0, 255), (byte)Core.Game.Random.Next(0, 255), (byte)Core.Game.Random.Next(0, 255));
		Shape.Origin = new Vector2f(GameConfiguration.DefaultPlayerRadius, GameConfiguration.DefaultPlayerRadius);
		Shape.OutlineThickness = 3;
		Shape.OutlineColor = Color.Black;

		Animation = new Animation(Shape, true);
		Animation.Loop = true;
		
		string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory (), "Game", "Animations", "Clips", "Blob"), "*.png");
		files = files.OrderBy(x => Core.Game.Random.Next()).ToArray();
		
		for (var i = 0; i < files.Length; i++)
		{
			Texture texture = new Texture(files[i]);
			AnimationKeyFrame keyFrame =
				AnimationKeyFrameBuilder.CreateKeyFrame(i * 0.8f).SetTexture(texture);
			
			Animation.KeyFrames.Add(keyFrame);
		}
		
		NickName = nickName;
		NickNameLabel = new Text(NickName, 20, Color.White, new Vector2f(0, 0));
	}
	public void AddMass(float mass)
	{
		if (Radius + mass > GameConfiguration.AbsoluteMaxRadius)
		{
			int chance = mass > 70 ? 70 : (int) mass;
			if (Core.Game.Lucky(chance))
			{
				Radius = 30;
				Radius = Math.Clamp(Radius, 0, GameConfiguration.AbsoluteMaxRadius);
				Shape.Origin = new Vector2f(Radius, Radius);
				Radius = MathF.Floor(Radius);
			}
			return;
		}
		
		Radius += mass;
		Radius = Math.Clamp(Radius, 0, GameConfiguration.AbsoluteMaxRadius);

		Shape.Origin = new Vector2f(Radius, Radius);
		Radius = MathF.Floor(Radius);
	}

}