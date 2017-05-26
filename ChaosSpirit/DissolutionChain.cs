using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.ChaosSpirit
{
	public class DissolutionChain : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.name = "Dissolution Chain";
			projectile.width = 32;
			projectile.height = 32;
			projectile.hostile = true;
			projectile.penetrate = -1;
			projectile.magic = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			cooldownSlot = 1;
		}

		private int timer = 0;
		private Color color;

		private float Length
		{
			get
			{
				int interval = Main.expertMode ? 40 : 50;
				if (timer <= interval)
				{
					return 0f;
				}
				float length = 32f * (timer - interval);
				if (length > 2400f)
				{
					length = 2400f;
				}
				return length;
			}
		}

		private Vector2 Source
		{
			get
			{
				return new Vector2(projectile.ai[0], projectile.ai[1]);
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.localAI[0]);
			writer.Write(projectile.localAI[1]);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.localAI[0] = reader.ReadSingle();
			projectile.localAI[1] = reader.ReadSingle();
		}

		public override void AI()
		{
			if (projectile.velocity.X != 0f)
			{
				projectile.localAI[0] = projectile.velocity.X == -1f ? 0f : projectile.velocity.X;
				projectile.velocity.X = 0f;
			}
			if (projectile.velocity.Y != 0f)
			{
				projectile.localAI[1] = projectile.velocity.Y;
				projectile.velocity.Y = 0f;
			}
			if (timer == 0)
			{
				color = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f);
			}
			if (projectile.localAI[1] > 0f)
			{
				timer++;
				int interval = Main.expertMode ? 40 : 50;
				if (timer == interval && projectile.localAI[1] > interval + 10f && Main.netMode != 1)
				{
					Player player = Main.player[(int)projectile.localAI[0]];
					int damage = 150;
					if (Main.expertMode)
					{
						damage = (int)(damage * 1.5f / 2f);
					}
					int proj = Projectile.NewProjectile(player.Center, Vector2.Zero, projectile.type, damage, 0f, Main.myPlayer, projectile.Center.X, projectile.Center.Y);
					Main.projectile[proj].localAI[0] = projectile.localAI[0];
					Main.projectile[proj].localAI[1] = projectile.localAI[1];
					NetMessage.SendData(27, -1, -1, "", proj);
				}
				if (timer == interval)
				{
					Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 81);
				}
			}
			projectile.localAI[1] -= 1f;
			if (projectile.localAI[1] <= -15f)
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
			if (projectile.localAI[1] <= 0f)
			{
				return false;
			}
			float length = Length;
			if (length > 0f)
			{
				float num = 0f;
				Vector2 center = Source;
				Vector2 direction = projectile.Center - center;
				if (direction == Vector2.Zero)
				{
					direction = new Vector2(0f, 1f);
				}
				direction.Normalize();
				direction *= length;
				Vector2 start = center + direction;
				Vector2 end = center - direction;
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 16, ref num))
				{
					return true;
				}
			}
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawCenter = Source - Main.screenPosition;
			Vector2 direction = projectile.Center - Source;
			if (direction == Vector2.Zero)
			{
				direction = new Vector2(0f, 1f);
			}
			direction.Normalize();
			float alpha = 1f;
			if (projectile.localAI[1] < 0f)
			{
				alpha = 1f + projectile.localAI[1] / 15f;
			}
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float length = Length;
			for (float k = 0f; k < length; k += projectile.width / 2)
			{
				spriteBatch.Draw(texture, drawCenter + k * direction, null, color * alpha, 0f, origin, 1f, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, drawCenter - k * direction, null, color * alpha, 0f, origin, 1f, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, ChaosSpirit.mainColor * alpha, 0f, origin, 2f, SpriteEffects.None, 0f);
			return false;
		}
	}
}