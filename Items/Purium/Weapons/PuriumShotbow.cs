/*using System;
using Microsoft.Xna.Framework;
using Terraria;
using TAPI;

namespace Bluemagic.Items
{
	public class PuriumShotbow : ModItem
	{
		public override bool PreShoot(Player player, Vector2 position, Vector2 velocity, int projType, int damage, float knockback)
		{
			Vector2 direction = velocity;
			direction.Normalize();
			position += direction * item.width;
			int num = Main.rand.Next(4, 9) / 2;
			for (int k = 0; k < num; k++)
			{
				Vector2 projSpeed = velocity;
				for (int j = 0; j < k; j++)
				{
					projSpeed += new Vector2(Main.rand.Next(-35, 36), Main.rand.Next(-35, 36)) * 0.04f;
				}
				int proj = Projectile.NewProjectile(position, projSpeed, projType, damage, knockback, player.whoAmI, 0f, 0f);
				if (k > 0)
				{
					Main.projectile[proj].noDropItem = true;
				}
			}
			return false;
		}
	}
}*/