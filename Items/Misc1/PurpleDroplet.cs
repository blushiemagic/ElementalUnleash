using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
    public class PurpleDroplet : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Used by the Clentamistation"
                + "\nImbues the Corruption");*/
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
            recipe.AddIngredient(ItemID.PurpleSolution);
            recipe.Register();

            recipe = Recipe.Create(ItemID.PurpleSolution);
            recipe.AddIngredient(this, 100);
            recipe.Register();
        }
    }
}