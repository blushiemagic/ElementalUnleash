using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit.Projectiles
{
	public class PrismaticShocker : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.magic = true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if (!player.active || player.dead || player.inventory[player.selectedItem].type != mod.ItemType("PrismaticShocker"))
			{
				projectile.Kill();
				return;
			}
			if (projectile.ai[0] == 0f)
			{
				Vector2 offset = projectile.Center - player.Center;
				projectile.ai[0] = offset.Length();
				projectile.ai[1] = offset.ToRotation();
			}
			projectile.ai[1] += 0.02f;
			projectile.ai[1] %= MathHelper.TwoPi;
			if (projectile.ai[0] == 0f)
			{
				projectile.Center = player.Center;
			}
			else
			{
				projectile.Center = player.Center + projectile.ai[0] * projectile.ai[1].ToRotationVector2();
			}
			projectile.rotation -= 0.05f;
			projectile.localAI[0] += 0.2f;
			projectile.localAI[0] %= 4f;
			Color light = GetAlpha(Color.White).Value;
			Lighting.AddLight(projectile.Center, light.R / 255f, light.G / 255f, light.B / 255f);
			projectile.timeLeft = 2;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			for (int k = projectile.whoAmI + 1; k < 1000; k++)
			{
				Projectile proj = Main.projectile[k];
				if (proj.active && proj.owner == projectile.owner && proj.type == projectile.type)
				{
					if (BeamCheck(proj, targetHitbox))
					{
						return true;
					}
				}
			}
			return null;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 4;
		}

		private bool BeamCheck(Projectile proj, Rectangle targetHitbox)
		{
			float point = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, proj.Center, 6f, ref point);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			int r = (Main.DiscoR + 4 * 255) / 5;
			int g = (Main.DiscoG + 4 * 255) / 5;
			int b = (Main.DiscoB + 4 * 255) / 5;
			return new Color(r, g, b);
		}

		public Color BeamColor()
		{
			int r = (Main.DiscoR + 255) / 2;
			int g = (Main.DiscoG + 255) / 2;
			int b = (Main.DiscoB + 255) / 2;
			return new Color(r, g, b);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for (int k = projectile.whoAmI + 1; k < 1000; k++)
			{
				Projectile proj = Main.projectile[k];
				if (proj.active && proj.owner == projectile.owner && proj.type == projectile.type)
				{
					DrawBeamTo(spriteBatch, proj, lightColor);
				}
			}
			return true;
		}

		private void DrawBeamTo(SpriteBatch spriteBatch, Projectile proj, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Items/PuritySpirit/Projectiles/PrismaticShockerBeam");
			Vector2 unit = proj.Center - projectile.Center;
			float length = unit.Length();
			unit.Normalize();
			float rotation = unit.ToRotation();
			Color color = BeamColor() * 0.6f;
			for (float k = projectile.localAI[0]; k <= length; k += 8f)
			{
				Vector2 drawPos = projectile.Center + unit * k - Main.screenPosition;
				spriteBatch.Draw(texture, drawPos, null, color, rotation, new Vector2(4, 4), 1f, SpriteEffects.None, 0f);
			}
		}
	}
}