using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Phantom
{
	public class Phantom : ModNPC
	{
		public override void SetDefaults()
		{
			npc.name = "Phantom";
			npc.displayName = "The Phantom";
			npc.aiStyle = -1;
			npc.lifeMax = 50000;
			npc.damage = 120;
			npc.defense = 50;
			npc.knockBackResist = 0f;
			npc.width = 80;
			npc.height = 80;
			npc.alpha = 70;
			npc.value = Item.buyPrice(0, 15, 0, 0);
			npc.npcSlots = 12f;
			npc.boss = true;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath6;
			for (int k = 0; k < npc.buffImmune.Length; k++)
			{
				npc.buffImmune[k] = true;
			}
			music = MusicID.Boss3;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.7f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.7f);
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			scale = 1.5f;
			return null;
		}

		public bool Enraged
		{
			get
			{
				return npc.ai[0] != 0f;
			}
			set
			{
				npc.ai[0] = value ? 1f : 0f;
			}
		}

		public float AttackID
		{
			get
			{
				return npc.ai[2];
			}
			set
			{
				npc.ai[2] = value;
			}
		}

		public float AttackTimer
		{
			get
			{
				return npc.ai[3];
			}
			set
			{
				npc.ai[3] = value;
			}
		}

		public float MaxAttackTimer
		{
			get
			{
				return 60f * (float)npc.life / (float)npc.lifeMax;
			}
		}

		public float PaladinTimer
		{
			get
			{
				return npc.localAI[1];
			}
			set
			{
				npc.localAI[1] = value;
			}
		}

		public float MaxPaladinTimer
		{
			get
			{
				float maxValue = Main.expertMode ? 2f / 3f : 0.5f;
				return 120f + 180f * (float)npc.life / (npc.lifeMax * maxValue);
			}
		}

		public override void AI()
		{
			Initialize();

			if (Main.netMode != 1 && !Enraged && (!npc.HasValidTarget || !Main.player[npc.target].ZoneDungeon))
			{
				Enraged = true;
				npc.netUpdate = true;
				Talk("You thought you could escape...");
			}
			if (Enranged)
			{
				
			}

			if (AttackID == 1f || AttackID == 2f)
			{
				ChargeAttack();
			}
			else if (AttackID == 3f)
			{
				SphereAttack();
			}
			AttackTimer += 1f;
			if (AttackTimer >= MaxAttackTimer)
			{
				ChooseAttack();
			}

			if (Main.netMode != 1 && (npc.life <= npc.lifeMax / 2 || (Main.expertMode && npc.lifeMax * 2 / 3)))
			{
				PaladinTimer += 1f;
				if (PaladinTimer >= MaxPaladinTimer)
				{
					SpawnPaladin();
					PaladinTimer = 0f;
					npc.netUpdate = true;
				}
			}
		}

		private void Initialize()
		{
			if (Main.netMode != 1 && npc.localAI[0] == 0f)
			{
				npc.localAI[0] = 1f;
				int spawnX = (int)npc.Bottom.X;
				int spawnY = (int)npc.Bottom.Y + 64;
				npc.ai[0] = NPC.NewNPC(spawnX - 128, spawnY, mod.NPCType("PhantomHand"), 0, npc.whoAmI, -1f, 0f, -30f);
				npc.ai[1] = NPC.NewNPC(spawnX + 128, spawnY, mod.NPCType("PhantomHand"), 0, npc.whoAmI, 1f, 0f, -60f);
				npc.npetUpdate = true;
				LeftHand.npc.netUpdate = true;
				RightHand.npc.netUpdate = true;
			}
		}

		private void ChooseAttack()
		{
			AttackID += 1f;
			if (AttackID >= 4f)
			{
				AttackID = 1f;
			}
			if (AttackID == 3f)
			{
				AttackTimer = -120f;
			}
			else
			{
				AttackTimer = -120f;
			}
			npc.TargetClosest(false);
			npc.netUpdate = true;
		}

		private void ChargeAttack()
		{
			
		}

		private void SphereAttack()
		{
			
		}

		private void SpawnPaladin()
		{
			
		}

		private void Talk(string message)
		{
			message = "<" + npc.displayName + "> " + message;
			if (Main.netMode == 0)
			{
				Main.NewText(message, 50, 150, 200);
			}
			else
			{
				NetMessage.SendData(MessageID.ChatText, -1, -1, message, 255, 50, 150, 200);
			}
		}
	}
}