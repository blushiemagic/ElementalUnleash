using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.BlushieBoss
{
	public class BlushiemagicA : BlushiemagicBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("blushiemagic (A)");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			this.music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Phyrnna - Return of the Snow Queen");
		}

		public override void AI()
		{
			if (BlushieBoss.Timer >= 390)
			{
				for (int k = 0; k < 5; k++)
				{
					float x = 12f + 60f * Main.rand.NextFloat();
					float y = 16f - 0.5f * x;
					if (Main.rand.Next(2) == 0)
					{
						x *= -1f;
					}
					x -= 2f;
					int dust = Dust.NewDust(npc.Center + new Vector2(x, y), 0, 0, 6, 0f, 0f, 0, default(Color), 4f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity.Y = 0.5f - 8f * Main.rand.NextFloat();
					Main.dust[dust].velocity.X += 0.1f * x;
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return true;
		}
	}
}