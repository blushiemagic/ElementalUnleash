using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
    public class SolarDrop : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Burning with sunlight");
        }

        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Orange;
            Item.value = 1000;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight((int)((Item.position.X + Item.width * 0.5f) / 16f), (int)((Item.position.Y + Item.height * 0.5f) / 16f), 0.5f, 0.5f, 0f);
        }
    }
}