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
		public override void SetDefaults()
		{
			projectile.name = "Dissonance Orb";
			projectile.width = 64;
			projectile.height = 64;
			projectile.hostile = true;
			projectile.penetrate = -1;
			projectile.magic = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			cooldownSlot = 1;
			ProjectileID.Sets.NeedsUUID[projectile.type] = true;
		}

		private int timer = 0;
		private bool synced = false;

		public override void AI()
		{
			if (Main.netMode == 2 && !synced)
			{
				NetMessage.SendData(27, -1, -1, "", projectile.whoAmI);
				synced = true;
			}
			Player player = Main.player[(int)projectile.ai[0]];
			if (timer == 0)
			{
				projectile.localAI[0] = player.Center.X;
				projectile.localAI[1] = player.Center.Y;
			}
			timer++;
			if (timer > 180)
			{
				Vector2 currentTarget = new Vector2(projectile.localAI[0], projectile.localAI[1]);
				Vector2 offset = player.Center - currentTarget;
				if (offset.Length() > 4f)
				{
					offset.Normalize();
					offset *= 4f;
				}
				currentTarget += offset;
				projectile.localAI[0] = currentTarget.X;
				projectile.localAI[1] = currentTarget.Y;
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
				BluemagicPlayer modPlayer = target.GetModPlayer<BluemagicPlayer>(mod);
				modPlayer.constantDamage = projectile.damage;
				modPlayer.percentDamage = 0.25f;
				if (Main.expertMode)
				{
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
				if (projectile.ai[0] == Main.myPlayer)
				{
					num = 0f;
					Vector2 laserTarget = new Vector2(projectile.localAI[0], projectile.localAI[1]);
					Vector2 offset = laserTarget - projectile.Center;
					if (offset == Vector2.Zero)
					{
						offset = new Vector2(0f, 1f);
					}
					offset.Normalize();
					Vector2 end = projectile.Center + 2400f * offset;
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

				if (projectile.ai[0] == Main.myPlayer)
				{
					Vector2 laserTarget = new Vector2(projectile.localAI[0], projectile.localAI[1]);
					Vector2 direction = laserTarget - projectile.Center;
					if (direction == Vector2.Zero)
					{
						direction = new Vector2(0f, 1f);
					}
					direction.Normalize();
					float length = 2400f;
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