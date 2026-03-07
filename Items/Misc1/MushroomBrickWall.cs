using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
    public class MushroomBrickWall : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 999;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 7;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createWall = Mod.Find<ModWall>("MushroomBrickWall").Type;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(4);
            recipe.AddIngredient(null, "MushroomBrick");
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}