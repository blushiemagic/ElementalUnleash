using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Dusts
{
	public class PuriumBullet : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.velocity = Vector2.Zero;
			dust.noLight = true;
		}

		public override bool Update(Dust dust)
		{
			dust.alpha += 20;
			dust.scale -= 0.04f;
			if (dust.alpha >= 255)
			{
				dust.alpha = 255;
				dust.active = false;
			}
			if (dust.customData is int)
			{
				dust.customData = (int)dust.customData - 1;
				if ((int)dust.customData <= 0)
				{
					dust.active = false;
				}
			}
			return false;
		}
	}
}