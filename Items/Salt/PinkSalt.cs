using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Salt
{
    public class PinkSalt : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Pink and sweet, but still filled with your rage");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 14;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 100;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(5);
            recipe.AddIngredient(Mod, "Salt", 4);
            recipe.AddIngredient(ItemID.PixieDust);
            recipe.AddTile(TileID.CrystalBall);
        }
    }
}