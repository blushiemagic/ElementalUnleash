using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Tiles
{
	public abstract class BaseMushroomTile : ModTile
	{
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			float num = Main.rand.Next(28, 42) * 0.005f;
			num += (270f - Main.mouseTextColor) / 1000f;
			r = 0.1f;
			g = 0.2f + num / 2f;
			b = 0.7f + num;
		}

		public override void RandomUpdate(int i, int j)
		{
		}
	}
}