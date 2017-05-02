using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Phantom
{
	public class PhantomHand : ModNPC
	{
		public override void SetDefaults()
		{
			npc.name = "Phantom Hand";
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
			npc.npcSlots = 0f;
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
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.7f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.7f);
		}

		public Phantom Head
		{
			get
			{
				return (Phantom)Main.npc[(int)npc.ai[0]];
			}
		}

		public float Direction
		{
			get
			{
				return npc.ai[1];
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

		public override void AI()
		{
			if (Head.Enraged)
			{
				
			}

			if (AttackID == 1 || AttackID == 4)
			{
				if (Direction == -1f)
				{
					HammerAttack();
				}
				else
				{
					WispAttack();
				}
			}
			else if (AttackID == 2 || AttackID == 5)
			{
				if (Direction == -1f)
				{
					BladeAttack();
				}
				else
				{
					HammerAttack();
				}
			}
			else if (AttackID == 3)
			{
				ChargeAttack();
			}
			AttackTimer += 1f;
			if (AttackTimer >= MaxAttackTimer)
			{
				ChooseAttack();
			}
		}

		private void ChooseAttack()
		{
			AttackID += 1f;
			if (AttackID >= 6f)
			{
				AttackID = 1f;
			}
			if (AttackID == 1f || AttackID == 4f)
			{
				if (Direction == -1f)
				{
					AttackTimer = -120f;
				}
				else
				{
					AttackTimer = -120f;
				}
			}
			else if (AttackID == 2f || AttackID == 5f)
			{
				if (Direction == -1f)
				{
					AttackTimer = -120f;
				}
				else
				{
					AttackTimer = -120f;
				}
			}
			else if (AttackID == 3f)
			{
				AttackTimer = -60f;
			}
			npc.TargetClosest(false);
			npc.netUpdate = true;
		}

		private void HammerAttack()
		{
			
		}

		private void BladeAttack()
		{
			
		}

		private void WispAttack()
		{
			
		}

		private void ChargeAttack()
		{
			
		}
	}
}