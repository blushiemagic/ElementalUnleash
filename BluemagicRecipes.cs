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

            Recipe recipe = Recipe.Create(ItemID.TruffleWorm);
            recipe.AddIngredient(ItemID.EnchantedNightcrawler);
            recipe.AddIngredient(ItemID.GlowingMushroom, 20);
            recipe.AddIngredient(ItemID.Ectoplasm, 2);
            recipe.AddIngredient(ItemID.DarkBlueSolution, 5);
            recipe.AddTile(TileID.DemonAltar);
            recipe.AddTile(mod.Find<ModTile>("Clentamistation").Type);
            recipe.Register();
        }

        private static void AddClentamistationRecipes(Mod mod)
        {
            int greenDroplet = mod.Find<ModItem>("GreenDroplet").Type;
            int blueDroplet = mod.Find<ModItem>("BlueDroplet").Type;
            int purpleDroplet = mod.Find<ModItem>("PurpleDroplet").Type;
            int darkBlueDroplet = mod.Find<ModItem>("DarkBlueDroplet").Type;
            int redDroplet = mod.Find<ModItem>("RedDroplet").Type;
            int[] solutionIDs = { ItemID.GreenSolution, ItemID.BlueSolution, ItemID.PurpleSolution, ItemID.DarkBlueSolution, ItemID.RedSolution };
            int[] dropletIDs = { greenDroplet, blueDroplet, purpleDroplet, darkBlueDroplet, redDroplet };
            int[] stoneIDs = { ItemID.StoneBlock, ItemID.PearlstoneBlock, ItemID.EbonstoneBlock, mod.Find<ModItem>("Shroomstone").Type, ItemID.CrimstoneBlock };
            int[] sandIDs = { ItemID.SandBlock, ItemID.PearlsandBlock, ItemID.EbonsandBlock, mod.Find<ModItem>("Shroomsand").Type, ItemID.CrimsandBlock };
            int[] iceIDs = { ItemID.IceBlock, ItemID.PinkIceBlock, ItemID.PurpleIceBlock, mod.Find<ModItem>("DarkBlueIce").Type, ItemID.RedIceBlock };
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
            Recipe recipe = Recipe.Create(result, 100);
            recipe.AddIngredient(ingredient, 100);
            recipe.AddIngredient(solution);
            recipe.AddTile(null, "Clentamistation");
            recipe.Register();

            recipe = Recipe.Create(result);
            recipe.AddIngredient(ingredient);
            recipe.AddIngredient(droplet);
            recipe.AddTile(null, "Clentamistation");
            recipe.Register();
        }
    }
}