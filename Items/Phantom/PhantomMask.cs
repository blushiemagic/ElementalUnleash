using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom
{
	public class PhantomMask : ModItem
	{
		public override bool Autoload(ref string name, ref string texture, IList<EquipType> equips)
		{
			equips.Add(EquipType.Head);
			return true;
		}

		public override void SetDefaults()
		{
			item.name = "Phantom Mask";
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