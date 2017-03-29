using System;
using Terraria;
using TAPI;

namespace Bluemagic.Items
{
	public class PuriumHelmet : ModItem
	{
		public override void Effects(Player player)
		{
			player.meleeDamage += 0.05f;
			player.meleeCrit += 5;
			player.meleeSpeed += 0.05f;
		}

		public override void ArmorSetBonus(Player player)
		{
			player.statDefense += 20;
			player.meleeDamage += 0.1f;
			player.meleeCrit += 5;
			player.moveSpeed += 0.07f;
			player.meleeSpeed += 0.1f;
			player.setBonus = "+20 defense, Greatly increased melee capabilities";
		}

		public override bool IsArmorSet(Player player, bool vanity)
		{
			return !vanity && player.armor[1].type == Bluemagic.ItemID("PuriumBreastplate") && player.armor[2].type == Bluemagic.ItemID("PuriumLeggings");
		}
	}
}