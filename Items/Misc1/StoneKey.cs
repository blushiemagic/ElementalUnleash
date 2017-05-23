using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
	public class StoneKey : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Stone Key";
			item.width = 14;
			item.height = 20;
			item.toolTip = "A key to a different dimension";
			item.maxStack = 99;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 10);
			recipe.AddIngredient(ItemID.GoldBar, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 10);
			recipe.AddIngredient(ItemID.PlatinumBar, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}