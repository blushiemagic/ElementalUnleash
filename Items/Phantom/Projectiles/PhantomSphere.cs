using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom.Projectiles
{
	public class PhantomSphere : ModProjectile
	{
		private const int maxTime = 600;
		private const int fadeOutTime = 100;
		private const int fadeInTime = 50;

		public override void SetDefaults()
		{
			projectile.width = 192;
			projectile.height = 192;
			projectile.alpha = 255;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
		}

		public override void AI()
		{
			for (int k = 0; k < 1000; k++)
			{
				if (k != projectile.whoAmI && Main.projectile[k].type == projectile.type && Main.projectile[k].owner == projectile.owner && Main.projectile[k].ai[0] < projectile.ai[0])
				{
					if (projectile.ai[0] < maxTime - fadeOutTime)
					{
						projectile.ai[0] = maxTime - fadeOutTime;
					}
					break;
				}
			}
			projectile.Center = Main.player[projectile.owner].Center;
			if (projectile.ai[0] < fadeInTime)
			{
				projectile.alpha = (int)((fadeInTime - projectile.ai[0]) * 185f / 50f) + 70;
			}
			if (projectile.ai[0] >= maxTime - fadeOutTime)
			{
				projectile.alpha = 255 - (int)((maxTime - projectile.ai[0]) * 185f / 100f);
			}
			if (projectile.ai[0] >= maxTime)
			{
				projectile.Kill();
			}
			projectile.ai[0] += 1f;
			for (int x = 0; x < 3; x++)
			{
				if (Main.rand.Next(30) == 0)
				{
					float angle = (float)Main.rand.Next(36) * 2f * (float)Math.PI / 36f;
					float xOff = (float)projectile.width * 0.5f * (float)Math.Cos(angle);
					float yOff = (float)projectile.width * 0.5f * (float)Math.Sin(angle);
					Dust.NewDust(projectile.Center + new Vector2(xOff, yOff), 0, 0, mod.DustType("SpectreDust"));
				}
			}
			Lighting.AddLight(projectile.Center, 0.05f, 0.15f, 0.2f);
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			return Ellipse.Collides(new Vector2(projHitbox.X, projHitbox.Y), new Vector2(projHitbox.Width, projHitbox.Height), new Vector2(targetHitbox.X, targetHitbox.Y), new Vector2(targetHitbox.Width, targetHitbox.Height));
		}

		public override Color? GetAlpha(Color color)
		{
			return Color.White * ((255 - projectile.alpha) / 255f);
		}
	}
}