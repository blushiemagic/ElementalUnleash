using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using System.Reflection;
using Bluemagic.PuritySpirit;

namespace Bluemagic.TerraSpirit
{
	public class TerraSpirit : ModNPC
	{
		private const int size = 120;
		private const int particleSize = 12;
		public const int arenaWidth = 2400;
		public const int arenaHeight = 1600;

		internal int Stage
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

		internal int Progress
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

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit of Purity");
			NPCID.Sets.MustAlwaysDraw[npc.type] = true;
			NPCID.Sets.NeedsExpertScaling[npc.type] = false;
		}

		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 750000;
			npc.damage = 0;
			npc.defense = 140;
			npc.knockBackResist = 0f;
			npc.width = size;
			npc.height = size;
			npc.npcSlots = 1337f;
			npc.boss = true;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.chaseable = false;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = null;
			npc.alpha = 255;
			for (int k = 0; k < npc.buffImmune.Length; k++)
			{
				npc.buffImmune[k] = true;
			}
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Despicable beautiful");
		}

		private IList<Particle> particles = new List<Particle>();
		private float[,] aura = new float[size, size];
		internal List<Bullet> bullets = new List<Bullet>();

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			scale = 2f;
			return null;
		}

		public override void AI()
		{
			int numPlayers = CountPlayers();
			if (!Main.dedServ)
			{
				UpdateParticles();
			}
			npc.timeLeft = NPC.activeTime;
			if (Stage % 2 > 0 && numPlayers == 0)
			{
				Stage = -1;
				Progress = 0;
			}
			if (Stage == -1)
			{
				RunAway();
			}
			else if (Stage % 2 == 0)
			{
				Initialize();
			}
			else if (Stage == 1)
			{
				Stage1();
			}
			else if (Stage == 3)
			{
				Stage2();
			}
			else if (Stage == 5)
			{
				Stage3();
			}
			else if (Stage == 7)
			{
				Stage4();
			}
			else if (Stage == 9)
			{
				Stage5();
			}
			else if (Stage == 11)
			{
				End();
			}
			FixLife();
			Progress++;
			Rectangle bounds = new Rectangle((int)npc.Center.X - arenaWidth / 2, (int)npc.Center.Y - arenaHeight / 2, arenaWidth, arenaHeight);
			for (int k = 0; k < bullets.Count; k++)
			{
				if (bullets[k].Update(this, bounds))
				{
					Player player = Main.player[Main.myPlayer];
					if (player.active && !player.dead && player.GetModPlayer<BluemagicPlayer>().terraLives > 0 && bullets[k].Collides(player.Hitbox))
					{
						player.GetModPlayer<BluemagicPlayer>().TerraKill();
					}
				}
				else
				{
					bullets.RemoveAt(k);
					k--;
				}
			}
		}

		private int CountPlayers()
		{
			int count = 0;
			for (int k = 0; k < 255; k++)
			{
				Player player = Main.player[k];
				if (player.active && !player.dead && player.GetModPlayer<BluemagicPlayer>().terraLives > 0)
				{
					count++;
				}
			}
			return count;
		}

		public Player GetTarget()
		{
			Player player = Main.player[Main.myPlayer];
			if (!player.active || player.dead || player.GetModPlayer<BluemagicPlayer>().terraLives <= 0)
			{
				for (int k = 0; k < 255; k++)
				{
					if (Main.player[k].active && !Main.player[k].dead && Main.player[k].GetModPlayer<BluemagicPlayer>().terraLives > 0)
					{
						player = Main.player[k];
						break;
					}
				}
			}
			return player;
		}

		private void UpdateParticles()
		{
			foreach (Particle particle in particles)
			{
				particle.Update();
			}
			Vector2 newPos = new Vector2(Main.rand.Next(3 * size / 8, 5 * size / 8), Main.rand.Next(3 * size / 8, 5 * size / 8));
			double newAngle = 2 * Math.PI * Main.rand.NextDouble();
			Vector2 newVel = new Vector2((float)Math.Cos(newAngle), (float)Math.Sin(newAngle));
			newVel *= 0.5f * (1f + (float)Main.rand.NextDouble());
			particles.Add(new Particle(newPos, newVel));
			if (particles[0].strength <= 0f)
			{
				particles.RemoveAt(0);
			}
			for (int x = 0; x < size; x++)
			{
				for (int y = 0; y < size; y++)
				{
					aura[x, y] *= 0.97f;
				}
			}
			foreach (Particle particle in particles)
			{
				int minX = (int)particle.position.X - particleSize / 2;
				int minY = (int)particle.position.Y - particleSize / 2;
				int maxX = minX + particleSize;
				int maxY = minY + particleSize;
				for (int x = minX; x <= maxX; x++)
				{
					for (int y = minY; y <= maxY; y++)
					{
						if (x >= 0 && x < size && y >= 0 && y < size)
						{
							float strength = particle.strength;
							float offX = particle.position.X - x;
							float offY = particle.position.Y - y;
							strength *= 1f - (float)Math.Sqrt(offX * offX + offY * offY) / particleSize * 2;
							if (strength < 0f)
							{
								strength = 0f;
							}
							aura[x, y] = 1f - (1f - aura[x, y]) * (1f - strength);
						}
					}
				}
			}
		}

		public void RunAway()
		{
			if (Progress >= 180)
			{
				npc.active = false;
				if (Main.netMode != 1)
				{
					BluemagicWorld.terraDeaths++;
					if (Main.netMode == 2)
					{
						NetMessage.SendData(MessageID.WorldData);
					}
				}
			}
		}

		public void Initialize()
		{
			if (Progress == Main.netMode)
			{
				bullets.Clear();
				Vector2 center = npc.Center;
				if (Main.netMode != 1)
				{
					int lives = 10;
					switch (Stage)
					{
					case 4:
						lives = BluemagicWorld.terraCheckpoint1;
						break;
					case 6:
						lives = BluemagicWorld.terraCheckpoint2;
						break;
					case 8:
						lives = BluemagicWorld.terraCheckpoint3;
						break;
					case 10:
						lives = BluemagicWorld.terraCheckpointS;
						break;
					}
					for (int k = 0; k < 255; k++)
					{
						Player player = Main.player[k];
						if (player.active & !player.dead && player.position.X > center.X - arenaWidth / 2 && player.position.X + player.width < center.X + arenaWidth / 2 && player.position.Y > center.Y - arenaHeight / 2 && player.position.Y + player.height < center.Y + arenaHeight / 2)
						{
							player.GetModPlayer<BluemagicPlayer>().terraLives = lives;
							if (Main.netMode == 2)
							{
								ModPacket netMessage = GetPacket(TerraSpiritMessageType.TerraPlayer);
								netMessage.Write((byte)lives);
								netMessage.Send(k);
							}
							else if (player.whoAmI == Main.myPlayer)
							{
								Main.NewText("You have " + lives + " lives!");
							}
						}
					}
				}
			}
			if (Main.netMode != 1)
			{
				bool startFight = false;
				if (BluemagicWorld.terraDeaths == 0)
				{
					if (Progress == 90)
					{
						Talk("You haved saved Terraria...");
					}
					if (Progress == 210)
					{
						Talk("You have passed my trial...");
					}
					if (Progress == 330)
					{
						Talk("You have slain the god responsible for the Elemental Unleash...");
					}
					if (Progress == 450)
					{
						Talk("And yet, you still desire more power, more challenge?");
					}
					if (Progress >= 570)
					{
						Talk("Your greed shall be your undoing.");
						startFight = true;
					}
				}
				if (BluemagicWorld.terraDeaths == 1)
				{
					if (Progress == 90)
					{
						Talk("You have been slain by me...");
					}
					if (Progress == 210)
					{
						Talk("And still, you return?");
					}
					if (Progress == 330)
					{
						Talk("If another death is truly what you wish for...");
					}
					if (Progress >= 450)
					{
						Talk("...So be it... witness my true power...");
						startFight = true;
					}
				}
				if (BluemagicWorld.terraDeaths == 2)
				{
					if (Progress == 90)
					{
						Talk("I can see granting you the gift of immortality was a mistake...");
					}
					if (Progress == 210)
					{
						Talk("If you still refuse to give up...");
					}
					if (Progress >= 330)
					{
						Talk("I shall just beat you into submission...");
						startFight = true;
					}
				}
				if (BluemagicWorld.terraDeaths == 3)
				{
					if (Progress == 90)
					{
						Talk("You think 3 deaths is not many.");
					}
					if (Progress == 210)
					{
						Talk("You believe you still have a chance against me.");
					}
					if (Progress >= 330)
					{
						Talk("Allow me to show you how mistaken you are...");
						startFight = true;
					}
				}
				if (BluemagicWorld.terraDeaths == 4)
				{
					if (Progress == 90)
					{
						Talk("Humans are truly such strange creatures.");
					}
					if (Progress == 210)
					{
						Talk("You never give up, even if there is no hope...");
					}
					if (Progress >= 330)
					{
						Talk("That is why I chose you to save Terraria. But you have made me regret my choice...");
						startFight = true;
					}
				}
				if (BluemagicWorld.terraDeaths >= 5 && BluemagicWorld.terraDeaths < 10)
				{
					if (Progress == 90)
					{
						Talk("Do you still not tire of this?");
					}
					if (Progress >= 210)
					{
						Talk("Do you really wish to die this many times?");
						startFight = true;
					}
				}
				if (BluemagicWorld.terraDeaths >= 10 && BluemagicWorld.terraDeaths < 25)
				{
					if (Progress == 90)
					{
						Talk("You have lost against me " + BluemagicWorld.terraDeaths + " times now.");
					}
					if (Progress >= 210)
					{
						Talk("I will do whatever it takes to eradicate your hope...");
						startFight = true;
					}
				}
				if (BluemagicWorld.terraDeaths >= 25)
				{
					if (Progress == 90)
					{
						Talk("You are now at " + BluemagicWorld.terraDeaths + " deaths.");
					}
					if (Progress >= 210)
					{
						Talk("I tire of you... begone...");
						startFight = true;
					}
				}
				if (startFight)
				{
					Main.PlaySound(15, -1, -1, 0);
					Stage++;
					Progress = -1;
					if (Stage == 7 || Stage == 9)
					{
						Progress = 120;
					}
					npc.netUpdate = true;
				}
			}
		}

		private void Stage1()
		{
			if (Progress == 0 && Main.netMode != 1)
			{
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("TerraProbe1"), 0, npc.whoAmI);
			}
		}

		private void Stage2()
		{
			if (Progress == 0)
			{
				Main.PlaySound(15, -1, -1, 0);
			}
			if (Progress == 60)
			{
				Vector2 center = npc.Center;
				const int xOffset = arenaWidth / 4;
				const int yOffset = arenaHeight / 4;
				bullets.Add(new BulletPortal(center, center + new Vector2(xOffset, yOffset)));
				bullets.Add(new BulletPortal(center, center + new Vector2(-xOffset, yOffset)));
				bullets.Add(new BulletPortal(center, center + new Vector2(xOffset, -yOffset)));
				bullets.Add(new BulletPortal(center, center + new Vector2(-xOffset, -yOffset)));
			}
			if (Progress >= 420 && Progress <= 660 && Progress % 60 == 0)
			{
				Vector2 center = GetTarget().Center;
				bullets.Add(new BulletCross(center + new Vector2(0f, 192f)));
				bullets.Add(new BulletCross(center + new Vector2(0f, -192f)));
				bullets.Add(new BulletCross(center + new Vector2(192f, 0f)));
				bullets.Add(new BulletCross(center + new Vector2(-192f, 0f)));
			}
			if (Progress == 720)
			{
				bullets.Add(new BulletChase(npc.Center, 30, 360, (position, spirit) => new BulletAccel(position, spirit.GetTarget().Center - position)));
			}
			if (Progress >= 1080 && Progress <= 1320 && Progress % 120 == 0)
			{
				Vector2 bulletPos = GetTarget().Center;
				bullets.Add(new BulletRingTimed(bulletPos, 6, 192f, 0.03f, 120));
				bullets.Add(new BulletBeamBig(bulletPos, 320, MathHelper.PiOver2, 120));
			}
			if (Progress == 1440 && Main.netMode != 1)
			{
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("TerraProbe2"), 0, npc.whoAmI);
			}
		}

		private void Stage3()
		{
			if (Progress == 0)
			{
				Main.PlaySound(15, -1, -1, 0);
			}
			if (Progress >= 60 && Progress <= 300 && Progress % 60 == 0)
			{
				Vector2 center = GetTarget().Center;
				const float length = 192f;
				float rotation = 0f;
				if (Progress == 120)
				{
					rotation = MathHelper.PiOver4;
				}
				else if (Progress == 180)
				{
					rotation = MathHelper.Pi / 8f;
				}
				else if (Progress == 240)
				{
					rotation = MathHelper.Pi * 3f / 8f;
				}
				bullets.Add(new BulletCross(center + length * rotation.ToRotationVector2(), rotation));
				bullets.Add(new BulletCross(center + length * (rotation + MathHelper.PiOver2).ToRotationVector2(), rotation));
				bullets.Add(new BulletCross(center - length * rotation.ToRotationVector2(), rotation));
				bullets.Add(new BulletCross(center - length * (rotation + MathHelper.PiOver2).ToRotationVector2(), rotation));
			}
			if (Progress == 360)
			{
				Vector2 center = npc.Center;
				const int xOffset = arenaWidth / 4;
				const int yOffset = arenaHeight / 4;
				bullets.Add(new BulletPortal(center, center + new Vector2(xOffset, yOffset)));
				bullets.Add(new BulletPortal(center, center + new Vector2(-xOffset, yOffset)));
				bullets.Add(new BulletPortal(center, center + new Vector2(xOffset, -yOffset)));
				bullets.Add(new BulletPortal(center, center + new Vector2(-xOffset, -yOffset)));
			}
			if (Progress >= 510 && Progress <= 690 && (Progress - 510) % 90 == 0)
			{
				bullets.Add(new BulletBeamBig(GetTarget().Center, 160, MathHelper.PiOver2, 60));
			}
			if (Progress >= 720 && Progress <= 1020 && Progress % 30 == 0)
			{
				bullets.Add(new BulletRingShrink(GetTarget().Center, 8f, 0.012f));
			}
			if (Progress == 1140)
			{
				bullets.Add(new BulletBeamBigRotate(npc.Center, 0.01f, 360, 120f, MathHelper.PiOver2, 60));
				bullets.Add(new BulletBeamBigRotate(npc.Center, 0.01f, 360, 120f, 0f, 60));
				bullets.Add(new BulletChase(npc.Center, 30, 360, (position, spirit) => new BulletAccel(position, spirit.GetTarget().Center - position)));
			}
			if (Progress >= 1560 && Progress <= 1860 && Progress % 4 == 0)
			{
				Vector2 bulletPos = new Vector2(npc.Center.X, npc.Center.Y - arenaHeight / 2);
				bulletPos.X += 80f * (float)Math.Sin(MathHelper.TwoPi * Progress / 120);
				bullets.Add(new BulletArray(bulletPos, 0f, 160f, new Vector2(0f, 8f), arenaHeight / 8));
			}
			if (Progress >= 1620 && Progress <= 1860 && Progress % 60 == 0)
			{
				bullets.Add(new BulletBeamBig(GetTarget().Center, 160, 0f, 60));
			}
			if (Progress == 2040 && Main.netMode != 1)
			{
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("TerraProbe3"), 0, npc.whoAmI);
			}
		}

		private void Stage4()
		{
			if (Progress == 0)
			{
				Main.PlaySound(15, -1, -1, 0);
			}
			if (Progress >= 180 && Progress <= 540 && Progress % 60 == 0)
			{
				bullets.Add(new BulletRingExpand(npc.Center, 6f));
			}
			if (Progress >= 180 && Progress <= 540 && Progress % 60 == 30)
			{
				bullets.Add(new BulletRingExpand(npc.Center, 6f).Rotation(MathHelper.Pi / 16f));
			}
			if (Progress == 180)
			{
				bullets.Add(new BulletBeamBigRotate(npc.Center, -0.01f, 360, 120f, MathHelper.PiOver2, 60));
				bullets.Add(new BulletBeamBigRotate(npc.Center, -0.01f, 360, 120f, 0f, 60));
			}
			if (Progress >= 600 && Progress <= 960 && Progress % 90 == 0)
			{
				Vector2 center = GetTarget().Center;
				bullets.Add(new BulletCrossRotate(center, 192f, 0.02f, 0f));
				bullets.Add(new BulletCrossRotate(center, 192f, 0.02f, MathHelper.PiOver2));
				bullets.Add(new BulletCrossRotate(center, 192f, 0.02f, MathHelper.Pi));
				bullets.Add(new BulletCrossRotate(center, 192f, 0.02f, 3f * MathHelper.PiOver2));
			}
			if (Progress == 1080)
			{
				bullets.Add(new BulletFlowerDoom(npc.Center, 720));
			}
			if (Progress == 1980)
			{
				bullets.Add(new BulletBlackHole(npc.Center));
			}
			if (Progress == 2820 && Main.netMode != 1)
			{
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("TerraProbe4"), 0, npc.whoAmI);
			}
		}

		private void Stage5()
		{
			if (Progress == 0)
			{
				Main.PlaySound(15, -1, -1, 0);
			}
			if (Progress == 180)
			{
				bullets.Add(new BulletChase(npc.Center, 1000, 300, (position, spirit) => null, 0.03f));
			}
			if (Progress >= 180 && Progress <= 420 && Progress % 120 == 60)
			{
				bullets.Add(new BulletRingSpinOut(npc.Center, 8f, 0.005f));
				bullets.Add(new BulletRingSpinOut(npc.Center, 8f, -0.005f));
			}
			if (Progress >= 180 && Progress <= 420 && Progress % 120 == 0)
			{
				bullets.Add(new BulletRingSpinOut(npc.Center, 8f, 0.01f).Rotation(MathHelper.Pi / 16f));
				bullets.Add(new BulletRingSpinOut(npc.Center, 8f, -0.01f).Rotation(MathHelper.Pi / 16f));
			}
			if (Progress >= 540 && Progress <= 1020 && Progress % 60 == 0)
			{
				bullets.Add(new BulletRingExpand(npc.Center, 4f).NumBullets(32));
				bullets.Add(new BulletRingExpand(npc.Center, 8f).NumBullets(32).Rotation(MathHelper.Pi / 32f));
				bullets.Add(new BulletRingExpand(npc.Center, 12f).NumBullets(16).Rotation((GetTarget().Center - npc.Center).ToRotation()));
			}
			if (Progress == 1260)
			{
				bullets.Add(new BulletSlide(npc.Center));
			}
			if (Progress >= 1380 && Progress <= 1860 && Progress % 120 == 60)
			{
				bullets.Add(new BulletBeamBig(GetTarget().Center, delay: 60, life: 20));
			}
			if (Progress == 2000 && Main.netMode != 2)
			{
				Main.NewText("You can feel a Void World opening up behind the fabrics of reality...");
			}
			if (Progress == 2060)
			{
				Main.PlaySound(15, -1, -1, 0);
			}
			if (Progress >= 2060 && Progress <= 2660)
			{
				Vector2 arena = new Vector2(arenaWidth, arenaHeight);
				Vector2 origin = npc.Center - arena / 2f;
				Vector2 target = GetTarget().Center;
				Vector2 localTarget = target - origin;
				if (Progress % 8 == 0)
				{
					float x1 = localTarget.X;
					float x2 = (localTarget.X + arena.X / 2f) % arena.X;
					float y1 = localTarget.Y;
					float y2 = (localTarget.Y + arena.Y / 2f) % arena.Y;
					x1 += origin.X;
					x2 += origin.X;
					y1 += origin.Y;
					y2 += origin.Y;
					bullets.Insert(0, new BulletVoidWorld(new Vector2(x1, y1)));
					bullets.Insert(0, new BulletVoidWorld(new Vector2(x1, y2)));
					bullets.Insert(0, new BulletVoidWorld(new Vector2(x2, y1)));
					bullets.Insert(0, new BulletVoidWorld(new Vector2(x2, y2)));
				}
				if (Progress % 8 == 2)
				{
					const int interval = 400;
					for (int x = 0; x < arenaWidth / interval; x++)
					{
						float xPos = (localTarget.X + x * interval + interval / 2) % arena.X + origin.X;
						for (int y = 0; y < arenaHeight / interval; y++)
						{
							float yPos = (localTarget.Y + y * interval + interval / 2) % arena.Y + origin.Y;
							bullets.Insert(0, new BulletVoidWorld(new Vector2(xPos, yPos)));
						}
					}
				}
				if (Progress % 8 == 4)
				{
					float x1 = origin.X + localTarget.X;
					float x2 = origin.X + arena.X - localTarget.X;
					float y1 = origin.Y + localTarget.Y;
					float y2 = origin.Y + arena.Y - localTarget.Y;
					bullets.Insert(0, new BulletVoidWorld(new Vector2(x1, y1)));
					bullets.Insert(0, new BulletVoidWorld(new Vector2(x1, y2)));
					bullets.Insert(0, new BulletVoidWorld(new Vector2(x2, y1)));
					bullets.Insert(0, new BulletVoidWorld(new Vector2(x2, y2)));
				}
				if (Progress % 8 == 6)
				{
					bullets.Insert(0, new BulletVoidWorld(new Vector2(target.X - 100f, target.Y)));
					bullets.Insert(0, new BulletVoidWorld(new Vector2(target.X + 100f, target.Y)));
					bullets.Insert(0, new BulletVoidWorld(new Vector2(target.X, target.Y - 100f)));
					bullets.Insert(0, new BulletVoidWorld(new Vector2(target.X, target.Y + 100f)));
				}
			}
			if (Progress == 2800 && Main.netMode != 1)
			{
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("TerraProbe5"), 0, npc.whoAmI);
			}
		}

		private void End()
		{
			if (Progress == 0)
			{
				Main.PlaySound(29, -1, -1, 92);
			}
			if (Main.expertMode)
			{
				if (Progress >= 280)
				{
					NPC.NewNPC((int)npc.Bottom.X, (int)npc.Bottom.Y, mod.NPCType("TerraSpirit2"));
					npc.active = false;
				}
			}
			else
			{
				npc.dontTakeDamage = true;
				if (Progress >= 420)
				{
					if (Main.netMode != 1)
					{
						BluemagicWorld.downedTerraSpirit = true;
						if (Main.netMode == 2)
						{
							NetMessage.SendData(MessageID.WorldData);
						}
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PuriumCoin"), Main.rand.Next(10, 13));
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("RainbowStar"));
					}
					npc.active = false;
				}
			}
		}

		private void FixLife()
		{
			switch (Stage)
			{
			case 0:
			case 1:
				npc.life = 750000;
				break;
			case 2:
			case 3:
				npc.life = 700000;
				break;
			case 4:
			case 5:
				npc.life = 600000;
				break;
			case 6:
			case 7:
				npc.life = 450000;
				break;
			case 8:
			case 9:
				npc.life = 250000;
				break;
			case 10:
			case 11:
				if (Main.expertMode)
				{
					npc.life = 1;
				}
				break;
			}
		}

		public override bool CheckDead()
		{
			FixLife();
			return false;
		}

		public override bool? CanBeHitByItem(Player player, Item item)
		{
			return false;
		}

		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			float scale = 1f;
			if (!Main.expertMode && Stage == 11)
			{
				scale += 9f * Progress / 400f;
			}
			float alpha = 1f;
			alpha -= 0.8f * (scale - 1f) / 9f;
			for (int x = 0; x < size; x++)
			{
				for (int y = 0; y < size; y++)
				{
					Vector2 drawPos = npc.Center - Main.screenPosition;
					drawPos.X += scale * (x * 2 - size);
					drawPos.Y += scale * (y * 2 - size);
					spriteBatch.Draw(mod.GetTexture("PuritySpirit/PurityParticle"), drawPos, null, Color.White * aura[x, y] * alpha, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
				}
			}
			spriteBatch.Draw(mod.GetTexture("PuritySpirit/PurityEyes"), npc.position - Main.screenPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			const int blockSize = 16;
			int centerX = (int)npc.Center.X;
			int centerY = (int)npc.Center.Y;
			Texture2D outlineTexture = mod.GetTexture("TerraSpirit/TerraBlockOutline");
			Texture2D blockTexture = mod.GetTexture("TerraSpirit/TerraBlock");
			for (int x = centerX - (arenaWidth + blockSize) / 2; x <= centerX + (arenaWidth + blockSize) / 2; x += blockSize)
			{
				int y = centerY - (arenaHeight + blockSize) / 2;
				Vector2 drawPos = new Vector2(x - blockSize / 2, y - blockSize / 2) - Main.screenPosition;
				spriteBatch.Draw(outlineTexture, drawPos, Color.White);
				spriteBatch.Draw(blockTexture, drawPos, Color.White * 0.75f);
				drawPos.Y += arenaHeight + blockSize;
				spriteBatch.Draw(outlineTexture, drawPos, Color.White);
				spriteBatch.Draw(blockTexture, drawPos, Color.White * 0.75f);
			}
			for (int y = centerY - (arenaHeight + blockSize) / 2; y <= centerY + (arenaHeight + blockSize) / 2; y += blockSize)
			{
				int x = centerX - (arenaWidth + blockSize) / 2;
				Vector2 drawPos = new Vector2(x - blockSize / 2, y - blockSize / 2) - Main.screenPosition;
				spriteBatch.Draw(outlineTexture, drawPos, Color.White);
				spriteBatch.Draw(blockTexture, drawPos, Color.White * 0.75f);
				drawPos.X += arenaWidth + blockSize;
				spriteBatch.Draw(outlineTexture, drawPos, Color.White);
				spriteBatch.Draw(blockTexture, drawPos, Color.White * 0.75f);
			}

			foreach (Bullet bullet in bullets)
			{
				bullet.Draw(spriteBatch);
			}
		}

		private void Talk(string message)
		{
			if (Main.netMode != 2)
			{
				string text = Language.GetTextValue("Mods.Bluemagic.NPCTalk", Lang.GetNPCNameValue(npc.type), message);
				Main.NewText(text, 150, 250, 150);
			}
			else
			{
				NetworkText text = NetworkText.FromKey("Mods.Bluemagic.NPCTalk", Lang.GetNPCNameValue(npc.type), message);
				NetMessage.BroadcastChatMessage(text, new Color(150, 250, 150));
			}
		}

		private ModPacket GetPacket(TerraSpiritMessageType type)
		{
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)MessageType.TerraSpirit);
			packet.Write(npc.whoAmI);
			packet.Write((byte)type);
			return packet;
		}

		public void HandlePacket(BinaryReader reader)
		{
			TerraSpiritMessageType type = (TerraSpiritMessageType)reader.ReadByte();
			if (type == TerraSpiritMessageType.TerraPlayer)
			{
				byte lives = reader.ReadByte();
				Player player = Main.player[Main.myPlayer];
				player.GetModPlayer<BluemagicPlayer>(mod).terraLives = lives;
				Main.NewText("You have " + lives + " lives!");
			}
		}
	}

	enum TerraSpiritMessageType : byte
	{
		TerraPlayer
	}
}
