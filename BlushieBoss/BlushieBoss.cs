using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Localization;

namespace Bluemagic.BlushieBoss
{
	public static class BlushieBoss
	{
		public const int ArenaSize = 800;
		internal static int Phase = 0;
		internal static int Timer = 0;
		internal static List<Bullet> bullets = new List<Bullet>();
		internal static Vector2 Origin;
		internal static bool[] Players = new bool[256];
		internal static bool CameraFocus = false;
		private static int[] index = new int[] { -1, -1, -1, -1, -1, -1 };
		internal static bool BlushieC;
		internal static Vector2 PosK;
		internal static Vector2 PosA;
		internal static Vector2 PosL;
		internal static Vector2 DataK;
		internal static float DataA;
		internal static Vector2 DataL;
		internal static float DataL2;
		internal static int Immune = 0;
		internal static int HealthK;
		internal static int HealthA;
		internal static int HealthL;
		internal static int ShieldK = 0;
		internal static int ShieldA = 0;
		internal static int ShieldL = 0;
		internal static Vector2 DragonPos;
		internal static Vector2 ArmLeftPos;
		internal static Vector2 ArmRightPos;
		internal static Vector2 SkullPos;
		internal static Vector2 BoneLTPos;
		internal static Vector2 BoneLBPos;
		internal static Vector2 BoneRTPos;
		internal static Vector2 BoneRBPos;
		internal static float BoneLTRot;
		internal static float BoneLBRot;
		internal static float BoneRTRot;
		internal static float BoneRBRot;
		internal static int Phase3Attack;

		private static int[] types = new int[6];
		internal static Texture2D BulletWhiteTexture;
		internal static Texture2D BulletGoldTexture;
		internal static Texture2D BulletGoldLargeTexture;
		internal static Texture2D BulletStarTexture;
		internal static Texture2D BulletPurpleTexture;
		internal static Texture2D BulletBlackTexture;
		internal static Texture2D BulletBlueTexture;
		internal static Texture2D BulletBlueLargeTexture;
		internal static Texture2D BulletBlueSmallTexture;
		internal static Texture2D BulletBoxBlueTexture;
		internal static Texture2D BulletFireLargeTexture;
		internal static Texture2D BulletFireTexture;
		internal static Texture2D[] BulletColorTextures;
		internal static Texture2D BulletLightTexture;

		public static bool Active
		{
			get
			{
				return Phase > 0;
			}
		}

		public static int Difficulty
		{
			get
			{
				return Main.expertMode ? 2 : 1;
			}
		}

		internal static void Load()
		{
			types[0] = Bluemagic.Instance.NPCType("Blushiemagic");
			types[1] = Bluemagic.Instance.NPCType("BlushiemagicK");
			types[2] = Bluemagic.Instance.NPCType("BlushiemagicA");
			types[3] = Bluemagic.Instance.NPCType("BlushiemagicL");
			types[4] = Bluemagic.Instance.NPCType("BlushiemagicM");
			types[5] = Bluemagic.Instance.NPCType("BlushiemagicJ");
			BulletWhiteTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletWhite");
			BulletGoldTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletGold");
			BulletGoldLargeTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletGoldLarge");
			BulletStarTexture = Bluemagic.Instance.GetTexture("BlushieBoss/Star");
			BulletPurpleTexture = Bluemagic.Instance.GetTexture("BlushieBoss/LightningOrb");
			BulletBlackTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletBlack");
			BulletBlueTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletBlue");
			BulletBlueLargeTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletBlueLarge");
			BulletBlueSmallTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletBlueSmall");
			BulletBoxBlueTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletBoxBlue");
			BulletFireLargeTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletFireLarge");
			BulletFireTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletFire");
			BulletColorTextures = new Texture2D[6];
			BulletColorTextures[0] = Bluemagic.Instance.GetTexture("BlushieBoss/BulletRed");
			BulletColorTextures[1] = Bluemagic.Instance.GetTexture("BlushieBoss/BulletOrange");
			BulletColorTextures[2] = BulletGoldTexture;
			BulletColorTextures[3] = Bluemagic.Instance.GetTexture("BlushieBoss/BulletGreenLight");
			BulletColorTextures[4] = Bluemagic.Instance.GetTexture("BlushieBoss/BulletBlueLight");
			BulletColorTextures[5] = Bluemagic.Instance.GetTexture("BlushieBoss/BulletPurple");
			BulletLightTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletLight");
		}

