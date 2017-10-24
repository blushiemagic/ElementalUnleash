using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.BlushieBoss
{
	public abstract class BlushiemagicBase : ModNPC
	{
		public override void SetStaticDefaults()
		{
			NPCID.Sets.NeedsExpertScaling[npc.type] = false;
		}

		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 1000000;
			npc.damage = 0;
			npc.defense = 0;
			npc.knockBackResist = 0f;
			npc.dontTakeDamage = true;
			npc.width = 24;
			npc.height = 48;
			npc.npcSlots = 9001f;
			npc.boss = true;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = null;
			for (int k = 0; k < npc.buffImmune.Length; k++)
			{
				npc.buffImmune[k] = true;
			}
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			scale = 1.5f;
			return null;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}