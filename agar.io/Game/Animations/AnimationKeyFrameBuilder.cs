using SFML.Graphics;

namespace agar.io.Game.Animations;

public static class AnimationKeyFrameBuilder
{
	private static AnimationKeyFrame currentKeyFrame = new AnimationKeyFrame();
	
	public static AnimationKeyFrame CreateKeyFrame(float time)
	{
		currentKeyFrame.Time = time;
		return currentKeyFrame;
	}
	
	public static AnimationKeyFrame SetPositionOffset(this AnimationKeyFrame keyFrame, SFML.System.Vector2f positionOffset)
	{
		keyFrame.PositionOffset = positionOffset;
		return keyFrame;
	}
	
	public static AnimationKeyFrame SetRotationOffset(this AnimationKeyFrame keyFrame, float rotationOffset)
	{
		keyFrame.RotationOffset = rotationOffset;
		return keyFrame;
	}
	
	public static AnimationKeyFrame SetScaleOffset(this AnimationKeyFrame keyFrame, SFML.System.Vector2f scaleOffset)
	{
		keyFrame.ScaleOffset = scaleOffset;
		return keyFrame;
	}
	
	public static AnimationKeyFrame SetColorOffset(this AnimationKeyFrame keyFrame, Color colorOffset)
	{
		keyFrame.ColorOffset = colorOffset;
		return keyFrame;
	}
	
	public static AnimationKeyFrame SetAlphaOffset(this AnimationKeyFrame keyFrame, float alphaOffset)
	{
		keyFrame.AlphaOffset = alphaOffset;
		return keyFrame;
	}
	
	public static AnimationKeyFrame SetTexture(this AnimationKeyFrame keyFrame, Texture texture)
	{
		keyFrame.Texture = texture;
		return keyFrame;
	}
	
	public static AnimationKeyFrame SetListener(this AnimationKeyFrame keyFrame, Action onKeyFrame)
	{
		keyFrame.OnAnimationKeyFrame += onKeyFrame;
		return keyFrame;
	}
}