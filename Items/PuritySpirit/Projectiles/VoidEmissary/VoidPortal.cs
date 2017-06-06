using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit.Projectiles.VoidEmissary
{
	public class VoidPortal : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 2;
			ProjectileID.Sets.Homing[projectile.type] = true;
			ProjectileID.Sets.MinionShot[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 80;
			projectile.height = 80;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}

		public override void AI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 10)
			{
				projectile.frame = 1 - projectile.frame;
				projectile.frameCounter = 0;
			}
			projectile.ai[0] += 1f;
			if (projectile.ai[0] > 60f)
			{
				projectile.Kill();
			}
			if (projectile.ai[1] == 1f)
			{
				projectile.ai[1] = 2f;
			}
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (projectile.ai[1] == 2f)
			{
				return false;
			}
			return base.CanHitNPC(target);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.penetrate++;
			if (projectile.ai[1] == 0f)
			{
				projectile.ai[1] = 1f;
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}