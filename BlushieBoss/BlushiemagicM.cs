using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.BlushieBoss
{
	public class BlushiemagicM : BlushiemagicBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("blushiemagic (M)");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			this.music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Phyrnna - Return of the Snow Queen");
		}

		public override void AI()
		{
			if (BlushieBoss.Timer > 300f)
			{
				for (int k = 0; k < 3; k++)
				{
					float radius = 32f;
					float rotation = Main.rand.NextFloat() * MathHelper.TwoPi;
					Vector2 dustPos = npc.Center + new Vector2(0f, 15f) + radius * rotation.ToRotationVector2();
					int dust = Dust.NewDust(dustPos, 0, 0, mod.DustType("Particle"), 0f, 0f, 0, Color.Green);
					Main.dust[dust].customData = npc;
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return true;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			BlushieBoss.DrawBullets(spriteBatch);
		}
	}
}