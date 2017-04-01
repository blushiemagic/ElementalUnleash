using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
	public class CrystalDestroyerScope : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Crystal Destroyer Scope";
			item.toolTip = "Increases view range for guns (Right click to zoom out)";
			item.toolTip2 = "+25% ranged damage, +25% critical strike chance";
			item.width = 14;
			item.height = 28;
			item.accessory = true;
			item.rare = 11;
			item.value = Item.sellPrice(0, 30, 0, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.scope = true;
			player.rangedDamage += 0.25f;
			player.rangedCrit += 25;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SniperScope);
			recipe.AddIngredient(null, "RangerSeal");
			recipe.AddIngredient(null, "InfinityCrystal");
			recipe.AddTile(null, "ElementalPurge");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}