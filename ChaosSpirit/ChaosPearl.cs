using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.ChaosSpirit
{
	public class ChaosPearl : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.name = "Chaos Pearl";
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
			if (projectile.localAI[0] < 180f)
			{
				Player player = Main.player[(int)projectile.ai[1]];
				Vector2 offset = player.Center - projectile.Center;
				if (offset != Vector2.Zero)
				{
					float strength = (180f - projectile.ai[1]) / 180f;
					float direction = projectile.velocity.ToRotation();
					float target = offset.ToRotation();
					if (target > direction + MathHelper.Pi)
					{
						target -= MathHelper.TwoPi;
					}
					else if (target < direction - MathHelper.Pi)
					{
						target += MathHelper.TwoPi;
					}
					float difference = (target - direction) * 0.5f * strength;
					projectile.velocity = projectile.velocity.Length() * (direction + difference).ToRotationVector2();
				}
			}
			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] > 600f)
			{
				projectile.Kill();
			}
			projectile.localAI[1] += 1f;
			projectile.localAI[1] %= 30f;
		}

		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			if (target.hurtCooldowns[1] <= 0)
			{
				BluemagicPlayer modPlayer = target.GetModPlayer<BluemagicPlayer>(mod);
				modPlayer.constantDamage = projectile.damage;
				modPlayer.percentDamage = 0.125f;
				if (Main.expertMode)
				{
					modPlayer.percentDamage *= 1.5f;
				}
				modPlayer.chaosDefense = true;
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(mod.BuffType("Undead"), 150, false);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			Color color = ChaosSpiritArm.GetColor((int)projectile.ai[0]);
			//color.R = (byte)((255 + color.R) / 2);
			//color.G = (byte)((255 + color.G) / 2);
			//color.B = (byte)((255 + color.B) / 2);
			return color;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float alpha = 0.25f + Math.Abs(projectile.localAI[1] - 15f) / 30f;
			spriteBatch.Draw(mod.GetTexture("ChaosSpirit/ChaosBitMask"), projectile.position - Main.screenPosition, Color.White * alpha);
		}
	}
}