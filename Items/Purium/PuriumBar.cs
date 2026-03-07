using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium
{
    public class PuriumBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("'Flowing with power'");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 1, 20, 0);
            Item.createTile = Mod.Find<ModTile>("ElementalBar").Type;
            Item.placeStyle = 0;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "PuriumOre", 6);
            recipe.AddTile(null, "PuriumForge");
            recipe.Register();
        }
    }
}