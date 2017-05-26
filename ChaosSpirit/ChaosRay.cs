using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.ChaosSpirit
{
	public class ChaosRay : ModProjectile
	{
		private const float length = 2400f;

		public override void SetDefaults()
		{
			projectile.name = "Chaos Ray";
			projectile.width = 48;
			projectile.height = 48;
			projectile.hostile = true;
			projectile.penetrate = -1;
			projectile.magic = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			cooldownSlot = 1;
		}

		private bool didHit = false;

		private float TrueRotation
		{
			get
			{
				float t = projectile.localAI[1] / 240f;
				return (1f - t) * projectile.ai[1] + t * projectile.localAI[0];
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.localAI[0]);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.localAI[0] = reader.ReadSingle();
		}

		public override void AI()
		{
			if (projectile.knockBack != 0f)
			{
				projectile.localAI[0] = projectile.knockBack;
				projectile.knockBack = 0f;
			}
			NPC npc = Main.npc[(int)projectile.ai[0]];
			if (!npc.active || npc.type != mod.NPCType("ChaosSpiritArm") || projectile.localAI[0] > 2f * MathHelper.TwoPi || projectile.localAI[0] < -2f * MathHelper.TwoPi)
			{
				projectile.Kill();
				return;
			}
			projectile.Center = npc.Center;
			projectile.localAI[1] += 1f;
			if (projectile.localAI[1] > 240f)
			{
				projectile.Kill();
				return;
			}
			NPC spirit = Main.npc[(int)npc.ai[0]];
			if (Main.netMode != 1 && !didHit && spirit.active && spirit.modNPC is ChaosSpirit2 && Colliding(projectile.Hitbox, spirit.Hitbox).Value)
			{
				((ChaosSpirit2)spirit.modNPC).Damage();
				didHit = true;
			}
			CreateDust();
		}

		private Color GetColor()
		{
			NPC npc = Main.npc[(int)projectile.ai[0]];
			return ChaosSpiritArm.GetColor((int)npc.ai[1]);
		}

		private void CreateDust()
		{
			Color color = GetColor();
			Vector2 direction = TrueRotation.ToRotationVector2();
			Vector2 center = projectile.Center + direction * length;
			for (int k = 0; k < 4; k++)
			{
				float angle = TrueRotation + (Main.rand.Next(2) * 2 - 1) * (float)Math.PI / 2f;
				float speed = (float)Main.rand.NextDouble() * 2.6f + 1f;
				Vector2 velocity = speed * angle.ToRotationVector2();
				int dust = Dust.NewDust(center, 0, 0, 267, velocity.X, velocity.Y, 0, color, 1.2f);
				Main.dust[dust].noGravity = true;
			}
		}

		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			if (target.hurtCooldowns[1] <= 0)
			{
				BluemagicPlayer modPlayer = target.GetModPlayer<BluemagicPlayer>(mod);
				modPlayer.constantDamage = 360;
				modPlayer.percentDamage = 0.6f;
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
			float num = 0f;
			Vector2 end = projectile.Center + length * TrueRotation.ToRotationVector2();
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, end, projectile.width, ref num);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Color color = GetColor();
			float trueRotation = TrueRotation;
			Vector2 direction = trueRotation.ToRotationVector2();
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			for (float k = projectile.width * 1.5f; k < length; k += projectile.width)
			{
				Vector2 drawPos = projectile.Center + k * direction - Main.screenPosition;
				spriteBatch.Draw(texture, drawPos, null, color, trueRotation, origin, 1f, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}