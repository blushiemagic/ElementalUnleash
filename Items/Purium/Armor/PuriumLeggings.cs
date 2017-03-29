using System;
using Terraria;
using TAPI;

namespace Bluemagic.Items
{
	public class PuriumLeggings : ModItem
	{
		public override void Effects(Player player)
		{
			player.moveSpeed += 0.08f;
		}
	}
}