		internal static void Update()
		{
			bool anyBlushie = false;
			for (int k = 0; k < index.Length; k++)
			{
				index[k] = NPC.FindFirstNPC(types[k]);
				if (index[k] > -1 && Main.npc[index[k]].timeLeft < 600)
				{
					Main.npc[index[k]].timeLeft = 6000;
				}
				for (int i = 0; i < 200; i++)
				{
					if (i != index[k] && Main.npc[i].active && Main.npc[i].type == types[k])
					{
						Main.npc[i].active = false;
					}
				}
				if (index[k] > -1)
				{
					anyBlushie = true;
				}
			}
			if (!Active && index[0] > -1 && Main.netMode != 1)
			{
				Initialize();
			}
			if (Active && !anyBlushie)
			{
				Reset();
			}
			if (Active)
			{
				SkyManager.Instance.Activate("Bluemagic:BlushieBoss");
			}
			else
			{
				SkyManager.Instance.Deactivate("Bluemagic:BlushieBoss");
			}
			for (int k = 0; k < 255; k++)
			{
				if (!Main.player[k].active || Main.player[k].dead)
				{
					Players[k] = false;
				}
			}
			if (Immune > 0)
			{
				Immune--;
			}
			if (Active)
			{
				Timer++;
				if (Phase == 1)
				{
					Phase1();
				}
				else if (Phase == 2)
				{
					Phase2();
				}
				else if (Phase == 3)
				{
					Phase3();
				}
				Player player = Main.player[GetTarget()];
				if (Players[player.whoAmI])
				{
					player.GetModPlayer<BluemagicPlayer>().BlushieBarrier();
				}
				for (int k = 0; k < bullets.Count; k++)
				{
					bullets[k].Update();
					if (bullets[k].ShouldRemove())
					{
						bullets[k].Active = false;
						bullets.RemoveAt(k);
						k--;
					}
					else if (bullets[k].Damage > 0f && Vector2.Distance(player.Center, bullets[k].Position) < bullets[k].Size)
					{
						player.GetModPlayer<BluemagicPlayer>().BlushieDamage(bullets[k].Damage);
					}
				}
			}
			bool allDead = true;
			for (int k = 0; k < 255; k++)
			{
				if (Players[k])
				{
					allDead = false;
					break;
				}
			}
			if (allDead)
			{
				if (Main.netMode != 1 && Phase == 1)
				{
					BlushieTalk("Done already? Well, I hope we can fight again soon!");
				}
				Reset();
			}
		}

		internal static void Initialize()
		{
			Phase = 1;
			Timer = 0;
			HealthK = 1000000;
			HealthA = 1000000;
			HealthL = 1000000;
			ShieldK = 0;
			ShieldA = 0;
			ShieldL = 0;
			Origin = Main.npc[index[0]].Center;
			for (int k = 0; k < 255; k++)
			{
				Player player = Main.player[k];
				Players[k] = player.active && !player.dead && player.position.X >= Origin.X - ArenaSize && player.position.X + player.width <= Origin.X + ArenaSize && player.position.Y >= Origin.Y - ArenaSize && player.position.Y + player.height <= Origin.Y + ArenaSize;
				if (Players[k])
				{
					Main.player[k].GetModPlayer<BluemagicPlayer>().blushieHealth = 1f;
				}
			}
		}

		internal static void Reset()
		{
			Phase = 0;
			Timer = 0;
			CameraFocus = false;
			HealthK = 1000000;
			HealthA = 1000000;
			HealthL = 1000000;
			ShieldK = 0;
			ShieldA = 0;
			ShieldL = 0;
			for (int k = 0; k < 255; k++)
			{
				Players[k] = false;
			}
			for (int k = 0; k < index.Length; k++)
			{
				if (index[k] > -1)
				{
					Main.npc[index[k]].active = false;
				}
			}
			bullets.Clear();
		}

