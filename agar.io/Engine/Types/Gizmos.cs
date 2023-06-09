using agar.io.Engine.Config;
using SFML.Graphics;
using SFML.System;

namespace agar.io.Engine.Types;

public static class Gizmos
{
	public static void DrawLine(Vector2f start, Vector2f end, Color color)
	{
		if (EngineConfiguration.DebugMode == false)
			return;
		
		VertexArray line = new VertexArray(PrimitiveType.Lines, 2);
		line[0] = new Vertex(start, color);
		line[1] = new Vertex(end, color);

		Engine.Window.Draw(line);
	}
	
	public static void DrawBox(Vector2f position, Vector2f size, Color color)
	{
		if (EngineConfiguration.DebugMode == false)
			return;
		
		VertexArray box = new VertexArray(PrimitiveType.Quads, 4);
		
		box[0] = new Vertex(new Vector2f(position.X - size.X / 2, position.Y - size.Y / 2), color);
		box[1] = new Vertex(new Vector2f(position.X + size.X / 2, position.Y - size.Y / 2), color);
		box[2] = new Vertex(new Vector2f(position.X + size.X / 2, position.Y + size.Y / 2), color);
		box[3] = new Vertex(new Vector2f(position.X - size.X / 2, position.Y + size.Y / 2), color);

		Engine.Window.Draw(box);
	}
	
	public static void DrawWireBox(Vector2f position, Vector2f size, Color color)
	{
		if (EngineConfiguration.DebugMode == false)
			return;
		
		VertexArray box = new VertexArray(PrimitiveType.LineStrip, 5);
		
		box[0] = new Vertex(new Vector2f(position.X - size.X / 2, position.Y - size.Y / 2), color);
		box[1] = new Vertex(new Vector2f(position.X + size.X / 2, position.Y - size.Y / 2), color);
		box[2] = new Vertex(new Vector2f(position.X + size.X / 2, position.Y + size.Y / 2), color);
		box[3] = new Vertex(new Vector2f(position.X - size.X / 2, position.Y + size.Y / 2), color);
		box[4] = new Vertex(new Vector2f(position.X - size.X / 2, position.Y - size.Y / 2), color);

		Engine.Window.Draw(box);
	}
	
	public static void DrawWireCircle(Vector2f position, float radius, Color color)
	{
		if (EngineConfiguration.DebugMode == false)
			return;
		
		VertexArray circle = new VertexArray(PrimitiveType.LineStrip, 100);
		for (uint i = 0; i < 100; i++)
		{
			float angle = (float) (i * 2 * Math.PI / 100);
			circle[i] = new Vertex(new Vector2f(position.X + (float) Math.Cos(angle) * radius, position.Y + (float) Math.Sin(angle) * radius), color);
		}

		Engine.Window.Draw(circle);
	}

	public static void DrawRect(Vector2f position, FloatRect rect, Color color)
	{
		if (EngineConfiguration.DebugMode == false)
			return;
		
		VertexArray box = new VertexArray(PrimitiveType.Quads, 4);
		
		Vector2f center = new Vector2f(position.X + rect.Left + rect.Width / 2, position.Y + rect.Top + rect.Height / 2);
		
		box[0] = new Vertex(new Vector2f(center.X - rect.Width / 2, center.Y - rect.Height / 2), color);
		box[1] = new Vertex(new Vector2f(center.X + rect.Width / 2, center.Y - rect.Height / 2), color);
		box[2] = new Vertex(new Vector2f(center.X + rect.Width / 2, center.Y + rect.Height / 2), color);
		box[3] = new Vertex(new Vector2f(center.X - rect.Width / 2, center.Y + rect.Height / 2), color);

		Engine.Window.Draw(box);
	}
	
	public static void DrawWireRect(Vector2f position, FloatRect rect, Color color)
	{
		if (EngineConfiguration.DebugMode == false)
			return;
		
		VertexArray box = new VertexArray(PrimitiveType.LineStrip, 5);
		
		Vector2f center = new Vector2f(position.X + rect.Left + rect.Width / 2, position.Y + rect.Top + rect.Height / 2);
		
		box[0] = new Vertex(new Vector2f(center.X - rect.Width / 2, center.Y - rect.Height / 2), color);
		box[1] = new Vertex(new Vector2f(center.X + rect.Width / 2, center.Y - rect.Height / 2), color);
		box[2] = new Vertex(new Vector2f(center.X + rect.Width / 2, center.Y + rect.Height / 2), color);
		box[3] = new Vertex(new Vector2f(center.X - rect.Width / 2, center.Y + rect.Height / 2), color);
		box[4] = new Vertex(new Vector2f(center.X - rect.Width / 2, center.Y - rect.Height / 2), color);

		Engine.Window.Draw(box);
	}
}