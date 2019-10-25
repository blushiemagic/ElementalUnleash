using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic
{
    public class BluemagicBackground : GlobalBgStyle
    {
        public override void ChooseSurfaceBgStyle(ref int style)
        {
            if (BlushieBoss.BlushieBoss.Players[Main.myPlayer] && BlushieBoss.BlushieBoss.CameraFocus)
            {
                float zoom = Main.screenHeight / (2f * BlushieBoss.BlushieBoss.ArenaSize + 320f);
                if (zoom < Main.GameViewMatrix.Zoom.Y)
                {
                    Main.GameViewMatrix.Zoom = new Vector2(zoom);
                }
            }
        }
    }
}