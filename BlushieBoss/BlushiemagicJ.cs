using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.BlushieBoss
{
	public class BlushiemagicJ : BlushiemagicBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("blushiemagic (J)");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.takenDamageMultiplier = 5f;
			this.music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Phyrnna - Return of the Snow Queen");
		}

		public override void AI()
		{
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("BlushieBoss/Skull_Back");
			spriteBatch.Draw(texture, npc.Bottom - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height), 1f, SpriteEffects.None, 0f);
			return true;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("BlushieBoss/Skull");
			Vector2 center = npc.Bottom + new Vector2(0f, -texture.Height / 2);
			spriteBatch.Draw(texture, center - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0f);
			texture = mod.GetTexture("BlushieBoss/Bone");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float alpha = 1f;
			if (BlushieBoss.Timer < 960)
			{
				alpha = (BlushieBoss.Timer - 900) / 60f;
				if (alpha < 0f)
				{
					alpha = 0f;
				}
			}
			Color color = Color.White * alpha;
			spriteBatch.Draw(texture, BlushieBoss.BoneLTPos - Main.screenPosition, null, color, BlushieBoss.BoneLTRot, origin, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, BlushieBoss.BoneLBPos - Main.screenPosition, null, color, BlushieBoss.BoneLBRot, origin, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, BlushieBoss.BoneRTPos - Main.screenPosition, null, color, BlushieBoss.BoneRTRot, origin, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, BlushieBoss.BoneRBPos - Main.screenPosition, null, color, BlushieBoss.BoneRBRot, origin, 1f, SpriteEffects.None, 0f);
		}

		public override bool UseSpecialDamage()
		{
			return false;
		}
	}
}