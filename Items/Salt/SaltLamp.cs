using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Salt
{
    public class SaltLamp : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 6;
            Item.height = 14;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 1000;
            Item.createTile = Mod.Find<ModTile>("SaltLamp").Type;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(Mod, "PinkSalt", 10);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 2);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();
        }
    }
}