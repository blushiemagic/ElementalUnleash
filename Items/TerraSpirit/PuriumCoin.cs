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
            // Tooltip.SetDefault("Can be crafted to and from 100 platinum coins");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 999;
            Item.value = 500000000;
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
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PlatinumCoin, 100);
            recipe.Register();

            recipe = Recipe.Create(ItemID.PlatinumCoin, 100);
            recipe.AddIngredient(this);
            recipe.Register();
        }
    }
}