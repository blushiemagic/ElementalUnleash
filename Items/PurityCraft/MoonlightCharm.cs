using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class MoonlightCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Provides huge life regeneration and greatly reduces the cooldown of healing potions"
				+ "\nIncreases pickup range and effectiveness of hearts");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 24;
			item.accessory = true;
			item.lifeRegen = 10;
			item.rare = 11;
			item.value = Item.sellPrice(0, 25, 0, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
			modPlayer.lifeMagnet2 = true;
			player.pStone = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CharmofMyths);
			recipe.AddIngredient(null, "InfinityCrystal", 2);
			recipe.AddTile(null, "PuriumAnvil");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}