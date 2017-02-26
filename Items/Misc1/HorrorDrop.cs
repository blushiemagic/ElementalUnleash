using System;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
	public class HorrorDrop : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Horror Drop";
			item.width = 8;
			item.height = 8;
			item.toolTip = "Red as blood";
			item.maxStack = 99;
			item.rare = 3;
			item.value = 1000;
		}
	}
}