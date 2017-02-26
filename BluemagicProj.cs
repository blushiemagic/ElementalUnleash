using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Projectiles
{
	public class BluemagicProj : GlobalProjectile
	{
		public override void AI(Projectile projectile)
		{
			if (projectile.type != ProjectileID.NorthPoleSnowflake && !projectile.npcProj && projectile.melee && !projectile.noEnchantments && Main.player[projectile.owner].active)
			{
				int meleeEnchant = Main.player[projectile.owner].GetModPlayer<BluemagicPlayer>(mod).customMeleeEnchant;
				if (meleeEnchant == 1)
				{
					if (Main.rand.Next(2) == 0)
					{
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("EtherealFlame"), projectile.velocity.X * 0.2f + projectile.direction * 3, projectile.velocity.Y * 0.2f, 100, default(Color), 2.5f);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 0.7f;
						Main.dust[dust].velocity.Y -= 0.5f;
					}
				}
			}
		}
	}
}