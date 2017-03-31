using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit.Projectiles
{
	public class BladeRain : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.name = "Dance of Blades";
			projectile.width = 32;
			projectile.height = 100;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.melee = true;
			projectile.alpha = 30;
		}

		public override void AI()
		{
			if (projectile.position.Y > Main.player[projectile.owner].position.Y + 400)
			{
				projectile.Kill();
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * 0.8f;
		}
	}
}