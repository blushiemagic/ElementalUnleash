using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Bluemagic.Items.TerraSpirit
{
	public class RainbowStar : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Activates [c/FF0000:G][c/FF7700:O][c/FFFF00:D][c/00FF00:M][c/0000FF:O][c/7700FF:D][c/FF00FF:E]");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 26;
			item.value = Item.buyPrice(10, 0, 0, 0);
			item.rare = 12;
			item.accessory = true;
			item.defense = 1337;
			item.lifeRegen = 400;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<BluemagicPlayer>(mod).godmode = true;
			if (player.endurance < 1f)
			{
				player.endurance = 1f;
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}