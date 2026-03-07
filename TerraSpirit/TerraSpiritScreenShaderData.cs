using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace Bluemagic.TerraSpirit
{
    public class TerraSpiritScreenShaderData : ScreenShaderData
    {
        private int terraSpiritIndex;

        public TerraSpiritScreenShaderData(string passName)
            : base(passName)
        {
        }

        private void UpdateTerraSpiritIndex()
        {
            int terraSpiritType = ModLoader.GetMod("Bluemagic").Find<ModNPC>("TerraSpirit").Type;
            if (terraSpiritIndex >= 0 && Main.npc[terraSpiritIndex].active && Main.npc[terraSpiritIndex].type == terraSpiritType)
            {
                return;
            }
            terraSpiritIndex = -1;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == terraSpiritType)
                {
                    terraSpiritIndex = i;
                    break;
                }
            }
        }

        public override void Apply()
        {
            UpdateTerraSpiritIndex();
            if (terraSpiritIndex != -1)
            {
                UseTargetPosition(Main.npc[terraSpiritIndex].Center);
            }
            base.Apply();
        }
    }
}