		private static void Phase1()
		{
			NPC npc = Main.npc[index[0]];
			if (Main.netMode != 1)
			{
				bool flag = false;
				for (int k = 0; k < 255; k++)
				{
					if (Players[k] && Main.player[k].GetModPlayer<BluemagicPlayer>().triedGodmode)
					{
						flag = true;
						break;
					}
				}
				if (Timer == 1)
				{
					Music ("Music - Shelter - by Phyrnna");
				}
				if (flag && Timer == 120)
				{
					BlushieTalk("You're using the Rainbow Star?");
				}
				if (flag && Timer == 240)
				{
					BlushieTalk("I hope you know you can't cheese me with an item I made...");
				}
				if (Timer == 360)
				{
					BlushieTalk("Hi there! I hope you enjoyed tModLoader!");
				}
				if (Timer == 480)
				{
					BlushieTalk("Are you ready for your final challenge?");
				}
				if (Timer == 2040)
				{
					BlushieTalk("\x300cAstral Collision\x300d");
				}
				if (Timer == 3480)
				{
					BlushieTalk("I'm... so tired...");
				}
				if (Timer == 3600)
				{
					BlushieTalk("Sorry for such a disappointing fight T-T");
				}
			}
			npc.Center = Origin + new Vector2(0f, 16f * (float)Math.Sin(Timer / 60f));
			npc.velocity = Vector2.Zero;
			if (Main.netMode != 2)
			{
				if (Timer >= 600 && Timer <= 1200 && Timer % 30 == 0)
				{
					Vector2 target = Main.player[GetTarget()].Center;
					Vector2 offset = target - Origin;
					float length = offset.Length();
					offset.Normalize();
					float rotate = offset.ToRotation();
					int num = 16 * Difficulty;
					for (int k = 0; k < num; k++)
					{
						float useRotate = rotate + k * MathHelper.TwoPi / num;
						AddBullet(BulletSimple.NewWhite(Origin, length / 90f * useRotate.ToRotationVector2()));
					}
					num = 4 * Difficulty;
					for (int k = 0; k < num; k++)
					{
						float useRotate = rotate + k * MathHelper.TwoPi / num;
						Vector2 direction = useRotate.ToRotationVector2();
						Vector2 center = Origin + length / 2 * direction;
						AddBullet(BulletRotate.NewGold(center, length / 2f, useRotate + MathHelper.Pi, MathHelper.TwoPi / 180f, 180));
						AddBullet(BulletRotate.NewGold(center, length / 2f, useRotate + MathHelper.Pi, -MathHelper.TwoPi / 180f, 180));
					}
				}
				if (Timer >= 1320 && Timer <= 1920 && Timer % 60 == 0)
				{
					Bullet center = BulletTarget.NewGoldLarge(Origin, Main.player[GetTarget()].Center, 60);
					AddBullet(center);
					int num = 8 * Difficulty;
					for (int k = 0; k < num; k++)
					{
						float rotate = k / (float)num * MathHelper.TwoPi;
						AddBullet(BulletRelease.NewWhite(center, 60f, rotate, MathHelper.TwoPi / 480f, 4f));
						AddBullet(BulletRelease.NewWhite(center, 60f, rotate, -MathHelper.TwoPi / 480f, 4f));
					}
				}
				if (Timer >= 2040 && Timer <= 2640 && Timer % 90 == 0)
				{
					Vector2 offset = Main.player[GetTarget()].Center - Origin;
					offset.Normalize();
					offset *= 6f;
					for (int k = 0; k < 5; k++)
					{
						float angle = MathHelper.Pi * 1.5f - k * MathHelper.TwoPi / 5f;
						float nextAngle = MathHelper.Pi * 1.5f - (k + 2) * MathHelper.TwoPi / 5f;
						Vector2 start = angle.ToRotationVector2();
						Vector2 end = nextAngle.ToRotationVector2();
						int num = 6 * Difficulty;
						for (int i = 0; i < num; i++)
						{
							if (i == num / 2 - 1 || i == num / 2)
							{
								continue;
							}
							Vector2 adjust = Vector2.Lerp(start, end, i / (float)num);
							AddBullet(BulletBounce.NewStar(Origin, offset + 0.75f * adjust, 5));
						}
					}
				}
			}
			if (Timer >= 3600)
			{
				npc.dontTakeDamage = false;
			}
			else
			{
				npc.life = npc.lifeMax;
			}
		}

		internal static void StartPhase2()
		{
			Phase = 2;
			Timer = 0;
			index[4] = NPC.NewNPC((int)Origin.X, (int)Origin.Y, types[4]);
			Main.npc[index[4]].Center = Origin;
			BlushieC = Main.rand.Next(10) == 0;
		}

