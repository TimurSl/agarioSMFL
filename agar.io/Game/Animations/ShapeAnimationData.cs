using SFML.Graphics;
using SFML.System;

namespace agar.io.Game.Animations;

public class ShapeAnimationData
{
	private Texture oldTexture;
	private Vector2f oldPosition;
	private float oldRotation;
	private Vector2f oldScale;
	private Color oldColor;
	private float oldAlpha;
	
	private Shape shape;
	
	public ShapeAnimationData(Shape shape)
	{
		this.shape = shape;
		oldPosition = shape.Position;
		oldRotation = shape.Rotation;
		oldScale = shape.Scale;
		oldColor = shape.FillColor;
		oldAlpha = shape.FillColor.A;
		oldTexture = shape.Texture;
	}

	public void Reset()
	{
		shape.Position = oldPosition;
		shape.Rotation = oldRotation;
		shape.Scale = oldScale;
		shape.FillColor = oldColor;
		shape.FillColor = new Color(shape.FillColor.R, shape.FillColor.G, shape.FillColor.B, (byte)oldAlpha);
		shape.Texture = oldTexture;
	}
}