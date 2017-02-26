using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination
{
	public class DimensionalChest : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Dimensional Chest";
			item.toolTip = "Steals loot from other dimensions";
			item.width = 26;
			item.height = 22;
			item.maxStack = 99;
			item.rare = 8;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.CorruptionKey);
			recipe.SetResult(ItemID.ScourgeoftheCorruptor);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.CrimsonKey);
			recipe.SetResult(ItemID.VampireKnives);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.JungleKey);
			recipe.SetResult(ItemID.PiranhaGun);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.HallowedKey);
			recipe.SetResult(ItemID.RainbowGun);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.FrozenKey);
			recipe.SetResult(ItemID.StaffoftheFrostHydra);
			recipe.AddRecipe();
		}
	}
}