# Zenisoft Game Engine on SFML

## How to use

### 1. Create a class that inherits from `BaseGame` class
```csharp
public class Game : BaseGame
```

### 2. Override `Initialize()` method if you want to initialize something
```csharp
public override void Initialize()
```

### 3. Override `OnFrameStart` or `OnFrameEnd` methods if you want to do something on frame start or end
```csharp
protected override void OnFrameStart()
protected override void OnFrameEnd()
```

### 4. Override OnWindowClosed method if you want to do something when window is closed
```csharp
public override void OnWindowClosed()
```

## Classes

- `EngineConfiguration` - Contains engine configuration
- `BaseGame` - Base class for game
- `Time` - Contains time information, such as delta time
- `Gizmos` - Contains methods for drawing gizmos
- `BaseObject` - Base class for objects, use this when creating enemy, player, etc.
- `IDrawable` - Interface for drawable objects
- `IUpdatable` - Interface for updateable objects
- `Engine` - Contains methods for engine, such as starting game, stopping game, etc.

## Examples
<details>
<summary>Game.cs</summary>

```csharp
using SFML_Animation_Practice.Engine.Types;
using SFML_Animation_Practice.Game.Objects;
using SFML.Window;

namespace SFML_Animation_Practice.Game;

public class Game : BaseGame
{
	public static Game Instance { get; private set; }
	
	private AnimatedObject animatedObject;
	
	public Game()
	{
		Instance = this;
	}

	public override void Initialize()
	{
		base.Initialize ();
		
		animatedObject = Engine.RegisterActor(new AnimatedObject ()) as AnimatedObject;
	}

	public override void Run()
	{
		base.Run ();
		animatedObject.Animation.Restart();
		Console.WriteLine("Hello World!");
	}

	protected override void OnFrameEnd()
	{
		if (Keyboard.IsKeyPressed(Keyboard.Key.E))
		{
			animatedObject.Animation.Restart ();
			animatedObject.Animation.Loop = false;
		}

		if (Keyboard.IsKeyPressed(Keyboard.Key.R))
		{
			animatedObject.Animation.Restart();
			animatedObject.Animation.Loop = true;
		}
	}
}
```

</details>

<details>
<summary>AnimatedObject.cs</summary>

```csharp
using SFML_Animation_Practice.Engine.Config;
using SFML_Animation_Practice.Engine.Interfaces;
using SFML_Animation_Practice.Engine.Types;
using SFML_Animation_Practice.Game.Animations;
using SFML_Animation_Practice.Game.Extensions;
using SFML.Graphics;
using SFML.System;

namespace SFML_Animation_Practice.Game.Objects;

public class AnimatedObject : BaseObject, IDrawable
{
	public int ZIndex { get; set; } = 1;
	public RectangleShape Shape { get; set; }
	public Animation Animation { get; set; }
	
	public AnimatedObject()
	{
		Shape = new RectangleShape(new Vector2f(100, 100));
		Shape.Position = new SFML.System.Vector2f(EngineConfiguration.WindowWidth / 2, EngineConfiguration.WindowHeight / 2);
		Shape.Origin = new SFML.System.Vector2f(Shape.Size.X / 2, Shape.Size.Y / 2);
		Shape.Scale = new SFML.System.Vector2f(1, 1);

		Animation = new(Shape);
		Animation.Loop = false;
		Animation.ResetOnStart = true;
		Animation.OnAnimationEnd += () =>
		{
			// do something
		};

		string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory (), "AnimationImages"));
		files = files.Where(x => x.EndsWith(".png") || x.EndsWith(".jpg") || x.EndsWith(".jpeg")).ToArray();

		for (var i = 0; i < files.Length; i++)
		{
			Texture texture = new Texture(files[i]);
			texture = texture.RemoveColor(new Color(0, 255, 255));

			AnimationKeyFrame keyFrame = AnimationKeyFrameBuilder
				.CreateKeyFrame(i * 0.05f)
				.SetTexture(texture)
				.SetScaleOffset(new Vector2f(0.5f, 0.5f));

			Animation.AddKeyFrame(keyFrame);
		}
		
		AnimationKeyFrame keyFrame2 = AnimationKeyFrameBuilder
			.CreateKeyFrame(0.0f)
			.SetListener(() =>
			{
				// you can put some code here
			});
		
		Animation.AddKeyFrame(keyFrame2);
		
		Game.Instance.RegisterUpdatable(Animation);
	}
	
	public void Draw(RenderTarget target)
	{
		target.Draw(Shape);
	}
}
```

</details>