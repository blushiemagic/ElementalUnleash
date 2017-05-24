using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom
{
	public class PhantomShield : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Phantom's Shield";
			item.width = 32;
			item.height = 32;
			item.toolTip = "Reduces damage taken by 7%";
			item.toolTip2 = "Players on your team receive this bonus as well";
			item.value = Item.buyPrice(0, 10, 0, 0);
			item.rare = 9;
			item.expert = true;
			item.accessory = true;
			item.defense = 6;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.AddBuff(mod.BuffType("PhantomShield"), 5, true);
			if (player.whoAmI != Main.myPlayer && Main.player[Main.myPlayer].team == player.team && player.team != 0)
			{
				Main.player[Main.myPlayer].AddBuff(mod.BuffType("PhantomShield"), 5, true);
			}
		}
	}
}