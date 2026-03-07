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
            // Tooltip.SetDefault("Bounces at high speeds");
        }

        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 999;
            Item.damage = 20;
            Item.knockBack = 4f;
            Item.consumable = true;
            Item.ammo = AmmoID.Bullet;
            Item.rare = ItemRarityID.Purple;
            Item.DamageType = DamageClass.Ranged;
            Item.value = Item.sellPrice(0, 0, 0, 25);
            Item.shoot = Mod.Find<ModProjectile>("PuriumBullet").Type;
            Item.shootSpeed = 2f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(111);
            recipe.AddIngredient(ItemID.MusketBall, 111);
            recipe.AddIngredient(null, "PuriumBar");
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}