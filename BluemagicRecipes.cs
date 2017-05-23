using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic
{
	public static class BluemagicRecipes
	{
		public static void AddRecipes(Mod mod)
		{
			AddClentamistationRecipes(mod);

			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.EnchantedNightcrawler);
			recipe.AddIngredient(ItemID.GlowingMushroom, 20);
			recipe.AddIngredient(ItemID.Ectoplasm, 2);
			recipe.AddIngredient(ItemID.DarkBlueSolution, 5);
			recipe.AddTile(TileID.DemonAltar);
			recipe.AddTile(mod.TileType("Clentamistation"));
			recipe.SetResult(ItemID.TruffleWorm);
			recipe.AddRecipe();
		}

		private static void AddClentamistationRecipes(Mod mod)
		{
			int greenDroplet = mod.ItemType("GreenDroplet");
			int blueDroplet = mod.ItemType("BlueDroplet");
			int purpleDroplet = mod.ItemType("PurpleDroplet");
			int darkBlueDroplet = mod.ItemType("DarkBlueDroplet");
			int redDroplet = mod.ItemType("RedDroplet");
			int[] solutionIDs = { ItemID.GreenSolution, ItemID.BlueSolution, ItemID.PurpleSolution, ItemID.DarkBlueSolution, ItemID.RedSolution };
			int[] dropletIDs = { greenDroplet, blueDroplet, purpleDroplet, darkBlueDroplet, redDroplet };
			int[] stoneIDs = { ItemID.StoneBlock, ItemID.PearlstoneBlock, ItemID.EbonstoneBlock, mod.ItemType("Shroomstone"), ItemID.CrimstoneBlock };
			int[] sandIDs = { ItemID.SandBlock, ItemID.PearlsandBlock, ItemID.EbonsandBlock, mod.ItemType("Shroomsand"), ItemID.CrimsandBlock };
			int[] iceIDs = { ItemID.IceBlock, ItemID.PinkIceBlock, ItemID.PurpleIceBlock, mod.ItemType("DarkBlueIce"), ItemID.RedIceBlock };
			int[] woodIDs = { ItemID.Wood, ItemID.Pearlwood, ItemID.Ebonwood, -1, ItemID.Shadewood };
			int[][] itemIDs = { stoneIDs, sandIDs, iceIDs, woodIDs };
			for (int j = 0; j < 5; j++)
			{
				for (int k = 0; k < 5; k++)
				{
					if (j != k)
					{
						for (int x = 0; x < itemIDs.Length; x++)
						{
							if (itemIDs[x][j] > 0 && itemIDs[x][k] > 0)
							{
								AddClentaminationRecipe(mod, itemIDs[x][j], itemIDs[x][k], solutionIDs[j], dropletIDs[j]);
							}
						}
					}
				}
			}
			AddClentaminationRecipe(mod, ItemID.RichMahogany, ItemID.GlowingMushroom, solutionIDs[0], dropletIDs[0]);
			AddClentaminationRecipe(mod, ItemID.GlowingMushroom, ItemID.RichMahogany, solutionIDs[3], dropletIDs[3]);
		}

		private static void AddClentaminationRecipe(Mod mod, int result, int ingredient, int solution, int droplet)
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ingredient, 100);
			recipe.AddIngredient(solution);
			recipe.AddTile(null, "Clentamistation");
			recipe.SetResult(result, 100);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ingredient);
			recipe.AddIngredient(droplet);
			recipe.AddTile(null, "Clentamistation");
			recipe.SetResult(result);
			recipe.AddRecipe();
		}
	}
}