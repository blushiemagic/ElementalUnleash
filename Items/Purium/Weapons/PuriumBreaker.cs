using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons
{
    public class PuriumBreaker : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.scale = 1.3f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 26;
            Item.useTime = 38;
            Item.damage = 943;
            Item.knockBack = 7f;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.rare = ItemRarityID.Purple;
            Item.DamageType = DamageClass.Melee;
            Item.value = Item.sellPrice(0, 12, 0, 0);
            Item.UseSound = SoundID.Item1;
            Item.shoot = Mod.Find<ModProjectile>("PuriumBoom").Type;
            Item.shootSpeed = 8f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float speed = (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
            float time = 0f;
            if (speed != 0f)
            {
                float gotoY = Main.screenPosition.Y + (float)Main.mouseY;
                if (player.gravDir == -1)
                {
                    gotoY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
                }
                float distanceX = Main.screenPosition.X + (float)Main.mouseX - position.X;
                float distanceY = gotoY - position.Y;
                time = (float)Math.Sqrt(distanceX * distanceX + distanceY * distanceY) / speed;
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, time, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "PuriumBar", 12);
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}