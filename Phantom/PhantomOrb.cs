using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Phantom
{
	public class PhantomOrb : ModNPC
	{
		public override void SetDefaults()
		{
			npc.name = "Phantom Orb";
			npc.aiStyle = -1;
			npc.lifeMax = 1000;
			npc.defense = 50;
			npc.knockBackResist = 0f;
			npc.width = 40;
			npc.height = 40;
			npc.alpha = 70;
			npc.npcSlots = 0f;
			npc.netAlways = true;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath6;
			for (int k = 0; k < npc.buffImmune.Length; k++)
			{
				npc.buffImmune[k] = true;
			}
		}

		public override void AI()
		{
			npc.rotation += 0.1f;
			if (npc.rotation > 2f * (float)Math.PI)
			{
				npc.rotation -= 2f * (float)Math.PI;
			}
			NPC follow = Main.npc[(int)npc.ai[1]];
			npc.Center = follow.Center + new Vector2(npc.ai[2], npc.ai[3]);
			npc.localAI[0] += 1f;
			npc.netUpdate = true;
			if (npc.localAI[0] >= 180f)
			{
				if (Main.netMode != 1 && npc.ai[0] == 1f)
				{
					
				}
				else if (Main.netMode != 1 && npc.ai[0] == -1f)
				{
					
				}
				else if (Main.netMode != 1)
				{
					
				}
				npc.active = false;
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for (int x = 0; x < 50; x++)
				{
					int dust = Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("Phantom"));
					Main.dust[dust].velocity *= 2f;
				}
			}
		}

		public override bool PreNPCLoot()
		{
			return false;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * 0.85f;
		}
	}
}