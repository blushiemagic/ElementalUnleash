using System;
using Terraria;
using TAPI;

namespace Bluemagic.Items
{
	public class PuriumBreastplate : ModItem
	{
		public override void Effects(Player player)
		{
			player.meleeDamage += 0.05f;
			player.rangedDamage += 0.05f;
			player.magicDamage += 0.05f;
			player.minionDamage += 0.05f;
			player.meleeCrit += 5;
			player.rangedCrit += 5;
			player.magicCrit += 5;
		}

		public override void VanitySetBonus(Player player)
		{
			if (Main.rand.Next(10) == 0)
			{
				BluemagicDust.CreateSparkleAura(player.position, player.width, player.height, Bluemagic.PureColor);
			}
		}

		public override bool IsArmorSet(Player player, bool vanity)
		{
			if (!vanity)
			{
				return false;
			}
			int headSlot = player.armor[8].IsBlank() ? 0 : 8;
			int legSlot = player.armor[10].IsBlank() ? 2 : 10;
			int headType = player.armor[headSlot].type;
			return (headType == Bluemagic.ItemID("PuriumHeadgear") || headType == Bluemagic.ItemID("PuriumHelmet") || headType == Bluemagic.ItemID("PuriumVisor") || headType == Bluemagic.ItemID("PuriumMask")) && player.armor[legSlot].type == Bluemagic.ItemID("PuriumLeggings");
		}
	}
}