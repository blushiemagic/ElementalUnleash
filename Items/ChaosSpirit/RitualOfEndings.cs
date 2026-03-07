using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.ChaosSpirit
{
    public class RitualOfEndings : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Brings the wrath of the End unto the world"
                + "\nCan be reused infinitely");*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 50, 0, 0);
            Item.rare = ItemRarityID.Purple;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item44;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(Mod.Find<ModNPC>("PuritySpirit").Type) && !NPC.AnyNPCs(Mod.Find<ModNPC>("ChaosSpirit").Type) && !NPC.AnyNPCs(Mod.Find<ModNPC>("ChaosSpirit2").Type) && !NPC.AnyNPCs(Mod.Find<ModNPC>("ChaosSpirit3").Type);
        }

        public override bool? UseItem(Player player)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC(player.GetSource_FromThis(), (int)player.Center.X, (int)player.Center.Y - 240, Mod.Find<ModNPC>("ChaosSpirit").Type);
            }
            return true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "InfinityCrystal");
            recipe.AddIngredient(null, "FoulOrb", 5);
            recipe.AddRecipeGroup(RecipeGroupID.Wood, 500);
            recipe.AddIngredient(ItemID.FallenStar, 99);
            recipe.AddIngredient(ItemID.FossilOre, 40);
            recipe.AddIngredient(null, "ChaoticSoul", 20);
            recipe.AddIngredient(ItemID.LivingFireBlock, 100);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 10);
            recipe.AddIngredient(ItemID.ShroomiteBar, 10);
            if (Bluemagic.Calamity == null)
            {
                recipe.AddIngredient(ItemID.LunarBar, 20);
            }
            else
            {
                recipe.AddIngredient(Bluemagic.Calamity.Find<ModItem>("CosmiliteBar").Type, 10);
            }
            if (Bluemagic.Thorium != null)
            {
                recipe.AddIngredient(Bluemagic.Thorium.Find<ModItem>("OceanEssence").Type, 3);
                recipe.AddIngredient(Bluemagic.Thorium.Find<ModItem>("DeathEssence").Type, 3);
                recipe.AddIngredient(Bluemagic.Thorium.Find<ModItem>("InfernoEssence").Type, 3);
            }
            recipe.Register();
        }
    }
}