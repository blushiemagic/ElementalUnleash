using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
    public class SpitePotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("12% increased damage and critical strike chance"
                + "\nIncompatible with Wrath and Rage");
        }

        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 24;
            item.maxStack = 30;
            item.rare = 8;
            item.value = 1000;
            item.useStyle = 2;
            item.useAnimation = 17;
            item.useTime = 17;
            item.useTurn = true;
            item.UseSound = SoundID.Item3;
            item.consumable = true;
            item.buffType = mod.BuffType("Spite");
            item.buffTime = 21600;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.WrathPotion, 2);
            recipe.AddIngredient(null, "ScytheBlade");
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.RagePotion, 2);
            recipe.AddIngredient(null, "ScytheBlade");
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}