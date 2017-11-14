using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.BlushieBoss
{
	public class BlushiemagicK : BlushiemagicBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("blushiemagic (K)");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			this.music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Phyrnna - Return of the Snow Queen");
		}

		public override void AI()
		{
			if (BlushieBoss.Timer >= 390 && BlushieBoss.Timer < 600)
			{
				for (int k = 0; k < 5; k++)
				{
					Dust.NewDust(npc.Center - new Vector2(50f, 50f), 100, 100, mod.DustType("Sparkle"), 0f, 0f, 0, new Color(0, 0, 255), 1f);
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (BlushieBoss.Timer >= 600)
			{
				Texture2D texture = mod.GetTexture("BlushieBoss/BlushiemagicK_Back");
				spriteBatch.Draw(texture, npc.Center - Main.screenPosition - new Vector2(texture.Width / 2, texture.Height / 2), Color.White);
			}
			return true;
		}
	}
}