using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Phantom
{
	public class PhantomHand : ModNPC
	{
		private const float maxSpeed = 8f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Phantom");
		}

		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 50000;
			npc.damage = 100;
			npc.defense = 50;
			npc.knockBackResist = 0f;
			npc.dontTakeDamage = true;
			npc.width = 32;
			npc.height = 40;
			npc.alpha = 70;
			npc.value = Item.buyPrice(0, 15, 0, 0);
			npc.npcSlots = 0f;
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
				return (Phantom)Main.npc[(int)npc.ai[0]].modNPC;
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

		public float MaxAttackTimer
		{
			get
			{
				return 60f + 120f * (float)Head.npc.life / (float)Head.npc.lifeMax;
			}
		}

		public override void AI()
		{
			NPC headNPC = Main.npc[(int)npc.ai[0]];
			if (!headNPC.active || headNPC.type != mod.NPCType("Phantom"))
			{
				npc.active = false;
				return;
			}
			headNPC.timeLeft = headNPC.timeLeft;

			if (Head.Enraged)
			{
				npc.damage = npc.defDamage * 3;
				npc.defense = npc.defDefense * 3;
			}
			npc.direction = (int)Direction;
			npc.spriteDirection = (int)Direction;
			if (!npc.HasValidTarget)
			{
				npc.TargetClosest(false);
			}

			if (AttackTimer >= 0f)
			{
				IdleBehavior();
			}
			else if (AttackID == 1 || AttackID == 4)
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
			else
			{
				IdleBehavior();
			}
			AttackTimer += 1f;
			if (AttackTimer >= MaxAttackTimer)
			{
				ChooseAttack();
			}

			CreateDust();
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
					AttackTimer = -210f;
				}
				else
				{
					AttackTimer = -240f;
				}
			}
			else if (AttackID == 2f || AttackID == 5f)
			{
				if (Direction == -1f)
				{
					AttackTimer = -240f;
				}
				else
				{
					AttackTimer = -210f;
				}
			}
			else if (AttackID == 3f)
			{
				AttackTimer = -60f;
			}
			npc.TargetClosest(false);
			npc.netUpdate = true;
		}

		private void IdleBehavior()
		{
			Vector2 target = Head.npc.Bottom + new Vector2(Direction * 128f, 64f);
			Vector2 change = target - npc.Bottom;
			CapVelocity(ref change, maxSpeed * 2f);
			ModifyVelocity(change);
			CapVelocity(ref npc.velocity, maxSpeed * 2f);
		}

		private void HammerAttack()
		{
			Vector2 target = Main.player[npc.target].Center;
			Vector2 moveTarget = target + new Vector2(Direction * 240f, -240f);
			Vector2 offset = moveTarget - npc.Center;
			CapVelocity(ref offset, maxSpeed);
			ModifyVelocity(offset);
			CapVelocity(ref npc.velocity, maxSpeed);

			int attackTimer = (int)AttackTimer + 210;
			if (attackTimer % 20 == 0 && attackTimer < 100 && Main.netMode != 1)
			{
				int damage = (npc.damage - 20) / 2;
				if (Main.expertMode)
				{
					damage /= 2;
				}
				Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("PhantomHammer"), damage, 6f, Main.myPlayer, npc.whoAmI);
			}
		}

		private void BladeAttack()
		{
			Vector2 target = Main.player[npc.target].Center;
			Vector2 moveTarget = target + new Vector2(Direction * 240f, 0f);
			Vector2 offset = moveTarget - npc.Center;
			CapVelocity(ref offset, maxSpeed);
			ModifyVelocity(offset);
			CapVelocity(ref npc.velocity, maxSpeed);

			if (AttackTimer == -240f && Main.netMode != 1)
			{
				NPC.NewNPC((int)npc.Bottom.X, (int)npc.Bottom.Y, mod.NPCType("PhantomOrb"), 0, 2f, npc.whoAmI, 0f, 0f, npc.target);
			}
		}

		private void WispAttack()
		{
			Vector2 target = Main.player[npc.target].Center;
			Vector2 moveTarget = target + new Vector2(Direction * 240f, 0f);
			Vector2 offset = moveTarget - npc.Center;
			CapVelocity(ref offset, maxSpeed);
			ModifyVelocity(offset);
			CapVelocity(ref npc.velocity, maxSpeed);

			if (AttackTimer == -240f && Main.netMode != 1)
			{
				NPC.NewNPC((int)npc.Bottom.X, (int)npc.Bottom.Y, mod.NPCType("PhantomOrb"), 0, 1f, npc.whoAmI, 0f, 0f, npc.target);
			}
		}

		private void ChargeAttack()
		{
			if (AttackTimer < -40f)
			{
				Vector2 offset = Main.player[npc.target].Center - npc.Center;
				CapVelocity(ref offset, maxSpeed);
				ModifyVelocity(offset, 0.1f);
				CapVelocity(ref npc.velocity, maxSpeed);
			}
		}

		private void CreateDust()
		{
			Vector2 target = Head.npc.Center;
			target += new Vector2(Direction * 60f, 60f);
			Vector2 offset = target - npc.Center;
			float length = offset.Length();
			if (offset != Vector2.Zero)
			{
				offset.Normalize();
			}
			for (float k = 0f; k < length - 10f; k += 4f)
			{
				if (Main.rand.Next(10) == 0)
				{
					int dust = Dust.NewDust(npc.Center + offset * k, 0, 0, mod.DustType("Phantom"));
					Main.dust[dust].alpha = 100;
				}
			}
		}

		private void ModifyVelocity(Vector2 modify, float weight = 0.05f)
		{
			npc.velocity = Vector2.Lerp(npc.velocity, modify, weight);
		}

		private void CapVelocity(ref Vector2 velocity, float max)
		{
			if (velocity.Length() > max)
			{
				velocity.Normalize();
				velocity *= max;
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * 0.8f;
		}
	}
}