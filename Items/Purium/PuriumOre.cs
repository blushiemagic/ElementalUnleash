using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium
{
	public class PuriumOre : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Flowing with power'");
		}

		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.rare = 11;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.createTile = mod.TileType("PuriumOre");
		}
	}
}