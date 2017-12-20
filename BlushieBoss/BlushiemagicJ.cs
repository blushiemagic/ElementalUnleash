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
			this.music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Phyrnna - Return of the Snow Queen");
		}

		public override void AI()
		{
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return true;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
		}

		public override bool UseSpecialDamage()
		{
			return false;
		}
	}
}