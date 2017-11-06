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
		private static int[] index = new int[] { -1, -1, -1, -1, -1 };
		private static Vector2 PosK;
		private static Vector2 PosA;
		private static Vector2 PosL;

		private static int[] types = new int[5];
		internal static Texture2D BulletWhiteTexture;
		internal static Texture2D BulletGoldTexture;
		internal static Texture2D BulletGoldLargeTexture;
		internal static Texture2D BulletStarTexture;

		public static bool Active
		{
			get
			{
				return Phase > 0;
			}
		}

		internal static void Load()
		{
			types[0] = Bluemagic.Instance.NPCType("Blushiemagic");
			types[1] = Bluemagic.Instance.NPCType("BlushiemagicK");
			types[2] = Bluemagic.Instance.NPCType("BlushiemagicA");
			types[3] = Bluemagic.Instance.NPCType("BlushiemagicL");
			types[4] = Bluemagic.Instance.NPCType("BlushiemagicM");
			BulletWhiteTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletWhite");
			BulletGoldTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletGold");
			BulletGoldLargeTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletGoldLarge");
			BulletStarTexture = Bluemagic.Instance.GetTexture("BlushieBoss/Star");
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
				Player player = Main.player[GetTarget()];
				for (int k = 0; k < bullets.Count; k++)
				{
					bullets[k].Update();
					if (bullets[k].ShouldRemove())
					{
						bullets[k].Active = false;
						bullets.RemoveAt(k);
						k--;
					}
					else if (Vector2.Distance(player.Center, bullets[k].Position) < bullets[k].Size)
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
					for (int k = 0; k < 16; k++)
					{
						float useRotate = rotate + k * MathHelper.TwoPi / 16f;
						bullets.Add(BulletSimple.NewWhite(Origin, length / 90f * useRotate.ToRotationVector2()));
					}
					for (int k = 0; k < 4; k++)
					{
						float useRotate = rotate + k * MathHelper.PiOver2;
						Vector2 direction = useRotate.ToRotationVector2();
						Vector2 center = Origin + length / 2 * direction;
						bullets.Add(BulletRotate.NewGold(center, length / 2f, useRotate + MathHelper.Pi, MathHelper.TwoPi / 180f, 180));
						bullets.Add(BulletRotate.NewGold(center, length / 2f, useRotate + MathHelper.Pi, -MathHelper.TwoPi / 180f, 180));
					}
				}
				if (Timer >= 1320 && Timer <= 1920 && Timer % 30 == 0)
				{
					Bullet center = BulletTarget.NewGoldLarge(Origin, Main.player[GetTarget()].Center, 60);
					bullets.Add(center);
					for (int k = 0; k < 8; k++)
					{
						float rotate = k / 8f * MathHelper.TwoPi;
						bullets.Add(BulletRelease.NewWhite(center, 60f, rotate, MathHelper.TwoPi / 480f, 4f));
						bullets.Add(BulletRelease.NewWhite(center, 60f, rotate, -MathHelper.TwoPi / 480f, 4f));
					}
				}
				if (Timer >= 2040 && Timer <= 2640 && Timer % 60 == 0)
				{
					Vector2 offset = Main.player[GetTarget()].Center - Origin;
					offset.Normalize();
					offset *= 8f;
					for (int k = 0; k < 5; k++)
					{
						float angle = MathHelper.Pi * 1.5f - k * MathHelper.TwoPi / 5f;
						float nextAngle = MathHelper.Pi * 1.5f - (k + 2) * MathHelper.TwoPi / 5f;
						Vector2 start = angle.ToRotationVector2();
						Vector2 end = nextAngle.ToRotationVector2();
						for (int i = 0; i < 6; i++)
						{
							if (i == 2 || i == 3)
							{
								continue;
							}
							Vector2 adjust = Vector2.Lerp(start, end, i / 6f);
							bullets.Add(BulletBounce.NewStar(Origin, offset + 0.75f * adjust, 5));
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
					AnnaTalk("Hi there! Are you ready to have fun?~");
					LunaTalk("You think you can stand up to me? We shall see...");
				}
			}
			if (Timer < 180)
			{
				return;
			}
			NPC kylie = Main.npc[index[1]];
			NPC anna = Main.npc[index[2]];
			NPC luna = Main.npc[index[3]];
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
			kylie.Center = Origin + PosK;
			anna.Center = Origin + PosA;
			luna.Center = Origin + PosL;
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

		private static void KylieTalk(string message)
		{
			Talk("blushiemagic (K)", message, 0, 128, 255);
		}

		private static void AnnaTalk(string message)
		{
			Talk("blushiemagic (A)", message, 255, 128, 128);
		}

		private static void LunaTalk(string message)
		{
			Talk("blushiemagic (L)", message, 128, 0, 128);
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