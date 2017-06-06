using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons.Projectiles
{
	public class PuriumSpear : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Purium Lightbeam");
		}

		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 18;
			projectile.aiStyle = 19;
			projectile.melee = true;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.scale = 1.3f;
			projectile.penetrate = -1;
			projectile.hide = true;
			projectile.ownerHitCheck = true;
		}

		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			Vector2 center = player.RotatedRelativePoint(player.MountedCenter, true);
			projectile.direction = player.direction;
			player.heldProj = projectile.whoAmI;
			player.itemTime = player.itemAnimation;
			projectile.position = center - projectile.Size / 2f;
			bool forwards = true;
			if (!player.frozen)
			{
				if (projectile.ai[0] == 0f)
				{
					projectile.ai[0] = 3f;
					projectile.netUpdate = true;
				}
				if (player.itemAnimation < player.itemAnimationMax / 3)
				{
					projectile.ai[0] -= 2.4f;
					forwards = false;
				}
				else
				{
					projectile.ai[0] += 2.1f;
				}
			}
			projectile.position += projectile.velocity * projectile.ai[0];
			float speed = (float)Math.Sqrt(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y);
			if (projectile.owner == Main.myPlayer && projectile.localAI[0] == 0f)
			{
				int proj = Projectile.NewProjectile(projectile.Center, projectile.velocity * 2f / speed, mod.ProjectileType("PuriumLightbeam"), projectile.damage, projectile.knockBack, projectile.owner);
				projectile.ai[1] = proj;
				projectile.localAI[0] = 1f;
			}
			int uuid = Projectile.GetByUUID(projectile.owner, projectile.ai[1]);
			if (forwards && uuid >= 0 && Main.projectile[uuid].active && Main.projectile[uuid].type == mod.ProjectileType("PuriumLightbeam"))
			{
				PuriumLightbeam beam = (PuriumLightbeam)Main.projectile[uuid].modProjectile;
				beam.AddPosition(projectile.Center);
			}
			if (player.itemAnimation == 0)
			{
				projectile.Kill();
			}
			projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 2.355f;
			if (projectile.spriteDirection == -1)
			{
				projectile.rotation -= 1.57f;
			}
			return false;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (target.defense > 100)
			{
				damage += (target.defense - 100) / 2;
			}
		}
	}
}