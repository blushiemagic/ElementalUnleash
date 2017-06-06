using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom.Projectiles
{
	public class PhantomHammerFriendly : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phantom Hammer");
		}

		public override void SetDefaults()
		{
			projectile.width = 38;
			projectile.height = 38;
			projectile.alpha = 70;
			projectile.timeLeft = 300;
			projectile.maxPenetrate = -1;
			projectile.friendly = true;
			projectile.thrown = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}

		public override void AI()
		{
			projectile.rotation += 0.5f;
			if (projectile.velocity.Y < 0f)
			{
				projectile.velocity.Y += 0.15f;
			}
			else
			{
				projectile.velocity.Y += 0.5f;
			}
			if (projectile.velocity.Y > 32f)
			{
				projectile.velocity.Y = 32f;
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * 0.85f;
		}
	}
}