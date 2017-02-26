using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit
{
	public class InfinityCrystal : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Infinity Crystal";
			item.width = 28;
			item.height = 28;
			item.maxStack = 99;
			item.rare = 11;
			item.value = Item.sellPrice(1, 0, 0, 0);
		}

		public override DrawAnimation GetAnimation()
		{
			return new DrawAnimationVertical(5, 4);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}