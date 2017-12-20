using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.ChaosSpirit
{
	public class ChaosSpiritArm : ModNPC
	{
		private const int size = ChaosSpirit.size;
		public const float armLength = ChaosSpirit2.armLength;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit of Chaos - Arm");
			NPCID.Sets.MustAlwaysDraw[npc.type] = true;
		}

		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 400000;
			npc.damage = 200;
			npc.defense = 0;
			npc.knockBackResist = 0f;
			npc.dontTakeDamage = false;
			npc.immortal = true;
			npc.width = size;
			npc.height = size;
			npc.npcSlots = 10f;
			npc.boss = true;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = null;
			npc.alpha = 255;
			for (int k = 0; k < npc.buffImmune.Length; k++)
			{
				npc.buffImmune[k] = true;
			}
			music = MusicID.LunarBoss;
		}

		private List<ChaosOrb> orbs = new List<ChaosOrb>();

		private int spiritIndex
		{
			get
			{
				return (int)npc.ai[0];
			}
		}

		private int colorType
		{
			get
			{
				return (int)npc.ai[1];
			}
		}

		internal Vector2 offset
		{
			get
			{
				return new Vector2(npc.ai[2], npc.ai[3]);
			}
			set
			{
				npc.ai[2] = value.X;
				npc.ai[3] = value.Y;
			}
		}

		private int attack
		{
			get
			{
				return (int)npc.localAI[1];
			}
			set
			{
				npc.localAI[1] = value;
			}
		}

		private int attackTimer
		{
			get
			{
				return (int)npc.localAI[2];
			}
			set
			{
				npc.localAI[2] = value;
			}
		}

		private int attackDelay
		{
			get
			{
				return (int)npc.localAI[3];
			}
			set
			{
				npc.localAI[3] = value;
			}
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax / Main.expertLife * 1.2f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.75f);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(npc.localAI[1]);
			writer.Write(npc.localAI[2]);
			writer.Write(npc.localAI[3]);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			npc.localAI[1] = reader.ReadSingle();
			npc.localAI[2] = reader.ReadSingle();
			npc.localAI[3] = reader.ReadSingle();
		}

		public override void AI()
		{
			if (!Main.dedServ)
			{
				UpdateChaosOrbs();
			}
			if (npc.localAI[0] == 0f)
			{
				npc.GivenName = GetName();
				npc.localAI[0] = 1f;
			}
			npc.timeLeft = NPC.activeTime;
			npc.life = npc.lifeMax;
			NPC center = Main.npc[spiritIndex];
			if (!center.active || center.type != mod.NPCType("ChaosSpirit2"))
			{
				npc.active = false;
				return;
			}
			ChaosSpirit2 spirit = center.modNPC as ChaosSpirit2;
			if (spirit == null)
			{
				return;
			}
			float rotation = spirit.armRotation + (colorType / 6f) * MathHelper.TwoPi;
			Vector2 target = armLength * rotation.ToRotationVector2();
			Vector2 move = target - offset;
			if (move.Length() > 4f)
			{
				move.Normalize();
				move *= 2f;
			}
			offset += move;
			npc.Center = center.Center + offset;
			DoAttack();
		}

		private void UpdateChaosOrbs()
		{
			foreach (ChaosOrb orb in orbs)
			{
				orb.Update();
			}
			float x = size * Main.rand.NextFloat();
			float y = size * Main.rand.NextFloat();
			Vector2 velocity = new Vector2();
			if (attack == 6)
			{
				velocity.X = size / 2 - x;
				velocity.Y = size / 2 - y;
				velocity /= 50f;
				Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("Sparkle"), 0f, 0f, 0, GetOrbColor(), 1f);
			}
			else
			{
				velocity.X = Main.rand.NextFloat() * 2f - 1f;
				velocity.Y = -1f;
			}
			for (int k = 0; k < 5; k++)
			{
				orbs.Add(new ChaosOrb(new Vector2(x, y), velocity, GetOrbColor()));
			}
			Vector2 offset = Main.npc[spiritIndex].Center - npc.Center;
			offset *= Main.rand.NextFloat();
			offset += new Vector2(npc.width / 2 + Main.rand.Next(-8, 9), npc.height / 2 + Main.rand.Next(-8, 9));
			orbs.Add(new ChaosOrb(offset, Vector2.Zero, GetArmOrbColor()));
			while (orbs[0].strength <= 0f)
			{
				orbs.RemoveAt(0);
			}
		}

		public static Color GetColor(int type)
		{
			switch (type)
			{
			case 0:
			default:
				return new Color(255, 0, 0);
			case 1:
				return new Color(255, 255, 0);
			case 2:
				return new Color(0, 255, 0);
			case 3:
				return new Color(0, 255, 255);
			case 4:
				return new Color(0, 0, 255);
			case 5:
				return new Color(255, 0, 255);
			}
		}

		private Color GetOrbColor()
		{
			return GetColor(colorType);
		}

		private string GetName()
		{
			string color;
			switch (colorType)
			{
			case 0:
			default:
				color = "Red";
				break;
			case 1:
				color = "Yellow";
				break;
			case 2:
				color = "Green";
				break;
			case 3:
				color = "Cyan";
				break;
			case 4:
				color = "Blue";
				break;
			case 5:
				color = "Purple";
				break;
			}
			return "Spirit of Chaos - " + color + " Arm";
		}

		private Color GetArmOrbColor()
		{
			if (Main.rand.Next(2) == 0)
			{
				return ChaosSpirit.mainColor;
			}
			else
			{
				return GetOrbColor();
			}
		}

		private void DoAttack()
		{
			if (attackDelay > 0)
			{
				attackDelay--;
				return;
			}
			if (attack == 1 || attack == 3 || attack == 5)
			{
				ChaosBitAttack();
			}
			else if (attack == 2 || attack == 4)
			{
				ChaosPearlAttack();
			}
			else if (attack == 6)
			{
				LaserAttack();
			}
		}

		private void ChaosBitAttack()
		{
			if (Main.netMode != 1 && attackTimer % 30 == 0 && attackTimer < 90)
			{
				const int numBits = 10;
				int damage = 100;
				if (Main.expertMode)
				{
					damage = (int)(damage * 1.5f / 2f);
				}
				for (int k = 0; k < numBits; k++)
				{
					float rotation = ((float)k / (float)numBits) * MathHelper.TwoPi;
					if (attackTimer % 60 == 30)
					{
						rotation += ((k + 0.5f) / numBits) * MathHelper.TwoPi;
					}
					Projectile.NewProjectile(npc.Center, 8f * rotation.ToRotationVector2(), mod.ProjectileType("ChaosBit"), damage, 0f, Main.myPlayer, colorType);
				}
			}
			attackTimer++;
			if (attackTimer > 90)
			{
				attack = 0;
				attackTimer = 0;
			}
		}

		private void ChaosPearlAttack()
		{
			if (Main.netMode != 1 && attackTimer % 30 == 0 && attackTimer < 90)
			{
				ChaosSpirit2 spirit = Main.npc[spiritIndex].modNPC as ChaosSpirit2;
				Player player = Main.player[spirit.RandomTarget()];
				Vector2 difference = player.Center - npc.Center;
				if (difference != Vector2.Zero)
				{
					difference.Normalize();
					int damage = 100;
					if (Main.expertMode)
					{
						damage = (int)(damage * 1.5f / 2f);
					}
					Projectile.NewProjectile(npc.Center, 8f * difference, mod.ProjectileType("ChaosPearl"), damage, 0f, Main.myPlayer, colorType, player.whoAmI);
				}
			}
			attackTimer++;
			if (attackTimer > 90)
			{
				attack = 0;
				attackTimer = 0;
			}
		}

		private void LaserAttack()
		{
			attackTimer++;
			if (attackTimer == 300 && Main.netMode != 1)
			{
				ChaosSpirit2 spirit = Main.npc[spiritIndex].modNPC as ChaosSpirit2;
				float rotation = spirit.armRotation + (colorType / 6f) * MathHelper.TwoPi;
				float startRot = rotation - MathHelper.Pi / 3f;
				float endRot = rotation + MathHelper.Pi / 3f;
				if (Main.rand.Next(2) == 0)
				{
					float temp = startRot;
					startRot = endRot;
					endRot = temp;
				}
				int damage = 360;
				if (Main.expertMode)
				{
					damage = (int)(damage * 1.5f / 2f);
				}
				int proj = Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("ChaosRay"), damage, 0f, Main.myPlayer, npc.whoAmI, startRot);
				Main.projectile[proj].localAI[0] = endRot;
				NetMessage.SendData(27, -1, -1, null, proj);
			}
			if (attackTimer == 300)
			{
				Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 104);
			}
			if (attackTimer > 540)
			{
				attack = 0;
				attackTimer = 0;
			}
		}

		public override bool CheckDead()
		{
			npc.active = true;
			npc.life = npc.lifeMax;
			return false;
		}

		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			BluemagicPlayer modPlayer = target.GetModPlayer<BluemagicPlayer>(mod);
			modPlayer.constantDamage = npc.damage;
			modPlayer.percentDamage = 1f / 3f;
			if (Main.expertMode)
			{
				modPlayer.percentDamage *= 1.5f;
			}
			modPlayer.chaosDefense = true;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(mod.BuffType("Undead"), 300, false);
		}

		public override bool? CanBeHitByItem(Player player, Item item)
		{
			return CanBeHitByPlayer(player);
		}

		public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{
			OnHit(player, damage);
		}

		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			return CanBeHitByPlayer(Main.player[projectile.owner]);
		}

		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			OnHit(Main.player[projectile.owner], damage);
		}

		private bool? CanBeHitByPlayer(Player player)
		{
			ChaosSpirit2 spirit = Main.npc[spiritIndex].modNPC as ChaosSpirit2;
			if (spirit != null && !spirit.targets.Contains(player.whoAmI))
			{
				return false;
			}
			return null;
		}

		private void OnHit(Player player, int damage)
		{
			if (player.active)
			{
				Vector2 direction = npc.Center - player.Center;
				if (direction != Vector2.Zero)
				{
					float magnitude = damage / 200f;
					if (magnitude > 1f)
					{
						magnitude = 1f;
					}
					magnitude *= 64f;
					direction.Normalize();
					direction *= magnitude;
					offset += direction;
					if (Main.netMode != 0)
					{
						ModPacket packet = mod.GetPacket();
						packet.Write((byte)MessageType.PushChaosArm);
						packet.Write(npc.whoAmI);
						packet.Write(direction.X);
						packet.Write(direction.Y);
						packet.Send();
					}
				}
			}
			npc.life = npc.lifeMax;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			foreach (ChaosOrb orb in orbs)
			{
				orb.Draw(spriteBatch, npc.position, mod);
			}
			return false;
		}
	}
}