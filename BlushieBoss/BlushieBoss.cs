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
		private static int[] index = new int[] { -1, -1 };

		private static int[] types = new int[2];
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
			types[1] = Bluemagic.Instance.NPCType("BlushiemagicM");
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
			int npc = NPC.NewNPC((int)Origin.X, (int)Origin.Y, types[1]);
			Main.npc[npc].Center = Origin;
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