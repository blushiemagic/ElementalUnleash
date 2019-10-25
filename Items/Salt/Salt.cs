using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Salt
{
    public class Salt : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Extracted from your tears of rage");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 14;
            item.maxStack = 999;
            item.rare = 8;
            item.value = 100;
        }
    }
}