using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
    public class HorrorDrop : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Red as blood");
        }

        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Orange;
            Item.value = 1000;
        }
    }
}