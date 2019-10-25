using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.ChaosSpirit
{
	public class DissonanceOrb : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.NeedsUUID[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 64;
			projectile.height = 64;
			projectile.hostile = true;
			projectile.penetrate = -1;
			projectile.magic = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			cooldownSlot = 1;
		}

		private int timer = 0;
		private bool synced = false;

		public override void AI()
		{
			if (Main.netMode == 2 && !synced)
			{
				NetMessage.SendData(27, -1, -1, null, projectile.whoAmI);
				synced = true;
			}
			if (timer == 0)
			{
				float direction = 1f;
				if (projectile.ai[0] < 0f)
				{
					projectile.ai[0] += 1f;
					projectile.ai[0] *= -1f;
					direction = -1f;
				}
				Vector2 target = Main.player[(int)projectile.ai[0]].Center;
				projectile.ai[0] = (target - projectile.Center).ToRotation();
				projectile.localAI[0] = direction;
			}
			timer++;
			if (timer > 180)
			{
				projectile.ai[0] += projectile.localAI[0] * (0.005f + ((timer - 180f) / 180f * 0.015f));
			}
			if (timer > 360)
			{
				projectile.Kill();
			}
			projectile.rotation += 0.05f;
		}

		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			if (target.hurtCooldowns[1] <= 0)
			{
				BluemagicPlayer modPlayer = target.GetModPlayer<BluemagicPlayer>();
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
			if (timer > 120)
			{
				float num = 0f;
				int other = Main.projectileIdentity[projectile.owner, (int)projectile.ai[1]];
				if (other >= 0)
				{
					Vector2 laserTarget = Main.projectile[other].Center;
					Vector2 offset = laserTarget - projectile.Center;
					Vector2 end = projectile.Center + offset;
					if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, end, 16, ref num))
					{
						return true;
					}
				}
				{
					num = 0f;
					Vector2 direction = projectile.ai[0].ToRotationVector2();
					Vector2 end = projectile.Center + 2400f * direction;
					if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, end, 16, ref num))
					{
						return true;
					}
				}
			}
			return base.Colliding(projHitbox, targetHitbox);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture;
			Vector2 origin;
			Vector2 drawPos;

			float laserAlpha = 0f;
			if (timer > 120)
			{
				laserAlpha = 1f;
			}
			else if (Math.Abs(timer - 60) < 20)
			{
				laserAlpha = (20f - Math.Abs(timer - 60)) / 40f;
			}
			else if (timer > 100)
			{
				laserAlpha = (timer - 100) / 20f;
			}
			if (laserAlpha > 0f)
			{
				texture = mod.GetTexture("ChaosSpirit/DissonanceRay");
				Color color = Color.White * laserAlpha;
				origin = new Vector2(texture.Width / 2, texture.Height / 2);
				int other = Main.projectileIdentity[projectile.owner, (int)projectile.ai[1]];
				if (other >= 0)
				{
					Vector2 laserTarget = Main.projectile[other].Center;
					Vector2 direction = laserTarget - projectile.Center;
					float length = direction.Length();
					direction.Normalize();
					float rotation = direction.ToRotation();
					for (float k = 8f; k < length; k += 16f)
					{
						drawPos = projectile.Center + k * direction - Main.screenPosition;
						float useRotation = rotation;
						if (Main.rand.Next(2) == 0)
						{
							useRotation += MathHelper.Pi;
						}
						spriteBatch.Draw(texture, drawPos, null, color, useRotation, origin, 1f, SpriteEffects.None, 0f);
					}
				}

				{
					Vector2 direction = projectile.ai[0].ToRotationVector2();
					float length = 2400f;
					float rotation = projectile.ai[0];
					for (float k = 8f; k < length; k += 16f)
					{
						drawPos = projectile.Center + k * direction - Main.screenPosition;
						float useRotation = rotation;
						if (Main.rand.Next(2) == 0)
						{
							useRotation += MathHelper.Pi;
						}
						spriteBatch.Draw(texture, drawPos, null, color, useRotation, origin, 1f, SpriteEffects.None, 0f);
					}
				}
			}

			texture = Main.projectileTexture[projectile.type];
			drawPos = projectile.Center - Main.screenPosition;
			origin = new Vector2(texture.Width / 2, texture.Height / 2);
			spriteBatch.Draw(texture, drawPos, null, Color.White, projectile.rotation, origin, 0.5f, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, drawPos, null, Color.White, -projectile.rotation, origin, 0.5f, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, drawPos, null, Color.White, -projectile.rotation, origin, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, drawPos, null, Color.White, projectile.rotation, origin, 1f, SpriteEffects.None, 0f);
			return false;
		}
	}
}