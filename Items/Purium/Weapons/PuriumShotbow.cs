using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons
{
    public class PuriumShotbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("50% chance not to consume ammo");
        }

        public override void SetDefaults()
        {
            item.width = 50;
            item.height = 18;
            item.useStyle = 5;
            item.useAnimation = 18;
            item.useTime = 18;
            item.noMelee = true;
            item.damage = 147;
            item.knockBack = 3f;
            item.autoReuse = true;
            item.useTurn = false;
            item.rare = 11;
            item.ranged = true;
            item.value = Item.sellPrice(0, 12, 0, 0);
            item.UseSound = SoundID.Item5;
            item.shoot = ProjectileID.WoodenArrowFriendly;
            item.useAmmo = AmmoID.Arrow;
            item.shootSpeed = 12f;
        }

        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.Next(2) == 0;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockback)
        {
            int num = Main.rand.Next(2, 5);
            for (int k = 0; k < num; k++)
            {
                Vector2 projSpeed = new Vector2(speedX, speedY);
                for (int j = 0; j < k; j++)
                {
                    projSpeed += new Vector2(Main.rand.Next(-35, 36), Main.rand.Next(-35, 36)) * 0.04f;
                }
                int proj = Projectile.NewProjectile(position, projSpeed, type, damage, knockback, player.whoAmI, 0f, 0f);
                if (k > 0)
                {
                    Main.projectile[proj].noDropItem = true;
                }
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PuriumBar", 12);
            recipe.AddTile(null, "PuriumAnvil");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}