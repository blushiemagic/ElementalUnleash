using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Dusts
{
	public class PurpleLightning : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			updateType = 226;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			Color newColor = Color.Lerp(lightColor, Color.White, 0.8f);
			return new Color((int)newColor.R, (int)newColor.G, (int)newColor.B, 25);
		}
	}
}