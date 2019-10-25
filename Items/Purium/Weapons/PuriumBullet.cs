using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons
{
    public class PuriumBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Bounces at high speeds");
        }

        public override void SetDefaults()
        {
            item.width = 8;
            item.height = 8;
            item.maxStack = 999;
            item.damage = 20;
            item.knockBack = 4f;
            item.consumable = true;
            item.ammo = AmmoID.Bullet;
            item.rare = 11;
            item.ranged = true;
            item.value = Item.sellPrice(0, 0, 0, 25);
            item.shoot = mod.ProjectileType("PuriumBullet");
            item.shootSpeed = 2f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MusketBall, 111);
            recipe.AddIngredient(null, "PuriumBar");
            recipe.AddTile(null, "PuriumAnvil");
            recipe.SetResult(this, 111);
            recipe.AddRecipe();
        }
    }
}