using System;
using Microsoft.Xna.Framework;
using Terraria;
using TAPI;

namespace Bluemagic.Items
{
	public class PuriumBreaker : ModItem
	{
		public override bool PreShoot(Player player, Vector2 position, Vector2 velocity, int projType, int damage, float knockback)
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
			Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI, time, 0f);
			return false;
		}
	}
}