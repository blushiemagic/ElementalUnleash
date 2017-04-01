using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons.Projectiles
{
	public class PuriumArrow : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.name = "Purium Arrow";
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = 1;
			projectile.ranged = true;
			projectile.arrow = true;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ignoreWater = false;
			ProjectileID.Sets.NeedsUUID[projectile.type] = true;
		}

		public override void AI()
		{
			if (projectile.localAI[0] == 0f && Main.myPlayer == projectile.owner)
			{
				projectile.localAI[0] = 1f;
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, mod.ProjectileType("PuriumArrowTrail"), projectile.damage, projectile.knockBack, projectile.owner, projectile.projUUID, 0f);
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			for (int k = 0; k < 10; k++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 128);
			}
		}
	}
}