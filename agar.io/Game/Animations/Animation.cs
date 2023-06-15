using SFML.Graphics;
using ZenisoftGameEngine.Interfaces;
using ZenisoftGameEngine.Types;

namespace agar.io.Game.Animations;

public class Animation : IUpdatable
{
	public bool Loop { get; set; } = true;
	public bool ResetOnStart { get; set; } = true;
	public float AnimationSpeedMultiplier { get; set; } = 1f;
	public Action OnAnimationEnd { get; set; }
	public List<AnimationKeyFrame> KeyFrames { get; set; }


	private Shape shape;
	private int currentKeyFrameIndex = 0;
	private float currentKeyFrameTime = 0f;

	private ShapeAnimationData oldState;
	private bool canPlayed = false;

	public Animation(Shape shape, bool playOnStart = false)
	{
		this.shape = shape;
		KeyFrames = new List<AnimationKeyFrame>();
		
		oldState = new ShapeAnimationData(shape);
		
		agar.io.Game.Core.Game.Instance.RegisterUpdatable(this);
		canPlayed = playOnStart;
	}
	
	/// <summary>
	/// Dont call this method directly if you have ZenisoftGameEngine, register to Engine Update, if dont, do all you want.
	/// </summary>
	public void Update()
	{
		if (!canPlayed)
			return;
		
		if (currentKeyFrameIndex < KeyFrames.Count)
		{
			UpdateFrames ();
		}
		else
		{
			OnAnimationEnd?.Invoke();
			if (Loop)
			{
				if (ResetOnStart)
					Reset ();

				Start ();
			}
		}
	}

	private void UpdateFrames()
	{
		float deltaTime = Time.DeltaTime;
		currentKeyFrameTime += deltaTime * AnimationSpeedMultiplier;

		if (currentKeyFrameTime >= KeyFrames[currentKeyFrameIndex].Time)
		{
			currentKeyFrameIndex++;
			if (currentKeyFrameIndex < KeyFrames.Count)
			{
				ApplyKeyFrameParameters(KeyFrames[currentKeyFrameIndex]);
			}
		}
	}

	/// <summary>
	/// Starts the animation. 
	/// </summary>
	public void Start()
	{
		if (!canPlayed)
		{
			canPlayed = true;
		}
		if (ResetOnStart)
		{
			Reset ();
		}
		
		currentKeyFrameIndex = 0;
		currentKeyFrameTime = 0f;

		if (KeyFrames.Count > 0)
		{
			ApplyKeyFrameParameters(KeyFrames[currentKeyFrameIndex]);
		}
	}

	
	/// <summary>
	/// Resets the shape to its original state. Can be used to reset the shape after an animation has ended (ResetOnStart doing this).
	/// </summary>
	public void Reset()
	{
		oldState.Reset();
	}
	
	private void ApplyKeyFrameParameters(AnimationKeyFrame keyFrame)
	{
		shape.Position += keyFrame.PositionOffset;
		shape.Rotation += keyFrame.RotationOffset;
		shape.Scale += keyFrame.ScaleOffset;
		
		Color newColor = shape.FillColor + keyFrame.ColorOffset;
		shape.FillColor = newColor;
		shape.FillColor = new Color(shape.FillColor.R, shape.FillColor.G, shape.FillColor.B, (byte)(shape.FillColor.A + keyFrame.AlphaOffset));
		
		shape.Texture = keyFrame.Texture;


		keyFrame.OnAnimationKeyFrame?.Invoke();
	}

	public void Stop()
	{
		canPlayed = false;
	}
}
