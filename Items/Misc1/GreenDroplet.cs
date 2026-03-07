using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
    public class GreenDroplet : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Used by the Clentamistation"
                + "\nImbues the Purity");*/
        }

        public override void SetDefaults()
        {
            Item.width = 4;
            Item.height = 4;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Orange;
            Item.value = 25;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(100);
            recipe.AddIngredient(ItemID.GreenSolution);
            recipe.Register();

            recipe = Recipe.Create(ItemID.GreenSolution);
            recipe.AddIngredient(this, 100);
            recipe.Register();
        }
    }
}