using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
    public class Clentamistation : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Clentaminates items");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 20;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Pink;
            Item.value = 2000000;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = Mod.Find<ModTile>("Clentamistation").Type;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ImbuingStation);
            recipe.AddIngredient(ItemID.Clentaminator);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}