		internal static void Phase2()
		{
			NPC megan = Main.npc[index[4]];
			if (Main.netMode != 1)
			{
				if (Timer == 1)
				{
					BlushieTalk("Woah... what's happening to me?");
				}
				if (Timer == 100)
				{
					Music("Music - Return of the Snow Queen - by Phyrnna");
				}
				if (Timer == 180)
				{
					int x = (int)megan.Bottom.X;
					int y = (int)megan.Bottom.Y;
					for (int k = 1; k <= 3; k++)
					{
						index[k] = NPC.NewNPC(x, y, types[k], index[4]);
					}
					PosK = Vector2.Zero;
					PosA = Vector2.Zero;
					PosL = Vector2.Zero;
					KylieTalk("Why am I here? I'm useless...");
					if (BlushieC)
					{
						ChrisTalk("blushiemagic (A) wasn't able to make it this time. But I can fight you in her place!");
					}
					else
					{
						AnnaTalk("Hi there! Are you ready to have fun?~");
					}
					LunaTalk("You think you can stand up to me? We shall see...");
				}
				if (Timer == 1800)
				{
					if (BlushieC)
					{
						ChrisTalk("\x300cRay of Absolute Light\x300d");
					}
					else
					{
						AnnaTalk("\x300cRay of Absolute Light\x300d!~");
					}
				}
				if (Timer == 2640)
				{
					KylieTalk("\x300cMirrors of Imprisonment\x300d");
				}
				if (Timer == 2040)
				{
					LunaTalk("\x300cShadow Vortex\x300d");
				}
			}
			if (Timer < 180)
			{
				return;
			}
			NPC kylie = index[1] > -1 ? Main.npc[index[1]] : null;
			NPC anna = index[2] > -1 ? Main.npc[index[2]] : null;
			NPC luna = index[3] > -1 ? Main.npc[index[3]] : null;
			if (Main.netMode != 1)
			{
				if (kylie != null && kylie.localAI[1] == 0f && (ShieldBuff(kylie) || ShieldBuff(anna) || ShieldBuff(luna)))
				{
					KylieTalk("Oh, um, I can create shields around us to protect us from damage. Yeah...");
					kylie.localAI[1] = 1f;
				}
				if (luna != null && luna.localAI[1] == 0f && (DamageBuff(kylie) || DamageBuff(anna) || DamageBuff(luna)))
				{
					LunaTalk("Impressive. You still live. But how about if I buff our damage?");
					luna.localAI[1] = 1f;
				}
				if (anna != null && anna.localAI[1] == 0f && (HealBuff(kylie) || HealBuff(anna) || HealBuff(luna)))
				{
					if (BlushieC)
					{
						ChrisTalk("Uh oh, looks like we're in need of some healing!");
					}
					else
					{
						AnnaTalk("You're not gonna defeat any of us on my watch! >:( Magic healing powers, go!");
					}
					anna.localAI[1] = 1f;
				}
				if (kylie == null && luna == null && anna == null)
				{
					StartPhase3();
				}
			}
			if (Timer >= 180 && Timer < 600)
			{
				float distance = (Timer - 180) / 300f;
				if (distance > 1f)
				{
					distance = 1f;
				}
				distance *= ArenaSize * 0.75f;
				PosK = new Vector2(0f, -distance);
				PosA = distance * (MathHelper.Pi * 5f / 6f).ToRotationVector2();
				PosL = distance * (MathHelper.Pi * 1f / 6f).ToRotationVector2();
			}
			if (Timer >= 480)
			{
				megan.Center = Origin + new Vector2(0f, 16f * (float)Math.Sin((Timer - 480f) / 60f));
			}
			if (Timer >= 600)
			{
				float kVal = ((Timer - 600) / 3600f * 4f + 0.5f) % 4f;
				float distance = ArenaSize * 0.75f;
				if (kVal < 1f)
				{
					PosK = Vector2.Lerp(new Vector2(-distance, -distance), new Vector2(distance, -distance), kVal);
				}
				else if (kVal < 2f)
				{
					PosK = Vector2.Lerp(new Vector2(distance, -distance), new Vector2(distance, distance), kVal - 1f);
				}
				else if (kVal < 3f)
				{
					PosK = Vector2.Lerp(new Vector2(distance, distance), new Vector2(-distance, distance), kVal - 2f);
				}
				else
				{
					PosK = Vector2.Lerp(new Vector2(-distance, distance), new Vector2(-distance, -distance), kVal - 3f);
				}
				float theta = (Timer - 600) * MathHelper.TwoPi / 1800f;
				float r = (float)Math.Cos(1.8f * theta);
				PosA = ArenaSize * 0.75f * r * (MathHelper.Pi * 5f / 6f - theta).ToRotationVector2();
				float rot = MathHelper.Pi / 6f - ((Timer - 600) % 3600) * MathHelper.TwoPi / 3600f;
				PosL = ArenaSize * 0.75f * rot.ToRotationVector2();
			}
			PosK += Origin;
			PosA += Origin;
			PosL += Origin;
			if (kylie != null)
			{
				kylie.Center = PosK;
			}
			if (anna != null)
			{
				anna.Center = PosA;
			}
			if (luna != null)
			{
				luna.Center = PosL;
			}
			if (Main.netMode != 2 && Timer >= 600)
			{
				if (kylie != null)
				{
					KylieAttack();
					kylie.dontTakeDamage = false;
					if (HealBuff(kylie))
					{
						HealthK += 123;
					}
					kylie.life = HealthK;
					if (ShieldK < 300)
					{
						ShieldK++;
					}
				}
				if (anna != null)
				{
					AnnaAttack();
					anna.dontTakeDamage = false;
					if (HealBuff(anna))
					{
						HealthA += 123;
					}
					anna.life = HealthA;
					if (ShieldA < 300)
					{
						ShieldA++;
					}
				}
				if (luna != null)
				{
					LunaAttack();
					luna.dontTakeDamage = false;
					if (HealBuff(luna))
					{
						HealthL += 123;
					}
					luna.life = HealthL;
					if (ShieldL < 300)
					{
						ShieldL++;
					}
				}
			}
		}

