using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Salt
{
	public class PinkSalt : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Pink and sweet, but still filled with your rage");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 14;
			item.maxStack = 999;
			item.rare = 8;
			item.value = 100;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod, "Salt", 4);
			recipe.AddIngredient(ItemID.PixieDust);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this, 5);
		}
	}
}