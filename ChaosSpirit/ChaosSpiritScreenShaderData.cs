using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace Bluemagic.ChaosSpirit
{
    public class ChaosSpiritScreenShaderData : ScreenShaderData
    {
        private int chaosSpiritIndex;

        public ChaosSpiritScreenShaderData(string passName)
            : base(passName)
        {
        }

        private void UpdateChaosSpiritIndex()
        {
            int chaosSpiritType = ModLoader.GetMod("Bluemagic").Find<ModNPC>("ChaosSpirit").Type;
            int chaosSpiritType2 = ModLoader.GetMod("Bluemagic").Find<ModNPC>("ChaosSpirit2").Type;
            if (chaosSpiritIndex >= 0 && Main.npc[chaosSpiritIndex].active && (Main.npc[chaosSpiritIndex].type == chaosSpiritType || Main.npc[chaosSpiritIndex].type == chaosSpiritType2))
            {
                return;
            }
            chaosSpiritIndex = -1;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].active && (Main.npc[i].type == chaosSpiritType || Main.npc[i].type == chaosSpiritType2))
                {
                    chaosSpiritIndex = i;
                    break;
                }
            }
        }

        public override void Apply()
        {
            UpdateChaosSpiritIndex();
            if (chaosSpiritIndex != -1)
            {
                UseTargetPosition(Main.npc[chaosSpiritIndex].Center);
            }
            base.Apply();
        }
    }
}