		private static void KylieAttack()
		{
			float damage = DamageBuff(Main.npc[index[1]]) ? 1.5f : 1f;
			int numCycles = Phase2Count() == 1 ? 2 : 1;
			int timerK = (Timer - 600) % 2820;
			for (int i = 0; i < numCycles; i++)
			{
				if (timerK < 840 && timerK % (120 / Difficulty) == 0)
				{
					for (int k = 0; k < 8; k++)
					{
						float angle = k / 8f * MathHelper.TwoPi;
						AddBullet(new BulletRotateKylie(angle, MathHelper.TwoPi / 300f), damage);
						AddBullet(new BulletRotateKylie(angle, -MathHelper.TwoPi / 300f), damage);
					}
				}
				if (timerK >= 1140 && timerK < 1860 && timerK % 90 == 60)
				{
					var bullet = BulletTargetSmooth.NewBlueLarge(PosK, Main.player[GetTarget()].Center, 450);
					AddBullet(bullet, damage);
					int num = (int)(32 * Math.Sqrt(Difficulty));
					for (int k = 0; k < num; k++)
					{
						float rot = MathHelper.TwoPi * k / (float)num;
						AddBullet(BulletRotateAround.NewBlueSmall(bullet, 80f * (float)Math.Sqrt(Difficulty), rot, MathHelper.TwoPi / 120f), damage);
					}
				}
				if (timerK >= 2040 && timerK < 2640)
				{
					if (timerK % 60 == 0)
					{
						Vector2 uv = Main.player[GetTarget()].Center - Origin;
						uv.X -= (float)Math.Floor(uv.X / 400f) * 400f;
						uv.Y -= (float)Math.Floor(uv.Y / 400f) * 400f;
						DataK = Origin - new Vector2(ArenaSize) + uv;
					}
					if (timerK % 8 == 0)
					{
						for (float x = DataK.X; x <= Origin.X + ArenaSize; x += 400f)
						{
							AddBullet(BulletSimple.NewBoxBlue(new Vector2(x, Origin.Y - ArenaSize), new Vector2(0f, 8f)), damage);
							if (Difficulty >= 2)
							{
								AddBullet(BulletSimple.NewBoxBlue(new Vector2(x, Origin.Y + ArenaSize), new Vector2(0f, -8f)), damage);
							}
						}
						for (float y = DataK.Y; y <= Origin.Y + ArenaSize; y += 400f)
						{
							AddBullet(BulletSimple.NewBoxBlue(new Vector2(Origin.X - ArenaSize, y), new Vector2(8f, 0f)), damage);
							if (Difficulty >= 2)
							{
								AddBullet(BulletSimple.NewBoxBlue(new Vector2(Origin.X + ArenaSize, y), new Vector2(-8f, 0f)), damage);
							}
						}
					}
				}
				timerK = (timerK + 1410) % 2820;
			}
		}

