using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Dusts
{
	public class CrystalStar : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
		}

		public override bool MidUpdate(Dust dust)
		{
			dust.velocity *= 0.98f;
			if (dust.noLight)
			{
				dust.velocity *= 0.95f;
			}
			float strength = dust.scale * 0.8f;
			if (strength > 1f)
			{
				strength = 1f;
			}
			Lighting.AddLight(dust.position, 0.4f * strength, 0.7f * strength, 0.4f * strength);
			return base.MidUpdate(dust);
		}

		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			float alpha = (float)(255 - dust.alpha) / 255f;
			alpha = (alpha + 3f) / 4f;
			int realAlpha = lightColor.A - dust.alpha;
			if (realAlpha < 0)
			{
				realAlpha = 0;
			}
			if (realAlpha > 255)
			{
				realAlpha = 255;
			}
			return new Color(lightColor.R * alpha, lightColor.G * alpha, lightColor.B * alpha, realAlpha);
		}
	}
}