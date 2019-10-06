using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace Bluemagic
{
	public class WorldReaverData
	{
		//Shatter sound: Shatter, Item27, Item107
		internal static WorldReaverData instance = null;
		private static Random rand = new Random();
		private int owner;
		private int timer;
		private List<LineSegment> cracks;

		public int Timer
		{
			get
			{
				return timer;
			}
		}

		public IEnumerable<LineSegment> Cracks
		{
			get
			{
				return cracks;
			}
		}

		public WorldReaverData(int player)
		{
			this.owner = player;
			this.timer = 0;
			this.cracks = new List<LineSegment>();
		}

		public static void Begin(int player)
		{
			if (instance == null && !Main.dedServ)
			{
				instance = new WorldReaverData(player);
				Overlays.Scene.Activate("Bluemagic:WorldReaver");
				Filters.Scene.Activate("Bluemagic:WorldReaver");
			}
		}

		public static void Update()
		{
			if (instance != null && !Main.dedServ)
			{
				instance.UpdateInstance();
				if (instance.timer >= 300)
				{
					Main.PlaySound(SoundID.Item107);
					Overlays.Scene.Deactivate("Bluemagic:WorldReaver");
					Filters.Scene.Deactivate("Bluemagic:WorldReaver");
					Filters.Scene["Bluemagic:WorldReaver"].Opacity = 0f;
					instance = null;
				}
			}
		}

		private void UpdateInstance()
		{
			timer++;
			if (timer == 1)
			{
				Damage(666, false);
			}
			if (timer == 60)
			{
				Main.PlaySound(SoundID.Item14);
				Damage(6666, false);
			}
			if (timer == 120 || timer == 180 || timer == 240)
			{
				float maxLength = (float)Math.Sqrt(Main.screenWidth * Main.screenWidth + Main.screenHeight * Main.screenHeight);
				float minLength = maxLength * 0.2f;
				for (var k = 0; k < 15; k++)
				{
					float x = (float)rand.NextDouble() * Main.screenWidth;
					float y = (float)rand.NextDouble() * Main.screenHeight;
					float angle = (float)rand.NextDouble() * MathHelper.Pi;
					float length = minLength + (float)rand.NextDouble() * (maxLength - minLength);
					cracks.Add(new LineSegment(new Vector2(x, y), angle, length));
				}
				Main.PlaySound(SoundID.Item27);
				Damage(33333, true);
			}
			if (timer == 300)
			{
				for (var k = 0; k < 5; k++)
				{
					Damage(333333, true);
				}
			}
			Filters.Scene["Bluemagic:WorldReaver"].GetShader().UseProgress(timer);
		}

		private void Damage(int damage, bool crit)
		{
			if (Main.myPlayer != owner)
			{
				return;
			}
			for (int k = 0; k < 200; k++)
			{
				if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5)
				{
					int npcDamage = damage + Main.npc[k].defense / 2;
					double damageDealt = Main.npc[k].StrikeNPC(npcDamage, 0f, 0, crit);
					if (Main.player[owner].accDreamCatcher)
					{
						Main.player[owner].addDPS((int)damageDealt);
					}
					if (Main.netMode != 0)
					{
						NetMessage.SendData(28, -1, -1, null, k, npcDamage, 0f, 0f, crit ? 1 : 0, 0, 0);
					}
				}
			}
		}
	}

	public class WorldReaverOverlay : Overlay
	{
		public WorldReaverOverlay() : base(EffectPriority.VeryHigh, RenderLayers.All) {}

		public override void Update(GameTime gameTime)
		{
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Texture2D texture = Bluemagic.Instance.GetTexture("Pixel");
			int timer = WorldReaverData.instance.Timer;
			float mainAngle = (float)Math.Atan(-2);
			if (timer < 60)
			{
				int bottom = (int)(Main.screenHeight * timer / 24f);
				Vector2 top = new Vector2(Main.screenWidth / 2f + Main.screenHeight / 4f, -16f);
				for (int k = 2; k <= 32; k += 2)
				{
					int limit = bottom - 16 * (k - 2);
					if (limit <= -16)
					{
						break;
					}
					float length = (float)Math.Sqrt(1.25f * limit * limit);
					spriteBatch.Draw(texture, top, null, Color.White, mainAngle, new Vector2(1f, 0.5f), new Vector2(length, k), SpriteEffects.None, 0f);
				}
			}
			else
			{
				spriteBatch.Draw(texture, new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), null, Color.White, mainAngle, new Vector2(0.5f, 0.5f), new Vector2(Main.screenWidth, 32f), SpriteEffects.None, 0f);
			}
			foreach (LineSegment crack in WorldReaverData.instance.Cracks)
			{
				spriteBatch.Draw(texture, crack.Center, null, Color.White * 0.5f, crack.Angle, new Vector2(0.5f, 0.5f), new Vector2(crack.Length, 8f), SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, crack.Center, null, Color.White * 0.5f, crack.Angle, new Vector2(0.5f, 0.5f), new Vector2(crack.Length + 8f, 4f), SpriteEffects.None, 0f);
			}
			if (timer > 270)
			{
				float alpha = (timer - 270) / 30f;
				if (alpha > 1f)
				{
					alpha = 1f;
				}
				spriteBatch.Draw(texture, Vector2.Zero, null, Color.White * alpha, 0f, Vector2.Zero, new Vector2(Main.screenWidth, Main.screenHeight), SpriteEffects.None, 0f);
			}
		}

		public override void Activate(Vector2 position, params object[] args) {}

		public override void Deactivate(params object[] args) {}

		public override bool IsVisible()
		{
			return WorldReaverData.instance != null;
		}
	}
}