using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium
{
    public class PuriumOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("'Flowing with power'");
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.createTile = Mod.Find<ModTile>("PuriumOre").Type;
        }
    }
}