using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.ChaosSpirit
{
	public class ChaosSpirit3 : ModNPC
	{
		private const int size = ChaosSpirit.size;

		public override void SetDefaults()
		{
			npc.name = "ChaosSpirit";
			npc.displayName = "Spirit of Chaos";
			npc.aiStyle = -1;
			npc.lifeMax = 200000;
			npc.damage = 0;
			npc.defense = 0;
			npc.knockBackResist = 0f;
			npc.takenDamageMultiplier = 2f;
			npc.width = size;
			npc.height = size;
			npc.value = Item.buyPrice(1, 0, 0, 0);
			npc.npcSlots = 100f;
			npc.boss = true;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = null;
			Main.npcFrameCount[npc.type] = 5;
			for (int k = 0; k < npc.buffImmune.Length; k++)
			{
				npc.buffImmune[k] = true;
			}
			NPCID.Sets.MustAlwaysDraw[npc.type] = true;
			music = MusicID.Title;
			bossBag = mod.ItemType("ChaosSpiritBag");
		}

		internal List<int> targets = new List<int>();
		private bool syncTargets = false;

		private int stage
		{
			get
			{
				return (int)npc.ai[0];
			}
			set
			{
				npc.ai[0] = value;
			}
		}

		private int timer
		{
			get
			{
				return (int)npc.ai[1];
			}
			set
			{
				npc.ai[1] = value;
			}
		}

		private int countdown
		{
			get
			{
				return (int)npc.ai[2];
			}
			set
			{
				npc.ai[2] = value;
			}
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax / Main.expertLife * 1.2f * bossLifeScale);
		}

		public override void AI()
		{
			Bluemagic.freezeHeroLives = false;
			FindPlayers();
			if (stage > 0 && targets.Count == 0)
			{
				timer = 0;
				stage = -1;
				npc.netUpdate = true;
			}
			int debuffType = mod.BuffType("ChaosPressure4");
			foreach (int target in targets)
			{
				Main.player[target].AddBuff(debuffType, 2, false);
			}
			switch (stage)
			{
				case -1:
					RunAway();
					break;
				case 0:
					Initialize();
					break;
				case 1:
					Countdown();
					break;
				case 2:
					EndAttack();
					break;
				case 10:
					FinishFight();
					break;
				default:
					break;
			}
			if (stage >= 0 && stage < 10)
			{
				CreateSuppressionSphere();
			}
			npc.timeLeft = NPC.activeTime;
		}

		public void FindPlayers()
		{
			if (Main.netMode != 1)
			{
				int originalCount = targets.Count;
				targets.Clear();
				for (int k = 0; k < 255; k++)
				{
					if (Main.player[k].active && Main.player[k].GetModPlayer<BluemagicPlayer>(mod).heroLives > 0)
					{
						targets.Add(k);
					}
				}
				if (Main.netMode == 2 && (syncTargets || targets.Count != originalCount))
				{
					ModPacket netMessage = GetPacket(ChaosSpiritMessageType.TargetList);
					netMessage.Write(targets.Count);
					foreach (int target in targets)
					{
						netMessage.Write(target);
					}
					netMessage.Send();
					syncTargets = false;
				}
			}
		}

		public void RunAway()
		{
			timer++;
			if (timer >= 360)
			{
				npc.active = false;
			}
		}

		private void Initialize()
		{
			countdown = 60 * 60;
			stage++;
			Talk("60 seconds until the end");
		}

		private void Countdown()
		{
			countdown--;
			if (countdown == 60 * 60 - 5)
			{
				syncTargets = true;
			}
			if (countdown == 60 * 45)
			{
				Talk("45 seconds until the end");
			}
			else if (countdown == 60 * 30)
			{
				Talk("30 seconds until the end");
			}
			else if (countdown == 60 * 20)
			{
				Talk("20 seconds until the end");
			}
			else if (countdown == 60 * 10)
			{
				Talk("10 seconds until the end");
			}
			else if (countdown == 60 * 5)
			{
				Talk("5");
			}
			else if (countdown == 60 * 4)
			{
				Talk("4");
			}
			else if (countdown == 60 * 3)
			{
				Talk("3");
			}
			else if (countdown == 60 * 2)
			{
				Talk("2");
			}
			else if (countdown == 60)
			{
				Talk("1");
			}
			else if (countdown <= 0)
			{
				countdown = 0;
				stage++;
				Talk("The air grows heavy with chaotic pressure");
				npc.netUpdate = true;
			}
		}

		private void EndAttack()
		{
			if (Main.netMode != 1 && countdown == 0)
			{
				int radius = Main.rand.Next(240, 320);
				float angle = Main.rand.NextFloat() * MathHelper.TwoPi;
				Vector2 offset = radius * angle.ToRotationVector2();
				Projectile.NewProjectile(npc.Center + offset, Vector2.Zero, mod.ProjectileType("HolySphere2"), 0, 0f, Main.myPlayer, npc.whoAmI);
			}
			countdown++;
			if (countdown == 140f)
			{
				PlaySound(29, 104);
			}
			if (countdown >= 180f)
			{
				countdown = 0;
				npc.netUpdate = true;
			}
		}

		public int RandomTarget()
		{
			if (targets.Count == 0)
			{
				return 255;
			}
			return targets[Main.rand.Next(targets.Count)];
		}

		private void CreateSuppressionSphere()
		{
			timer++;
			if (timer >= 60 || (stage < 2 && timer >= 30))
			{
				if (Main.netMode != 1)
				{
					Vector2 direction;
					if (Main.rand.Next(stage == 2 ? 6 : 3) == 0)
					{
						direction = Main.player[RandomTarget()].Center - npc.Center;
						direction.Normalize();
					}
					else
					{
						direction = (Main.rand.NextFloat() * MathHelper.TwoPi).ToRotationVector2();
					}
					Projectile.NewProjectile(npc.Center, 4f * direction, mod.ProjectileType("SuppressionSphere"), 0, 0f, Main.myPlayer, npc.whoAmI);
				}
				timer = 0;
			}
		}

		public override bool CheckDead()
		{
			if (stage == 10)
			{
				return true;
			}
			npc.active = true;
			npc.life = 1;
			npc.dontTakeDamage = true;
			stage = 10;
			timer = 0;
			npc.netUpdate = true;
			return false;
		}

		private void FinishFight()
		{
			if (timer == 0)
			{
				if (!Main.dedServ)
				{
					MoonlordDeathDrama.RequestLight(1f, npc.Center);
				}
				PlaySound(29, 92);
			}
			if (!Main.dedServ)
			{
				float x = Main.rand.Next(-Main.screenWidth / 2, Main.screenWidth / 2);
				float y = Main.rand.Next(-Main.screenHeight / 2, Main.screenHeight / 2);
				MoonlordDeathDrama.AddExplosion(npc.Center + new Vector2(x, y));
			}
			timer++;
			if (timer >= 300)
			{
				npc.dontTakeDamage = false;
				npc.HitSound = null;
				npc.takenDamageMultiplier = 1f;
				npc.StrikeNPCNoInteraction(9999, 0f, 0);
			}
		}

		public override bool? CanBeHitByItem(Player player, Item item)
		{
			return CanBeHitByPlayer(player);
		}

		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			return CanBeHitByPlayer(Main.player[projectile.owner]);
		}

		private bool? CanBeHitByPlayer(Player player)
		{
			if (!targets.Contains(player.whoAmI))
			{
				return false;
			}
			return null;
		}

		public override void NPCLoot()
		{
			int choice = Main.rand.Next(10);
			int item = 0;
			switch (choice)
			{
				case 0:
					item = mod.ItemType("ChaosTrophy");
					break;
				case 1:
					item = mod.ItemType("CataclysmTrophy");
					break;
			}
			if (item > 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, item);
			}
			if (Main.expertMode)
			{
				npc.DropBossBags();
			}
			else
			{
				choice = Main.rand.Next(7);
				if (choice == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ChaosSpiritMask"));
				}
				else if (choice == 1)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CataclysmMask"));
				}
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ChaosCrystal"));
			}
			BluemagicWorld.downedChaosSpirit = true;
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			name = "The " + npc.displayName;
			potionType = ItemID.SuperHealingPotion;
		}

		public override void FindFrame(int frameSize)
		{
			npc.frameCounter += 1.0;
			if (npc.frameCounter >= 6.0)
			{
				npc.frameCounter = 0.0;
				npc.frame.Y += frameSize;
				npc.frame.Y %= 5 * frameSize;
			}
		}

		public override Color? GetAlpha(Color drawColor)
		{
			return Color.White;
		}

		private void Talk(string message, byte r = 255, byte g = 255, byte b = 255)
		{
			if (Main.netMode == 0)
			{
				Main.NewText(message, r, g, b);
			}
			else if (Main.netMode == 2)
			{
				NetMessage.SendData(25, -1, -1, message, 255, r, g, b);
			}
		}

		private void PlaySound(int type, int style)
		{
			if (Main.netMode != 2)
			{
				if (targets.Contains(Main.myPlayer))
				{
					Main.PlaySound(type, -1, -1, style);
				}
				else
				{
					Main.PlaySound(type, (int)npc.position.X, (int)npc.position.Y, style);
				}
			}
		}

		private ModPacket GetPacket(ChaosSpiritMessageType type)
		{
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)MessageType.ChaosSpirit);
			packet.Write(npc.whoAmI);
			packet.Write((byte)type);
			return packet;
		}

		public void HandlePacket(BinaryReader reader)
		{
			ChaosSpiritMessageType type = (ChaosSpiritMessageType)reader.ReadByte();
			if (type == ChaosSpiritMessageType.HeroPlayer)
			{
				Player player = Main.player[Main.myPlayer];
				player.GetModPlayer<BluemagicPlayer>(mod).heroLives = reader.ReadInt32();
			}
			else if (type == ChaosSpiritMessageType.TargetList)
			{
				int numTargets = reader.ReadInt32();
				targets.Clear();
				for (int k = 0; k < numTargets; k++)
				{
					targets.Add(reader.ReadInt32());
				}
			}
			else if (type == ChaosSpiritMessageType.DeActivate)
			{
				npc.active = false;
			}
			else if (type == ChaosSpiritMessageType.PlaySound)
			{
				int soundType = reader.ReadInt32();
				int style = reader.ReadInt32();
				if (targets.Contains(Main.myPlayer))
				{
					Main.PlaySound(soundType, -1, -1, style);
				}
				else
				{
					Main.PlaySound(soundType, (int)npc.position.X, (int)npc.position.Y, style);
				}
			}
			else if (type == ChaosSpiritMessageType.Damage)
			{
			}
		}
	}
}