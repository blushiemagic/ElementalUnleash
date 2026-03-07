using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace Bluemagic.PuritySpirit
{
    public class PuritySpiritScreenShaderData : ScreenShaderData
    {
        private int puritySpiritIndex;

        public PuritySpiritScreenShaderData(string passName)
            : base(passName)
        {
        }

        private void UpdatePuritySpiritIndex()
        {
            int puritySpiritType = ModLoader.GetMod("Bluemagic").Find<ModNPC>("PuritySpirit").Type;
            if (puritySpiritIndex >= 0 && Main.npc[puritySpiritIndex].active && Main.npc[puritySpiritIndex].type == puritySpiritType)
            {
                return;
            }
            puritySpiritIndex = -1;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == puritySpiritType)
                {
                    puritySpiritIndex = i;
                    break;
                }
            }
        }

        public override void Apply()
        {
            UpdatePuritySpiritIndex();
            if (puritySpiritIndex != -1)
            {
                UseTargetPosition(Main.npc[puritySpiritIndex].Center);
            }
            base.Apply();
        }
    }
}