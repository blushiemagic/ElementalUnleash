using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Projectiles
{
	public class PuriumJackhammer : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.name = "Purium Jackhammer";
			projectile.width = 18;
			projectile.height = 18;
			projectile.scale = 1.2f;
			projectile.aiStyle = 20;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hide = true;
			projectile.ownerHitCheck = true;
			projectile.melee = true;
			Main.projFrames[projectile.type] = 4;
		}

		public override void AI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 4)
			{
				projectile.frameCounter = 0;
				projectile.frame++;
			}
			if (projectile.frame > 3)
			{
				projectile.frame = 0;
			}
		}
	}
}