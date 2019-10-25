using System;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
    public class LunarDrop : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Glowing with moonlight");
        }

        public override void SetDefaults()
        {
            item.width = 8;
            item.height = 8;
            item.maxStack = 999;
            item.rare = 3;
            item.value = 1000;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight((int)((item.position.X + item.width * 0.5f) / 16f), (int)((item.position.Y + item.height * 0.5f) / 16f), 0.2f, 0.2f, 0.4f);
        }
    }
}