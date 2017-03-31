/*using System;
using Microsoft.Xna.Framework;
using Terraria;
using TAPI;

namespace Bluemagic.Projectiles
{
	public class PuriumArrow : ModProjectile
	{
		public override void AI()
		{
			if (projectile.localAI[0] == 0f && Main.myPlayer == projectile.owner)
			{
				projectile.localAI[0] = 1f;
				projectile.localAI[1] = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, "Bluemagic:PuriumArrowTrail", projectile.damage, projectile.knockBack, projectile.owner, 45f, 0f);
			}
			float speed = (float)Math.Sqrt(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y);
			projectile.ai[1] += speed;
			int k = 0;
			if (Main.myPlayer == projectile.owner)
			{
				TrailHitbox trail = Main.projectile[(int)projectile.localAI[1]].GetSubClass<TrailHitbox>();
				while (projectile.ai[1] >= 4f)
				{
					projectile.ai[1] -= 4f;
					k++;
				}
				while (k > 0)
				{
					Vector2 pos = projectile.Center - projectile.velocity * 4f / speed * k;
					trail.Add(pos);
					BluemagicNet.SendTrailAdd((int)projectile.localAI[1], pos);
					k--;
				}
			}
		}

		public override void Kill()
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			for (int k = 0; k < 10; k++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 128);
			}
		}
	}
}*/