		private static void AnnaAttack()
		{
			float damage = DamageBuff(Main.npc[index[2]]) ? 1.5f : 1f;
			int numCycles = Phase2Count() == 1 ? 2 : 1;
			int timerA = (Timer - 600) % 1800;
			for (int i = 0; i < numCycles; i++)
			{
				if (timerA < 450 && timerA % (90 / Difficulty) == 0)
				{
					AddBullet(new BulletFireBomb(Main.player[GetTarget()].Center, 120, damage));
				}
				if (timerA >= 600 && timerA < 1080 && timerA % (12 / Difficulty) == 0)
				{
					float baseRot = (float)Math.Sin(timerA * MathHelper.TwoPi / 120f);
					baseRot *= MathHelper.Pi / 6f;
					for (int k = 0; k < 6; k++)
					{
						float rot = baseRot + MathHelper.TwoPi * k / 6f;
						int color = (k + (timerA / (12 / Difficulty))) % 6;
						AddBullet(BulletSimple.NewColor(PosA, 6f * rot.ToRotationVector2(), color), damage);
					}
				}
				float raySize = 80f * Difficulty;
				if (timerA == 1200)
				{
					DataA = Main.player[GetTarget()].Center.X - raySize;
				}
				if (timerA >= 1200 && timerA < 1680 && timerA % (2 / Difficulty) == 0)
				{
					float progress = (timerA - 1200) / 480f;
					int count = 1 + (int)(progress * 5f);
					for (int k = 0; k < progress; k++)
					{
						AddBullet(BulletSimple.NewLight(new Vector2(DataA + 2 * raySize * Main.rand.NextFloat(), Origin.Y + ArenaSize), new Vector2(0f, -4f - progress * 16f)), damage);
					}
				}
				if (timerA == 1680)
				{
					Main.PlaySound(29, -1, -1, 104);
				}
				if (timerA >= 1680 && timerA < 1740)
				{
					for (int k = 0; k < 20 * Difficulty; k++)
					{
						var bullet = BulletSimple.NewLight(new Vector2(DataA + 2 * raySize * Main.rand.NextFloat(), Origin.Y - ArenaSize - 32f), new Vector2(0f, 32f));
						bullet.Damage = 0.2f;
						AddBullet(bullet, damage);
						bullet = BulletSimple.NewLight(new Vector2(DataA + 2 * raySize * Main.rand.NextFloat(), Origin.Y - ArenaSize - 16f), new Vector2(0f, 32f));
						bullet.Damage = 0.2f;
						AddBullet(bullet, damage);
					}
				}
				timerA = (timerA + 900) % 1800;
			}
		}

		private static void LunaAttack()
		{
			float damage = DamageBuff(Main.npc[index[3]]) ? 1.5f : 1f;
			int numCycles = Phase2Count() == 1 ? 2 : 1;
			int timerL = (Timer - 600) % 2200;
			for (int i = 0; i < numCycles; i++)
			{
				if (timerL < 600)
				{
					if (timerL % 32 == 0)
					{
						int num = 2 * Difficulty;
						for (int k = 0; k < num; k++)
						{
							AddBullet(new BulletRotateLuna(1f, k * MathHelper.TwoPi / num), damage);
						}
					}
					else if (timerL % 32 == 16)
					{
						int num = 2 * Difficulty;
						for (int k = 0; k < num; k++)
						{
							AddBullet(new BulletRotateLuna(-1f, (k + 0.5f) * MathHelper.TwoPi / num), damage);
						}
					}
				}
				if (timerL >= 780 && timerL < 1260)
				{
					if (timerL % 120 == 60)
					{
						DataL = PosL;
						Vector2 dir = Main.player[GetTarget()].Center - PosL;
						DataL2 = dir.ToRotation();
					}
					if (timerL % 120 >= 60 && timerL % 120 < 60 + 30 * Difficulty)
					{
						float normal = DataL2 + MathHelper.PiOver2;
						Vector2 pos = DataL + (32f * Main.rand.NextFloat() - 16f) * normal.ToRotationVector2();
						AddBullet(new BulletLightning(pos, 10f * DataL2.ToRotationVector2()), damage);
					}
				}
				if (timerL >= 1440 && timerL < 2080 && (Difficulty > 1 || timerL % 2 == 0))
				{
					float rot = timerL;
					int num = 1;
					for (int k = 0; k < num; k++)
					{
						float useRot = rot + k * MathHelper.TwoPi / num;
						AddBullet(new BulletPull(PosL + 1600f * (float)Math.Sqrt(2) * useRot.ToRotationVector2()), damage);
					}
				}
				timerL = (timerL + 1100) % 2200;
			}
		}

		internal static int Phase2Count()
		{
			int count = 0;
			for (int k = 1; k <= 3; k++)
			{
				if (index[k] > -1)
				{
					count++;
				}
			}
			return count;
		}

		internal static bool ShieldBuff(NPC npc)
		{
			int count = Phase2Count();
			return index[1] > -1 && npc.life <= 1500000 - 250000 * count;
		}

		internal static bool DamageBuff(NPC npc)
		{
			int count = Phase2Count();
			return index[3] > -1 && npc.life <= 1250000 - 250000 * count;
		}

		internal static bool HealBuff(NPC npc)
		{
			int count = Phase2Count();
			return index[2] > -1 && npc.life <= 1000000 - 250000 * count;
		}

