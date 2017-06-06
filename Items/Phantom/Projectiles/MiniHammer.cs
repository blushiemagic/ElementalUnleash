using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom.Projectiles
{
	public class MiniHammer : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.Homing[projectile.type] = true;
			ProjectileID.Sets.MinionShot[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 19;
			projectile.height = 19;
			projectile.timeLeft = 200;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}

		public override void AI()
		{
			if (projectile.velocity.X < 0f)
			{
				projectile.rotation -= 0.5f;
			}
			else
			{
				projectile.rotation += 0.5f;
			}
		}
	}
}