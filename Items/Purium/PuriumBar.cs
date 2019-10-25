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
            Tooltip.SetDefault("'Flowing with power'");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.rare = 11;
            item.value = Item.sellPrice(0, 1, 20, 0);
            item.createTile = mod.TileType("ElementalBar");
            item.placeStyle = 0;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PuriumOre", 6);
            recipe.AddTile(null, "PuriumForge");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}