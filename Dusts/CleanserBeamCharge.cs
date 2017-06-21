using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Dusts
{
	public class CleanserBeamCharge : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
		}

		public override bool Update(Dust dust)
		{
			Vector2 goal = (Vector2)dust.customData;
			Vector2 offset = goal - dust.position;
			dust.position += offset * 0.1f;
			dust.rotation += offset.X * 0.1f;
			dust.scale *= 0.95f;
			if (Vector2.Distance(goal, dust.position) < 0.1f)
			{
				dust.active = false;
			}
			return false;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}