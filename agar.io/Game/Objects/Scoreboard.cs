using agar.io.Engine.Interfaces;
using agar.io.Engine.Types;
using SFML.Graphics;
using SFML.System;

namespace agar.io.Game.Objects;

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
		if (Core.Game.Camera != null)
		{
			text.TextClass.Position = Core.Game.GetLeftTopCorner () + new Vector2f(70, 30) * Core.Game.CurrentCameraZoom;
			text.TextClass.Scale = new Vector2f(1 * Core.Game.CurrentCameraZoom, 1 * Core.Game.CurrentCameraZoom);
		}
		
		text.Draw(target);
	}

	public void Update()
	{
		var players = Core.Game.Players;
		players.Sort((x, y) => y.PlayerBlob.Radius.CompareTo(x.PlayerBlob.Radius));

		if (players.Count > 10)
			players = players.GetRange(0, 10);
		else
			players = players.GetRange(0, players.Count);

		text.SetMessage("Scoreboard\n");
		foreach(var player in players)
		{
			if (player == Player.LocalPlayer)
			{
				text.SetMessage(text.GetMessage () + player.PlayerBlob.NickName + " - " + player.PlayerBlob.Radius + " (You)\n");
				continue;
			}
			text.SetMessage(text.GetMessage () + player.PlayerBlob.NickName + " - " + player.PlayerBlob.Radius + "\n");
		}
		
		if (!players.Contains(Player.LocalPlayer))
			text.SetMessage(text.GetMessage () + Player.LocalPlayer.PlayerBlob.NickName + " - " + Player.LocalPlayer.PlayerBlob.Radius + " (You)\n");
	}
}