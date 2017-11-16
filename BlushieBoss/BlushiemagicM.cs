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
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (BlushieBoss.Timer >= 480)
			{
				Texture2D texture = mod.GetTexture("BlushieBoss/GreenOrb");
				Vector2 draw = npc.Center - Main.screenPosition - new Vector2(texture.Width / 2, texture.Height / 2);
				float offset = BlushieBoss.Timer - 480;
				if (offset > 60f)
				{
					offset = 60f;
				}
				spriteBatch.Draw(texture, draw + new Vector2(-offset, 0f), null, Color.White);
				spriteBatch.Draw(texture, draw + new Vector2(offset, 0f), null, Color.White);
			}
			return true;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (BlushieBoss.Timer < 480)
			{
				Texture2D texture = mod.GetTexture("ChaosSpirit/DissonanceOrb");
				Vector2 drawPos = npc.Center - Main.screenPosition;
				Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
				float rotation = 0.05f * BlushieBoss.Timer;
				spriteBatch.Draw(texture, drawPos, null, Color.White, rotation, origin, 0.5f, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, drawPos, null, Color.White, -rotation, origin, 0.5f, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, drawPos, null, Color.White, -rotation, origin, 0.25f, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, drawPos, null, Color.White, rotation, origin, 0.25f, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, drawPos, null, Color.White, -rotation, origin, 1f, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, drawPos, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
			}
			else
			{
				Texture2D shield = mod.GetTexture("Mounts/PurityShield");
				spriteBatch.Draw(shield, npc.Center - Main.screenPosition - new Vector2(shield.Width / 2, shield.Height / 2), null, Color.White * 0.5f);
			}
			BlushieBoss.DrawBullets(spriteBatch);
		}

		public override bool UseSpecialDamage()
		{
			return false;
		}
	}
}