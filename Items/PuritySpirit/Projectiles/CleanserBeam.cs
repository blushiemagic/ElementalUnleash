using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit.Projectiles
{
	public class CleanserBeam : ModProjectile
	{
		private const int maxTime = 50;

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.NeedsUUID[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 80;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.hide = true;
			projectile.ranged = true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if (projectile.ai[0] < 2f * maxTime)
			{
				projectile.ai[0] += 1f;
			}
			if (Main.myPlayer == projectile.owner)
			{
				if (projectile.ai[0] == maxTime)
				{
					Vector2 direction = Vector2.Normalize(projectile.velocity);
					if (float.IsNaN(direction.X) || float.IsNaN(direction.Y))
					{
						direction = -Vector2.UnitY;
					}
					int laser = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, direction.X, direction.Y, mod.ProjectileType("CleanserLaser"), projectile.damage, projectile.knockBack, projectile.owner, 0f, projectile.whoAmI);
					projectile.ai[1] = Main.projectile[laser].identity;
					projectile.netUpdate = true;
				}
				if (!player.channel || player.noItems || player.CCed)
				{
					projectile.Kill();
				}
			}
			projectile.soundDelay--;
			if (projectile.ai[0] >= maxTime && projectile.soundDelay <= 0)
			{
				Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 15);
				projectile.soundDelay = 40;
			}

			projectile.Center = player.RotatedRelativePoint(player.MountedCenter, true);
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
			projectile.spriteDirection = projectile.direction;
			projectile.timeLeft = 2;
			player.heldProj = projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;
			player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * projectile.direction, projectile.velocity.X * projectile.direction);

			if (projectile.ai[0] < maxTime && projectile.velocity != Vector2.Zero)
			{
				Vector2 dustTarget = projectile.Center + Vector2.Normalize(projectile.velocity) * 50f;
				for (int k = 0; k < 1; k++)
				{
					int dust = Dust.NewDust(dustTarget - new Vector2(24f, 24f), 48, 48, mod.DustType("CleanserBeamCharge"), 0f, 0f, 70);
					Main.dust[dust].customData = dustTarget;
				}
			}
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			int byUUID = Projectile.GetByUUID(projectile.owner, projectile.ai[1]);
			if (byUUID >= 0 && projectile.ai[0] > maxTime && Main.projectile[byUUID].type == mod.ProjectileType("CleanserLaser"))
			{
				Main.instance.DrawProj(byUUID);
			}
			return true;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Items/PuritySpirit/Projectiles/CleanserBeam_Glow");
			SpriteEffects spriteEffects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), null, Color.White, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), projectile.scale, spriteEffects, 0f);
		}
	}
}