using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.TerraSpirit
{
    public class PuriumCoin : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Can be crafted to and from 100 platinum coins");
        }

        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 14;
            item.maxStack = 999;
            item.value = 500000000;
            /*item.ammo = AmmoID.Coin;
            item.notAmmo = true;
            item.damage = 200;
            item.shoot = 161;
            item.shootSpeed = 4f;
            item.ranged = true;
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.consumable = true;
            item.createTile = 333;
            item.noMelee = true;*/
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PlatinumCoin, 100);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(this);
            recipe.SetResult(ItemID.PlatinumCoin, 100);
            recipe.AddRecipe();
        }
    }
}