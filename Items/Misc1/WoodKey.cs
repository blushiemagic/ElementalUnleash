using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
    public class WoodKey : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("A key to a different dimension");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 20;
            Item.maxStack = 99;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Wood, 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}