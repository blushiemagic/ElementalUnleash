using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium
{
    public class PuriumAnvil : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Used to craft items from elemental bars");
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 14;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 12, 0, 0);
            Item.createTile = Mod.Find<ModTile>("PuriumAnvil").Type;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "PuriumBar", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}