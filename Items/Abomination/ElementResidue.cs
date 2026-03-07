using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination
{
    public class ElementResidue : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Residual Elements");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(0, 2, 50, 0);
        }
    }
}