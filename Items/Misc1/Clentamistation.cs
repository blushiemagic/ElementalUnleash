using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
	public class Clentamistation : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Clentamistation";
			item.width = 26;
			item.height = 20;
			item.maxStack = 99;
			item.rare = 5;
			item.value = 2000000;
			item.toolTip = "Clentaminates items";
			item.useStyle = 1;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = mod.TileType("Clentamistation");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ImbuingStation);
			recipe.AddIngredient(ItemID.Clentaminator);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}