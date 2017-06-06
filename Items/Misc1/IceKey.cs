using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
	public class IceKey : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("A key to a different dimension");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 20;
			item.maxStack = 99;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IceBlock, 10);
			recipe.AddIngredient(ItemID.GoldBar, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IceBlock, 10);
			recipe.AddIngredient(ItemID.PlatinumBar, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}