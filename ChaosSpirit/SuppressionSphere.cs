using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.ChaosSpirit
{
	public class SuppressionSphere : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 120;
			projectile.height = 120;
			projectile.penetrate = -1;
			projectile.magic = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}

		private int timer = 0;

		public override void AI()
		{
			NPC npc = Main.npc[(int)projectile.ai[0]];
			if (!npc.active || npc.type != mod.NPCType("ChaosSpirit3"))
			{
				projectile.Kill();
				return;
			}
			for (int k = 0; k < 255; k++)
			{
				Player player = Main.player[k];
				if (player.active && !player.dead && Ellipse.Collides(projectile.position, new Vector2(projectile.width, projectile.height), player.position, new Vector2(player.width, player.height)))
				{
					bool flag = true;
					for (int i = 0; i < Player.maxBuffs; i++)
					{
						if (player.buffType[i] == mod.BuffType("Suppression1"))
						{
							player.buffType[i] = mod.BuffType("Suppression2");
							flag = false;
						}
						else if (player.buffType[i] == mod.BuffType("Suppression2"))
						{
							player.buffType[i] = mod.BuffType("Suppression3");
							flag = false;
						}
						else if (player.buffType[i] == mod.BuffType("Suppression3"))
						{
							player.buffType[i] = mod.BuffType("Suppression4");
							flag = false;
						}
						else if (player.buffType[i] == mod.BuffType("Suppression4"))
						{
							player.buffTime[i] = 300;
							flag = false;
						}
					}
					if (flag)
					{
						player.AddBuff(mod.BuffType("Suppression1"), 300);
					}
					for (int i = 0; i < 20; i++)
					{
						Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("Sparkle"), 0f, 0f, 0, Color.Red, 1.1f);
					}
					projectile.Kill();
				}
			}
			projectile.ai[1] += 1f;
			if (projectile.ai[1] >= 600f)
			{
				projectile.Kill();
			}
			projectile.frameCounter++;
			if (projectile.frameCounter >= 10)
			{
				projectile.frameCounter = 0;
				projectile.frame++;
				projectile.frame %= 3;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.position - Main.screenPosition, null, Color.White * 0.8f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Rectangle frame = new Rectangle(0, 120 * projectile.frame, 120, 120);
			spriteBatch.Draw(mod.GetTexture("ChaosSpirit/SuppressionSphereBorder"), projectile.position - Main.screenPosition, frame, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			return false;
		}
	}
}