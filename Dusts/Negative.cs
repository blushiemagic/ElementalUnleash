using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Dusts
{
	public class Negative : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
		}
	}
}