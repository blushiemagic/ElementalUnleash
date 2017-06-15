using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace Bluemagic.TerraSpirit
{
	public class TerraSpiritSky : CustomSky
	{
		private bool isActive = false;
		private float intensity = 0f;
		private int terraSpiritIndex;

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
		}

		private bool UpdateTerraSpiritIndex()
		{
			int terraSpiritType = ModLoader.GetMod("Bluemagic").NPCType("TerraSpirit");
			if (terraSpiritIndex >= 0 && Main.npc[terraSpiritIndex].active && Main.npc[terraSpiritIndex].type == terraSpiritType)
			{
				return true;
			}
			terraSpiritIndex = -1;
			for (int i = 0; i < Main.npc.Length; i++)
			{
				if (Main.npc[i].active && Main.npc[i].type == terraSpiritType)
				{
					terraSpiritIndex = i;
					break;
				}
			}
			return terraSpiritIndex >= 0;
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= 0 && minDepth < 0)
			{
				spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(200, 200, 200) * intensity);
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
}