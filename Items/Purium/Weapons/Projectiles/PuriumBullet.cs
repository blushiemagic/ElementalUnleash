using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons.Projectiles
{
	public class PuriumBullet : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.alpha = 255;
			projectile.ranged = true;
			projectile.penetrate = 20;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ignoreWater = false;
			projectile.MaxUpdates = 16;
		}

		public override void AI()
		{
			if (projectile.soundDelay > 0)
			{
				projectile.soundDelay--;
			}
			if (projectile.localAI[0] == 0f)
			{
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 12);
				projectile.localAI[0] = 1f;
				projectile.soundDelay = 96;
				float speed = (float)Math.Sqrt(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y);
				projectile.velocity *= 2f / speed;
			}
			Dust.NewDust(projectile.position, 0, 0, mod.DustType("PuriumBullet"));
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.penetrate--;
			if (projectile.penetrate > 0)
			{
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
				if (projectile.soundDelay <= 0f)
				{
					Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 12);
					projectile.soundDelay = 96;
				}
			}
			else
			{
				return true;
			}
			return false;
		}

		public override void OnHitPvp(Player player, int damage, bool crit)
		{
			if (Main.rand.Next(2) == 0)
			{
				player.AddBuff(BuffID.BrokenArmor, 600, true);
			}
		}
	}
}