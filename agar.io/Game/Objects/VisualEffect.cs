using agar.io.Game.Animations;
using SFML_Animation_Practice.Game.Extensions;
using SFML.Graphics;
using SFML.System;
using ZenisoftGameEngine.Interfaces;
using ZenisoftGameEngine.Types;

namespace agar.io.Game.Objects;

public class VisualEffect : BaseObject, IDrawable
{
	public int ZIndex { get; set; } = 15;
	public RectangleShape Shape { get; set; }

	public Animation Animation { get; set; }


	public VisualEffect(Vector2f position, string[] files)
	{
		Shape = new RectangleShape(new Vector2f(100, 100));
		Shape.Position = position;
		Shape.Origin = new Vector2f(Shape.Size.X / 2, Shape.Size.Y / 2);
		
		Animation = new Animation(Shape, false);
		
		for (var i = 0; i < files.Length; i++)
		{
			Texture texture = new Texture(files[i]);
			texture.RemoveColor(new Color(0, 255, 255));
			
			AnimationKeyFrame keyFrame = AnimationKeyFrameBuilder.CreateKeyFrame(i * 0.08f).SetTexture(texture);
			Animation.KeyFrames.Add(keyFrame);
		}
	}

	public void Draw(RenderTarget target)
	{
		target.Draw(Shape);
	}
}