using System;
using Terraria;
using TAPI;

namespace Bluemagic.Items
{
	public class PuriumMask : ModItem
	{
		public override void Effects(Player player)
		{
			player.maxMinions += 2;
			player.minionDamage += 0.15f;
		}

		public override void ArmorSetBonus(Player player)
		{
			player.maxMinions += 2;
			player.minionDamage += 0.3f;
			player.setBonus = "+2 max minions, +30% minion damage";
		}

		public override bool IsArmorSet(Player player, bool vanity)
		{
			return !vanity && player.armor[1].type == Bluemagic.ItemID("PuriumBreastplate") && player.armor[2].type == Bluemagic.ItemID("PuriumLeggings");
		}
	}
}