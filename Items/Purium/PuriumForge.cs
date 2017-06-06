using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium
{
	public class PuriumForge : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Used to smelt elemental ores");
		}

		public override void SetDefaults()
		{
			item.width = 44;
			item.height = 30;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.rare = 11;
			item.value = Item.sellPrice(0, 13, 0, 0);
			item.createTile = mod.TileType("PuriumForge");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.AdamantiteForge);
			recipe.AddIngredient(null, "PuriumOre", 60);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.TitaniumForge);
			recipe.AddIngredient(null, "PuriumOre", 60);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}