using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons
{
	public class PuriumBreaker : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Purium Breaker";
			item.width = 40;
			item.height = 40;
			item.scale = 1.3f;
			item.useStyle = 1;
			item.useAnimation = 26;
			item.useTime = 38;
			item.damage = 943;
			item.knockBack = 7f;
			item.autoReuse = true;
			item.useTurn = false;
			item.rare = 11;
			item.melee = true;
			item.value = Item.sellPrice(0, 12, 0, 0);
			item.UseSound = SoundID.Item1;
			item.shoot = mod.ProjectileType("PuriumBoom");
			item.shootSpeed = 8f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockback)
		{
			float speed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
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
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockback, player.whoAmI, time, 0f);
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