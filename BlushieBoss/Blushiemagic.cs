using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.BlushieBoss
{
	public class Blushiemagic : BlushiemagicBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			this.music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Shelter");
		}

		public override void AI()
		{
			if (BlushieBoss.Phase == 0)
			{
				if (Main.netMode != 1)
				{
					BlushieBoss.Initialize(npc);
				}
				else
				{
					return;
				}
			}
			
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("BlushieBoss/Wing");
			Vector2 drawPos = new Vector2(npc.position.X + npc.width / 2, npc.position.Y + npc.height * 3 / 4) - Main.screenPosition;
			float scale = (BlushieBoss.Timer - 60f) / 90f;
			if (scale < 0f)
			{
				scale = 0f;
			}
			if (scale > 1f)
			{
				scale = 1f;
			}
			float baseRot = MathHelper.Pi / 5f;
			float rotate = (BlushieBoss.Timer - 180f) / 120f;
			if (rotate < 0f)
			{
				rotate = 0f;
			}
			if (rotate > 1f)
			{
				rotate = 1f;
			}
			rotate *= MathHelper.Pi * 0.4f;
			for (int k = 0; k <= 3; k++)
			{
				float rot = rotate * k / 3f;
				spriteBatch.Draw(texture, drawPos, null, Color.White, baseRot - rot, new Vector2(0f, 34f), new Vector2(scale, 1f), SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, drawPos, null, Color.White, -baseRot + rot, new Vector2(100f, 34f), new Vector2(scale, 1f), SpriteEffects.FlipHorizontally, 0f);
			}
			return true;
		}
	}
}