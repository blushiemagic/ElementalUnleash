using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.ChaosSpirit
{
	public class CrossFracture : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 32;
			projectile.hostile = true;
			projectile.penetrate = -1;
			projectile.magic = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			cooldownSlot = 1;
		}

		public override void AI()
		{
			if (projectile.ai[1] == 0f)
			{
				projectile.localAI[0] = Main.rand.NextFloat();
			}
			projectile.ai[1] += 1f;
			if (projectile.ai[1] == 120f)
			{
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
			}
			if (projectile.ai[1] >= 150f)
			{
				projectile.Kill();
			}
		}

		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			if (target.hurtCooldowns[1] <= 0)
			{
				BluemagicPlayer modPlayer = target.GetModPlayer<BluemagicPlayer>(mod);
				modPlayer.constantDamage = 200;
				modPlayer.percentDamage = 1f / 3f;
				if (Main.expertMode)
				{
					modPlayer.constantDamage = (int)(modPlayer.constantDamage * 1.5f);
					modPlayer.percentDamage *= 1.5f;
				}
				modPlayer.chaosDefense = true;
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(mod.BuffType("Undead"), 300, false);
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Vector2.Distance(target.Center, projectile.Center) >= 600f)
			{
				return false;
			}
			return base.CanHitNPC(target);
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (projectile.ai[1] >= 120f)
			{
				float width = projectile.width * ((150f - projectile.ai[1]) / 30f);
				float num = 0f;
				Vector2 offset = 2400f * projectile.ai[0].ToRotationVector2();
				Vector2 start = projectile.Center + offset;
				Vector2 end = projectile.Center - offset;
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, width, ref num))
				{
					return true;
				}
				num = 0f;
				offset = 2400f * (projectile.ai[0] + MathHelper.PiOver2).ToRotationVector2();
				start = projectile.Center + offset;
				end = projectile.Center - offset;
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, width, ref num))
				{
					return true;
				}
			}
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Color color = Main.hslToRgb(projectile.localAI[0], 1f, 0.6f);
			float laserAlpha = 0f;
			if (projectile.ai[1] >= 120f)
			{
				laserAlpha = 1f;
			}
			else if (Math.Abs(projectile.ai[1] - 60) < 10)
			{
				laserAlpha = (10f - Math.Abs(projectile.ai[1] - 60f)) / 40f;
			}
			float scale = 1f;
			if (projectile.ai[1] >= 120f)
			{
				scale = (150f - projectile.ai[1]) / 30f;
			}
			if (laserAlpha > 0f && scale > 0f)
			{
				Texture2D texture = Main.projectileTexture[projectile.type];
				Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
				Vector2 drawPos = projectile.Center - Main.screenPosition;
				Vector2 drawScale = new Vector2(2400f, scale);
				float rotation = projectile.ai[0];
				Color drawColor = color * laserAlpha;
				spriteBatch.Draw(texture, drawPos, null, drawColor, rotation, origin, drawScale, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, drawPos, null, drawColor, rotation + MathHelper.PiOver2, origin, drawScale, SpriteEffects.None, 0f);
			}

			if (projectile.ai[1] < 120f)
			{
				Texture2D texture = mod.GetTexture("ChaosSpirit/CrossFractureHolder");
				Vector2 drawCenter = projectile.Center - Main.screenPosition;
				Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
				for (int k = 0; k < 4; k++)
				{
					float rotation = projectile.ai[0] + MathHelper.PiOver2 * k;
					Vector2 drawPos = drawCenter + 240f * rotation.ToRotationVector2();
					spriteBatch.Draw(texture, drawPos, null, color, rotation, origin, 1f, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
	}
}