using System.Diagnostics;
using agar.io.Core;
using agar.io.Engine.Interfaces;
using SFML.Graphics;
using SFML.System;

namespace agar.io.Objects;

public class Scoreboard : BaseObject, IDrawable, IUpdatable
{
	public int ZIndex { get; set; } = 999;
	private Text text;
	
	public Scoreboard()
	{
		text = new Text("Scoreboard\n", 20, Color.White, new Vector2f(0, 0));
	}
	
	public void Draw(RenderTarget target)
	{
		if (Game.Camera != null)
		{
			text.TextClass.Position = Game.GetLeftTopCorner () + new Vector2f(70, 30) * Game.CurrentCameraZoom;
			text.TextClass.Scale = new Vector2f(1 * Game.CurrentCameraZoom, 1 * Game.CurrentCameraZoom);
		}
		
		text.Draw(target);
	}

	public void Update()
	{
		var players = Game.Players;
		players.Sort((x, y) => y.Radius.CompareTo(x.Radius));

		if (players.Count > 10)
			players = players.GetRange(0, 10);
		else
			players = players.GetRange(0, players.Count);

		text.SetMessage("Scoreboard\n");
		foreach(var player in players)
		{
			if (player == Player.LocalPlayer)
			{
				text.SetMessage(text.GetMessage () + player.NickName + " - " + player.Radius + " (You)\n");
				continue;
			}
			text.SetMessage(text.GetMessage () + player.NickName + " - " + player.Radius + "\n");
		}
	}
}