using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.ChaosSpirit
{
	public class ChaosBit : ModProjectile
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
			projectile.ai[1] += 1f;
			if (projectile.ai[1] > 600f)
			{
				projectile.Kill();
			}
			projectile.localAI[0] += 1f;
			projectile.localAI[0] %= 30f;
		}

		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			if (target.hurtCooldowns[1] <= 0)
			{
				BluemagicPlayer modPlayer = target.GetModPlayer<BluemagicPlayer>(mod);
				modPlayer.constantDamage = 100;
				modPlayer.percentDamage = 1f / 6f;
				if (Main.expertMode)
				{
					modPlayer.constantDamage *= 2;
					modPlayer.percentDamage *= 2f;
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
			float alpha = 0.25f + Math.Abs(projectile.localAI[0] - 15f) / 30f;
			spriteBatch.Draw(mod.GetTexture("ChaosSpirit/ChaosBitMask"), projectile.position - Main.screenPosition, Color.White * alpha);
		}
	}
}