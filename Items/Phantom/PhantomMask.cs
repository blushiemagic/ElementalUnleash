using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom
{
	[AutoloadEquip(EquipType.Head)]
	public class PhantomMask : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 20;
			item.rare = 1;
			item.vanity = true;
		}

		public override bool DrawHead()
		{
			return false;
		}
	}
}