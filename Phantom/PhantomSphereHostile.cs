using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Phantom
{
	public class PhantomSphereHostile : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.name = "Phantom Sphere";
			projectile.width = 192;
			projectile.height = 192;
			projectile.alpha = 70;
			projectile.hostile = true;
			projectile.timeLeft = 1200;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.maxPenetrate = -1;
		}

		public override void AI()
		{
			projectile.ai[1] += 1f;
			if (projectile.ai[1] == 120f)
			{
				Player player = Main.player[Main.npc[(int)projectile.ai[0]].target];
				Vector2 offset = player.Center - projectile.Center;
				if (Main.expertMode)
				{
					Vector2 prediction = player.velocity;
					prediction *= offset.Length() / 6f;
					prediction *= Main.rand.NextFloat();
					offset += prediction;
				}
				if (offset != Vector2.Zero)
				{
					offset.Normalize();
					offset *= 6f;
				}
				projectile.velocity = offset;
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			return Ellipse.Collides(new Vector2(projHitbox.X, projHitbox.Y), new Vector2(projHitbox.Width, projHitbox.Height), new Vector2(targetHitbox.X, targetHitbox.Y), new Vector2(targetHitbox.Width, targetHitbox.Height));
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * 0.85f;
		}
	}
}