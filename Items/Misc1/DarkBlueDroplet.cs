using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
	public class DarkBlueDroplet : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Dark Blue Droplet";
			item.width = 4;
			item.height = 4;
			item.toolTip = "Used by the Clentamistation";
			item.toolTip2 = "Imbues Glowing Mushrooms";
			item.maxStack = 999;
			item.rare = 3;
			item.value = 25;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DarkBlueSolution);
			recipe.SetResult(this, 100);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 100);
			recipe.SetResult(ItemID.DarkBlueSolution);
			recipe.AddRecipe();
		}
	}
}