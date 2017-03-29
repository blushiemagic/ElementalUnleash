using System;
using Terraria;
using TAPI;

namespace Bluemagic.Items
{
	public class PuriumHeadgear : ModItem
	{
		public override void Effects(Player player)
		{
			player.statManaMax2 += 40;
			player.magicDamage += 0.05f;
			player.magicCrit += 5;
			player.manaCost -= 0.05f;
		}

		public override void ArmorSetBonus(Player player)
		{
			player.statManaMax2 += 40;
			player.magicDamage += 0.1f;
			player.magicCrit += 5;
			player.manaCost -= 0.1f;
			player.setBonus = "+40 maximum mana, Greatly increased magic capabilities";
		}

		public override bool IsArmorSet(Player player, bool vanity)
		{
			return !vanity && player.armor[1].type == Bluemagic.ItemID("PuriumBreastplate") && player.armor[2].type == Bluemagic.ItemID("PuriumLeggings");
		}
	}
}