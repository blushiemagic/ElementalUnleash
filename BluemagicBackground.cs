using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace Bluemagic
{
    public class BluemagicBackground : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            if (BlushieBoss.BlushieBoss.Players[Main.myPlayer] && BlushieBoss.BlushieBoss.CameraFocus)
            {
                float zoom = Main.screenHeight / (2f * BlushieBoss.BlushieBoss.ArenaSize + 320f);
                if (zoom < Main.GameViewMatrix.Zoom.Y)
                {
                    Main.GameViewMatrix.Zoom = new Vector2(zoom);
                }
                return true;
            }
            return false;
        }
    }

    public class PhaseThree : ModSceneEffect
    {

        public override bool IsSceneEffectActive(Player player)
        {
            return !Main.gameMenu && BlushieBoss.BlushieBoss.Active && BlushieBoss.BlushieBoss.Phase >= 3;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Fallen Blood");
    }
}