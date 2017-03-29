using System;
using Terraria;
using TAPI;

namespace Bluemagic.Items
{
	public class PuriumVisor : ModItem
	{
		public override void Effects(Player player)
		{
			player.rangedDamage += 0.05f;
			player.rangedCrit += 5;
			player.ammoCost80 = true;
		}

		public override void ArmorSetBonus(Player player)
		{
			player.statDefense += 10;
			player.rangedDamage += 0.1f;
			player.rangedCrit += 5;
			player.setBonus = "+10 defense, Greatly increased ranged capabilities";
		}

		public override bool IsArmorSet(Player player, bool vanity)
		{
			return !vanity && player.armor[1].type == Bluemagic.ItemID("PuriumBreastplate") && player.armor[2].type == Bluemagic.ItemID("PuriumLeggings");
		}
	}
}