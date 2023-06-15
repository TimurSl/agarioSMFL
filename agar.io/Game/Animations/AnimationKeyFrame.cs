using SFML.Graphics;

namespace agar.io.Game.Animations;

public struct AnimationKeyFrame
{
	public float Time { get; set; }
	
	public SFML.System.Vector2f PositionOffset { get; set; }
	public float RotationOffset { get; set; }
	public SFML.System.Vector2f ScaleOffset { get; set; }
	public Color ColorOffset { get; set; }
	public float AlphaOffset { get; set; }
	public Texture Texture { get; set; }
	public IntRect TextureRect { get; set; }
	public Action OnAnimationKeyFrame { get; set; }
}