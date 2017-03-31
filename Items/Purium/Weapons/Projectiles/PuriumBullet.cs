/*using System;
using Microsoft.Xna.Framework;
using Terraria;
using TAPI;

namespace Bluemagic.Projectiles
{
	public class PuriumBullet : ModProjectile
	{
		public override void AI()
		{
			if (projectile.localAI[1] > 0f)
			{
				projectile.localAI[1] -= 16f / (float)projectile.maxUpdates;
			}
			if (projectile.localAI[0] == 0f)
			{
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 12);
				projectile.localAI[0] = 1f;
				projectile.localAI[1] = 30f;
				float speed = (float)Math.Sqrt(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y);
				projectile.velocity *= 2f / speed;
			}
			BluemagicDust.CreatePuriumBulletDust(projectile.position);
		}

		public override bool OnTileCollide(ref Vector2 velocityChange)
		{
			projectile.ai[0] += 1f;
			if (projectile.ai[0] < 20f)
			{
				if (projectile.velocity.X != velocityChange.X)
				{
					projectile.velocity.X = -velocityChange.X;
				}
				if (projectile.velocity.Y != velocityChange.Y)
				{
					projectile.velocity.Y = -velocityChange.Y;
				}
				if (projectile.localAI[1] <= 0f)
				{
					Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 12);
					projectile.localAI[1] = 96f;
				}
			}
			else
			{
				projectile.Kill();
			}
			return false;
		}

		public override void DealtPlayer(Player player, int hitDir, int dmgDealt, bool crit)
		{
			if (Main.rand.Next(2) == 0)
			{
				player.AddBuff(36, 600, true);
			}
		}
	}
}*/