		internal static void StartPhase3()
		{
			Phase = 3;
			Timer = 0;
			DragonPos = Origin + new Vector2(0f, -ArenaSize * 0.6f);
			ArmLeftPos = Origin + new Vector2(ArenaSize * 0.6f, 0f);
			ArmRightPos = Origin + new Vector2(-ArenaSize * 0.6f, 0f);
			SkullPos = Origin + new Vector2(0f, ArenaSize * 0.6f);
			BoneLTPos = SkullPos + new Vector2(-ArenaSize * 0.3f - 100f, 100f);
			BoneLBPos = SkullPos + new Vector2(-ArenaSize * 0.3f - 100f, -100f);
			BoneRTPos = SkullPos + new Vector2(ArenaSize * 0.3f + 100f, 100f);
			BoneRBPos = SkullPos + new Vector2(ArenaSize * 0.3f + 100f, -100f);
			BoneLTRot = -MathHelper.Pi / 4f;
			BoneLBRot = MathHelper.Pi / 4f;
			BoneRTRot = MathHelper.Pi / 4f;
			BoneRBRot = -MathHelper.Pi / 4f;
			Phase3Attack = 0;
			for (int k = 0; k < 255; k++)
			{
				if (Players[k])
				{
					BluemagicPlayer modPlayer = Main.player[k].GetModPlayer<BluemagicPlayer>();
					if (modPlayer.blushieHealth > BluemagicWorld.blushieCheckpoint)
					{
						BluemagicWorld.blushieCheckpoint = modPlayer.blushieHealth;
					}
				}
			}
		}

		internal static void Phase3()
		{
			NPC megan = Main.npc[index[4]];
			megan.Center = Origin + new Vector2(0f, 16f * (float)Math.Sin((Timer - 480f) / 60f));
			if (Main.netMode != 2)
			{
				if (Timer == 840)
				{
					Main.PlaySound(29, -1, -1, 92, 1f, 0f);
				}
				else if (Timer == 960)
				{
					Main.PlaySound(29, -1, -1, 104);
				}
				if (Timer >= 600 && Timer < 780)
				{
					for (int k = 0; k < 5; k++)
					{
						int dust = Dust.NewDust(SkullPos - new Vector2(80f, 80f), 160, 160, Bluemagic.Instance.DustType("Smoke"), 0f, 0f, 0, Color.Black);
						Main.dust[dust].scale = 2.5f;
						Main.dust[dust].noLight = true;
					}
				}
				if (Timer >= 780)
				{
					for (int k = 0; k < 1; k++)
					{
						int dust = Dust.NewDust(SkullPos - new Vector2(80f, 80f) + new Vector2(26f, 58f), 36, 16, Bluemagic.Instance.DustType("Smoke"), 0f, 0f, 0, Color.Black);
						Main.dust[dust].scale = 2.5f;
						Main.dust[dust].noLight = true;
						dust = Dust.NewDust(SkullPos - new Vector2(80f, 80f) + new Vector2(98f, 58f), 36, 16, Bluemagic.Instance.DustType("Smoke"), 0f, 0f, 0, Color.Black);
						Main.dust[dust].scale = 2.5f;
						Main.dust[dust].noLight = true;
					}
				}
			}
			if (Main.netMode != 1)
			{
				if (Timer == 100)
				{
					Music("Music - Fallen Blood - by Phyrnna, for Epic Battle Fantasy 4");
				}
				if (Timer == 300)
				{
					MeganTalk("I'm... all alone again...");
				}
				if (Timer == 600)
				{
					JoyceTalk("At long last... freedom!");
				}
				if (Timer == 660)
				{
					MeganTalk("Who... was that?");
				}
				if (Timer == 780)
				{
					index[5] = NPC.NewNPC((int)SkullPos.X, (int)SkullPos.Y + 80, types[5], index[4]);
					JoyceTalk("I would thank you, player, but you shall be the first thing I destroy with my newfound power.");
				}
			}
			if (Timer >= 780 && index[5] > -1)
			{
				Main.npc[index[5]].Bottom = SkullPos + new Vector2(0f, 80f);
			}
			if (Timer >= 900 && Timer < 960)
			{
				BoneLTRot -= MathHelper.TwoPi / 30f;
				BoneLBRot += MathHelper.TwoPi / 30f;
				BoneRTRot += MathHelper.TwoPi / 30f;
				BoneRBRot -= MathHelper.TwoPi / 30f;
			}
			if (Timer == 960)
			{
				BoneLTRot = -MathHelper.Pi / 4f;
				BoneLBRot = MathHelper.Pi / 4f;
				BoneRTRot = MathHelper.Pi / 4f;
				BoneRBRot = -MathHelper.Pi / 4f;
			}
			if (Timer >= 960 && Timer <= 990)
			{
				BoneLTPos += new Vector2(100f / 30f, -100f / 30f);
				BoneLBPos += new Vector2(100f / 30f, 100f / 30f);
				BoneRTPos += new Vector2(-100f / 30f, -100f / 30f);
				BoneRBPos += new Vector2(-100f / 30f, 100f / 30f);
			}
			if (Timer == 990)
			{
				BoneLTPos = SkullPos + new Vector2(-ArenaSize * 0.3f, 0f);
				BoneLBPos = SkullPos + new Vector2(-ArenaSize * 0.3f, 0f);
				BoneRTPos = SkullPos + new Vector2(ArenaSize * 0.3f, 0f);
				BoneRBPos = SkullPos + new Vector2(ArenaSize * 0.3f, 0f);
			}
		}

