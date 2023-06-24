using SFML.Graphics;
using SFML.System;
using ZenisoftGameEngine.Interfaces;
using ZenisoftGameEngine.Sound;
using ZenisoftGameEngine.Types;
using Time = SFML.System.Time;

namespace agar.io.Game.Objects
{
    public class Scoreboard : BaseObject, IDrawable, IUpdatable
    {
        public int ZIndex { get; set; } = 999;
        private Text text;
        private Clock updateClock;
        private float updateInterval = 1f / 10f; 
        
        private bool previousFirstPlaceIsLocalPlayer = false;

        public Scoreboard()
        {
            text = new Text("Scoreboard\n", 20, Color.White, new Vector2f(0, 0));
            updateClock = new Clock();
        }

        public void Draw(RenderTarget target)
        {
            if (Core.Game.Camera != null)
            {
                text.TextClass.Position = Core.Game.GetLeftTopCorner() + new Vector2f(70, 30) * Core.Game.CurrentCameraZoom;
                text.TextClass.Scale = new Vector2f(1 * Core.Game.CurrentCameraZoom, 1 * Core.Game.CurrentCameraZoom);
            }
            
            text.Draw(target);
        }

        public void Update()
        {
            // Check if enough time has passed since the last update
            if (updateClock.ElapsedTime.AsSeconds() >= updateInterval)
            {
                var players = Core.Game.Players;
                players.Sort((x, y) => y.PlayerBlob.Radius.CompareTo(x.PlayerBlob.Radius));

                if (players.Count > 10)
                    players = players.GetRange(0, 10);
                else
                    players = players.GetRange(0, players.Count);

                string newText = "Scoreboard\n";

                foreach (var player in players)
                {
                    if (player == Player.LocalPlayer)
                    {
                        newText += player.PlayerBlob.NickName + " - " + player.PlayerBlob.Radius + " (You)\n";
                        continue;
                    }
                    newText += player.PlayerBlob.NickName + " - " + player.PlayerBlob.Radius + "\n";
                }

                if (!players.Contains(Player.LocalPlayer))
                    newText += Player.LocalPlayer.PlayerBlob.NickName + " - " + Player.LocalPlayer.PlayerBlob.Radius + " (You)\n";
                
                text.SetMessage(newText);

                int localPlayerPosition = players.IndexOf(Player.LocalPlayer);

                CheckLocalPlayerPosition(localPlayerPosition);
                
                updateClock.Restart();
            }
        }

        private void CheckLocalPlayerPosition(int localPlayerPosition)
        {
            if (localPlayerPosition == 0 && !previousFirstPlaceIsLocalPlayer)
            {
                AudioPlayer.PlayAudioClip("levelup");
                previousFirstPlaceIsLocalPlayer = true;
            }
            else if (localPlayerPosition > 0)
            {
                previousFirstPlaceIsLocalPlayer = false;
            }
        }
    }
}
