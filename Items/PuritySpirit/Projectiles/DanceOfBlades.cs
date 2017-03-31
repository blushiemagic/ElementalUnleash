using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit.Projectiles
{
	public class DanceOfBlades : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.name = "Dance of Blades";
			projectile.width = 220;
			projectile.height = 220;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.hide = true;
			projectile.ownerHitCheck = false;
			projectile.melee = true;
			projectile.alpha = 30;
		}

		public override void AI()
		{
			projectile.soundDelay--;
			if (projectile.soundDelay <= 0)
			{
				Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 15);
				projectile.soundDelay = 45;
			}
			Player player = Main.player[projectile.owner];
			if (Main.myPlayer == projectile.owner)
			{
				if (!player.channel || player.noItems || player.CCed)
				{
					projectile.Kill();
				}
				else
				{
					projectile.ai[0] -= 1f;
					if (projectile.ai[0] <= 0f)
					{
						CreateBlade();
						projectile.ai[0] = 5f;
					}
				}
			}
			Lighting.AddLight(projectile.Center, 0.3f, 1f, 0.3f);
			projectile.Center = player.MountedCenter;
			projectile.position.X += player.width / 2 * player.direction;
			projectile.spriteDirection = player.direction;
			projectile.timeLeft = 2;
			projectile.rotation += 0.3f * player.direction;
			if (projectile.rotation > MathHelper.TwoPi)
			{
				projectile.rotation -= MathHelper.TwoPi;
			}
			else if (projectile.rotation < 0)
			{
				projectile.rotation += MathHelper.TwoPi;
			}
			player.heldProj = projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;
			player.itemRotation = projectile.rotation;
		}

		private void CreateBlade()
		{
			Player player = Main.player[projectile.owner];
			float x = player.Center.X + 2f * Main.rand.Next(-300, 301);
			float y = player.Center.Y - 400f;
			Projectile.NewProjectile(x, y, 0f, 12f, mod.ProjectileType("BladeRain"), projectile.damage, projectile.knockBack, projectile.owner);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 5;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			return false;
		}
	}
}