		internal static int GetTarget()
		{
			if (Players[Main.myPlayer])
			{
				return Main.myPlayer;
			}
			for (int k = 0; k < 255; k++)
			{
				if (Players[k])
				{
					return k;
				}
			}
			return 255;
		}

		internal static void AddBullet(Bullet bullet, float damageMult = 1f)
		{
			bullet.Damage *= damageMult;
			bullets.Add(bullet);
		}

		private static void Music(string message)
		{
			if (Main.netMode != 2)
			{
				Main.NewText(message);
			}
			else
			{
				NetworkText text = NetworkText.FromLiteral(message);
				NetMessage.BroadcastChatMessage(text, Color.White);
			}
		}

		private static void Talk(string name, string message, byte r, byte g, byte b)
		{
			if (Main.netMode != 2)
			{
				string text = Language.GetTextValue("Mods.Bluemagic.NPCTalk", name, message);
				Main.NewText(text, r, g, b);
			}
			else
			{
				NetworkText text = NetworkText.FromKey("Mods.Bluemagic.NPCTalk", name, message);
				NetMessage.BroadcastChatMessage(text, new Color(r, g, b));
			}
		}

		private static void BlushieTalk(string message)
		{
			Talk("blushiemagic", message, 200, 255, 255);
		}

		internal static void KylieTalk(string message)
		{
			Talk("blushiemagic (K)", message, 0, 128, 255);
		}

		internal static void AnnaTalk(string message)
		{
			Talk("blushiemagic (A)", message, 255, 128, 128);
		}

		internal static void LunaTalk(string message)
		{
			Talk("blushiemagic (L)", message, 128, 0, 128);
		}

		internal static void ChrisTalk(string message)
		{
			Talk("blushiemagic (C)", message, 255, 255, 0);
		}

		internal static void MeganTalk(string message)
		{
			Talk("blushiemagic (M)", message, 0, 255, 0);
		}

		internal static void JoyceTalk(string message)
		{
			Talk("blushiemagic (J)", message, 127, 0, 0);
		}

		internal static void DrawArena(SpriteBatch spriteBatch)
		{
			const int blockSize = 16;
			int centerX = (int)Origin.X;
			int centerY = (int)Origin.Y;
			Texture2D outlineTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BlushieBlockOutline");
			Texture2D blockTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BlushieBlock");
			for (int x = centerX - ArenaSize - blockSize / 2; x <= centerX + ArenaSize + blockSize / 2; x += blockSize)
			{
				int y = centerY - ArenaSize - blockSize / 2;
				Vector2 drawPos = new Vector2(x - blockSize / 2, y - blockSize / 2) - Main.screenPosition;
				spriteBatch.Draw(outlineTexture, drawPos, Color.White);
				spriteBatch.Draw(blockTexture, drawPos, Color.White * 0.75f);
				drawPos.Y += 2 * ArenaSize + blockSize;
				spriteBatch.Draw(outlineTexture, drawPos, Color.White);
				spriteBatch.Draw(blockTexture, drawPos, Color.White * 0.75f);
			}
			for (int y = centerY - ArenaSize - blockSize / 2; y <= centerY + ArenaSize + blockSize / 2; y += blockSize)
			{
				int x = centerX - ArenaSize - blockSize / 2;
				Vector2 drawPos = new Vector2(x - blockSize / 2, y - blockSize / 2) - Main.screenPosition;
				spriteBatch.Draw(outlineTexture, drawPos, Color.White);
				spriteBatch.Draw(blockTexture, drawPos, Color.White * 0.75f);
				drawPos.X += 2 * ArenaSize + blockSize;
				spriteBatch.Draw(outlineTexture, drawPos, Color.White);
				spriteBatch.Draw(blockTexture, drawPos, Color.White * 0.75f);
			}
		}

		internal static void DrawBullets(SpriteBatch spriteBatch)
		{
			foreach (Bullet bullet in bullets)
			{
				spriteBatch.Draw(bullet.Texture, bullet.Position - new Vector2(bullet.Size) - Main.screenPosition, Color.White);
			}
		}
	}
}