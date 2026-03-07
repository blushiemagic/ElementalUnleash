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
            // Tooltip.SetDefault("Extracted from your tears of rage");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 14;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 100;
        }
    }
}