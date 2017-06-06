using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Phantom
{
	public class PhantomSoul : ModNPC
	{
		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.width = 16;
			npc.height = 16;
			npc.alpha = 100;
			npc.noTileCollide = true;
			npc.lifeMax = 100;
			npc.damage = 0;
			npc.defense = 100;
			npc.knockBackResist = 0f;
			npc.npcSlots = 12f;
			npc.dontTakeDamage = true;
			npc.noGravity = true;
			music = MusicID.Boss3;
		}

		public override void AI()
		{
			Player player = Main.player[npc.target];
			if (npc.ai[0] < 200f)
			{
				npc.Center = player.Center;
			}
			else if (npc.ai[0] < 600f)
			{
				npc.Center = player.Center - new Vector2(0f, (npc.ai[0] - 200f) * 0.625f);
			}
			else
			{
				npc.Center = player.Center - new Vector2(0f, 250f);
			}
			if (npc.ai[0] < 660f)
			{
				npc.ai[1] = 0f;
			}
			else if (npc.ai[0] < 720f)
			{
				npc.ai[1] = (npc.ai[0] - 660f) / 60f * 0.6f;
			}
			else
			{
				npc.ai[1] = 0.6f;
			}
			if (npc.ai[0] < 750f)
			{
				npc.ai[2] = 1f;
			}
			else
			{
				npc.ai[2] = 1f + (npc.ai[0] - 750f) / 50f;
			}

			int num = 3;
			bool flag = npc.ai[0] == 899f;
			if (npc.ai[0] < 120)
			{
				num = 1;
			}
			if (flag)
			{
				num = 200;
			}
			for (int k = 0; k < num; k++)
			{
				int dust = Dust.NewDust(npc.position, 16, 16, mod.DustType("Phantom"));
				if (flag)
				{
					Main.dust[dust].velocity *= (Main.rand.Next(3) + 1);
				}
			}
			npc.ai[0] += 1f;
			if (npc.ai[0] >= 900)
			{
				npc.active = false;
				if (Main.netMode != 1)
				{
					NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y + 40, mod.NPCType("Phantom"), 0, 0f, 0f, 0f, 0f, npc.target);
				}
			}
			npc.rotation += 0.1f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.npcTexture[npc.type];
			spriteBatch.Draw(texture, npc.position - Main.screenPosition, Color.White * 0.8f);
			texture = mod.GetTexture("Phantom/PhantomSoulAura");
			float alpha = npc.ai[1];
			float scale = npc.ai[2];
			spriteBatch.Draw(texture, npc.Center - Main.screenPosition, null, Color.White * alpha, npc.rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}