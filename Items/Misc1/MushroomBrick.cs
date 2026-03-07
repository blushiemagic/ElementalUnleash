using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
    public class MushroomBrick : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 999;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = Mod.Find<ModTile>("MushroomBrick").Type;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "Shroomstone", 2);
            recipe.AddTile(TileID.Furnaces);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(null, "MushroomBrickWall", 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}