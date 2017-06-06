using System;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
	public class HorrorDrop : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Red as blood");
		}

		public override void SetDefaults()
		{
			item.width = 8;
			item.height = 8;
			item.maxStack = 999;
			item.rare = 3;
			item.value = 1000;
		}
	}
}