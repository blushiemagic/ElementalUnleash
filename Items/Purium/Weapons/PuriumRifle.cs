using System;
using Microsoft.Xna.Framework;
using Terraria;
using TAPI;

namespace Bluemagic.Items
{
	public class PuriumRifle : ModItem
	{
		public override bool PreShoot(Player player, Vector2 position, Vector2 velocity, int projType, int damage, float knockback)
		{
			Vector2 direction = velocity;
			direction.Normalize();
			position += direction * item.width;
			int proj = Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI, 0f, 0f);
			Main.projectile[proj].maxUpdates += (int)Math.Sqrt(Main.projectile[proj].maxUpdates);
			if (projType == ProjDef.byName["Bluemagic:PuriumBullet"].type)
			{
				Main.projectile[proj].maxUpdates = 32;
			}
			if (Main.netMode != 0)
			{
				BluemagicNet.SendProjMaxUpdates(proj);
			}
			return false;
		}
	}
}