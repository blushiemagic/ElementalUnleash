using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
	public class CrystalVeil : ModItem
	{
		public override bool Autoload(ref string name, ref string texture, IList<EquipType> equips)
		{
			equips.Add(EquipType.Neck);
			return base.Autoload(ref name, ref texture, equips);
		}

		public override void SetDefaults()
		{
			item.name = "Crystal Veil";
			item.toolTip = "Causes crystals to fall and increases length of invincibility after taking damage";
			item.width = 16;
			item.height = 24;
			item.accessory = true;
			item.rare = 11;
			item.value = Item.sellPrice(0, 25, 0, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>(mod);
			modPlayer.crystalCloak = true;
			player.longInvince = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StarVeil);
			recipe.AddIngredient(null, "InfinityCrystal");
			recipe.AddTile(null, "ElementalPurge");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}