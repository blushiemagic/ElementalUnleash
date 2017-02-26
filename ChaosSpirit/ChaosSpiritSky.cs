using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace Bluemagic.ChaosSpirit
{
	public class ChaosSpiritSky : CustomSky
	{
		private bool isActive = false;
		private float intensity = 0f;
		private int chaosSpiritIndex;
		private bool stage2;
		private bool stage3;
		private bool finish;
		private Texture2D texture;
		private Texture2D explosionTexture;
		private float pressure = 0f;
		private float light = 0f;
		private List<SkyChaosOrb> orbs = new List<SkyChaosOrb>();
		private List<SkyChaosFracture> fractures = new List<SkyChaosFracture>();

		public override void OnLoad()
		{
			texture = ModLoader.GetMod("Bluemagic").GetTexture("ChaosSpirit/ChaosOrb");
			explosionTexture = TextureManager.Load("Images/Misc/MoonExplosion/Explosion");
		}

		public override void Update(GameTime gameTime)
		{
			if (isActive && intensity < 1f)
			{
				intensity += 0.01f;
			}
			else if (!isActive && intensity > 0f)
			{
				intensity -= 0.01f;
			}
			if (Main.gamePaused || !Main.hasFocus)
			{
				return;
			}
			if (UpdateChaosSpiritIndex() && (stage3 || Main.npc[chaosSpiritIndex].ai[1] == 11f))
			{
				pressure += 0.005f;
				if (pressure > 1f)
				{
					pressure = 1f;
				}
				if (finish)
				{
					light += 0.005f;
					if (light > 1f)
					{
						light = 1f;
					}
				}
				else
				{
					light = 0f;
				}
			}
			else
			{
				pressure = 0f;
				light = 0f;
			}
			if (!stage3 && Main.rand.Next(stage2 ? 60 : 20) == 0)
			{
				orbs.Add(new SkyChaosOrb());
			}
			if (orbs.Count > 0 && orbs[0].position.Y < -32f)
			{
				orbs.RemoveAt(0);
			}
			foreach (SkyChaosOrb orb in orbs)
			{
				orb.Update();
			}
			if (stage2 && Main.rand.Next(120) == 0)
			{
				fractures.Add(new SkyChaosFracture());
			}
			if (fractures.Count > 0 && fractures[0].width <= 0f)
			{
				fractures.RemoveAt(0);
			}
			foreach (SkyChaosFracture fracture in fractures)
			{
				fracture.Update();
			}
		}

		private bool UpdateChaosSpiritIndex()
		{
			stage2 = false;
			stage3 = false;
			finish = false;
			int chaosSpiritType = ModLoader.GetMod("Bluemagic").NPCType("ChaosSpirit");
			int chaosSpiritType2 = ModLoader.GetMod("Bluemagic").NPCType("ChaosSpirit2");
			int chaosSpiritType3 = ModLoader.GetMod("Bluemagic").NPCType("ChaosSpirit3");
			if (chaosSpiritIndex >= 0 && Main.npc[chaosSpiritIndex].active && (Main.npc[chaosSpiritIndex].type == chaosSpiritType || Main.npc[chaosSpiritIndex].type == chaosSpiritType2 || Main.npc[chaosSpiritIndex].type == chaosSpiritType3))
			{
				if (Main.npc[chaosSpiritIndex].type == chaosSpiritType2)
				{
					stage2 = true;
				}
				if (Main.npc[chaosSpiritIndex].type == chaosSpiritType3)
				{
					stage3 = true;
					if (Main.npc[chaosSpiritIndex].ai[0] == 10f)
					{
						finish = true;
					}
				}
				return true;
			}
			chaosSpiritIndex = -1;
			for (int i = 0; i < Main.npc.Length; i++)
			{
				if (Main.npc[i].active && (Main.npc[i].type == chaosSpiritType || Main.npc[i].type == chaosSpiritType2 || Main.npc[i].type == chaosSpiritType3))
				{
					chaosSpiritIndex = i;
					if (Main.npc[i].type == chaosSpiritType2)
					{
						stage2 = true;
					}
					if (Main.npc[i].type == chaosSpiritType3)
					{
						stage3 = true;
						if (Main.npc[i].ai[0] == 10f)
						{
							finish = true;
						}
					}
					break;
				}
			}
			return chaosSpiritIndex >= 0;
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= 0 && minDepth < 0)
			{
				if (light < 1f)
				{
					spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color((byte)(150 * (1f - pressure)), 0, 0) * intensity);
					float decorAlpha = intensity * (1f - pressure);
					foreach (SkyChaosFracture fracture in fractures)
					{
						fracture.Draw(spriteBatch, decorAlpha);
					}
					foreach (SkyChaosOrb orb in orbs)
					{
						orb.Draw(spriteBatch, texture, decorAlpha);
					}
				}
				if (light > 0f)
				{
					spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * light * intensity);
				}
			}
		}

		public override float GetCloudAlpha()
		{
			return 0f;
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			isActive = true;
		}

		public override void Deactivate(params object[] args)
		{
			isActive = false;
		}

		public override void Reset()
		{
			isActive = false;
		}

		public override bool IsActive()
		{
			return isActive || intensity > 0f;
		}
	}

	class SkyChaosOrb
	{
		internal Vector2 position;
		internal float t;
		internal float amplitude;
		internal float frequency;
		internal float scale;
		internal Color color;

		internal SkyChaosOrb()
		{
			this.position = new Vector2(Main.rand.Next(Main.screenWidth) - 32, Main.screenHeight + 32f);
			this.t = MathHelper.TwoPi * Main.rand.NextFloat();
			this.amplitude = Main.rand.Next(32, 160);
			this.frequency = 1f / Main.rand.Next(30, 240);
			this.t /= this.frequency;
			this.scale = 0.5f + 0.5f * Main.rand.NextFloat();
			if (Main.rand.Next(3) == 0)
			{
				this.scale *= 2f;
			}
			this.color = ChaosSpirit.RandomOrbColor();
		}

		internal void Update()
		{
			position.Y -= 2f;
			t += 1f;
		}

		internal void Draw(SpriteBatch spriteBatch, Texture2D texture, float intensity)
		{
			Vector2 drawPos = position;
			drawPos.X += amplitude * (float)Math.Sin(frequency * t);
			Main.spriteBatch.Draw(texture, drawPos, null, color * intensity, 0f, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
		}
	}

	class SkyChaosFracture
	{
		internal float x;
		internal float width;

		internal SkyChaosFracture()
		{
			this.x = Main.rand.Next(32, Main.screenWidth - 32);
			this.width = 64f;
		}

		internal void Update()
		{
			this.width -= 0.5f;
		}

		internal void Draw(SpriteBatch spriteBatch, float intensity)
		{
			spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)(x - width / 2f), 0, (int)width, Main.screenHeight), Color.White * intensity * (width / 96f));
		}
	}
}