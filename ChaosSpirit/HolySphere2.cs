using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.ChaosSpirit
{
	public class HolySphere2 : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 48;
			projectile.height = 48;
			projectile.penetrate = -1;
			projectile.magic = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}

		public override void AI()
		{
			NPC npc = Main.npc[(int)projectile.ai[0]];
			if (!npc.active || npc.type != mod.NPCType("ChaosSpirit3"))
			{
				projectile.Kill();
				return;
			}
			projectile.ai[1] += 1f;
			if (projectile.ai[1] == 180f)
			{
				if (npc.modNPC is ChaosSpirit3 && ((ChaosSpirit3)npc.modNPC).targets.Contains(Main.myPlayer))
				{
					Player player = Main.player[Main.myPlayer];
					if (!Ellipse.Collides(projectile.position, new Vector2(projectile.width, projectile.height), player.position, new Vector2(player.width, player.height)))
					{
						player.GetModPlayer<BluemagicPlayer>().ChaosKill();
						player.AddBuff(mod.BuffType("Undead"), 300, false);
					}
				}
				if (Main.netMode != 2)
				{
					projectile.Kill();
				}
			}
			if (Main.netMode == 2 && projectile.ai[1] >= 240f)
			{
				projectile.Kill();
			}

			if (Main.rand.Next(4) == 0)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("Sparkle"), 0f, 0f, 0, default(Color), 1f);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.position - Main.screenPosition, null, Color.White * 0.8f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(mod.GetTexture("ChaosSpirit/HolySphereBorder2"), projectile.position - Main.screenPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			if (projectile.ai[1] > 140f)
			{
				spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * ((projectile.ai[1] - 140f) / 40f));
			}
			return false;
		}
	}
}