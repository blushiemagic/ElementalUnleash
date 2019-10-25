using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit
{
    public class CleanserBeam : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Cleanse your foes, one line at a time."
                + "\nWipes out everything within the beam."
                + "\nNo enemy armor can survive the destruction!");
        }

        public override void SetDefaults()
        {
            item.damage = 212;
            item.ranged = true;
            item.width = 64;
            item.height = 24;
            item.useTime = 20;
            item.useAnimation = 20;
            item.UseSound = SoundID.Item13;
            item.channel = true;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.useStyle = 5;
            item.knockBack = 1f;
            item.value = Item.sellPrice(0, 50, 0, 0);
            item.rare = 11;
            item.expert = true;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("CleanserBeam");
            item.shootSpeed = 14f;
        }

        public override void AddRecipes()
        {
            if (Bluemagic.Sushi != null)
            {
                ModRecipe recipe;

                recipe = new ModRecipe(mod);
                recipe.AddIngredient(null, "DanceOfBlades");
                recipe.AddIngredient(Bluemagic.Sushi.ItemType("SwapToken"));
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.SetResult(this);
                recipe.AddRecipe();

                recipe = new ModRecipe(mod);
                recipe.AddIngredient(null, "PrismaticShocker");
                recipe.AddIngredient(Bluemagic.Sushi.ItemType("SwapToken"));
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.SetResult(this);
                recipe.AddRecipe();

                recipe = new ModRecipe(mod);
                recipe.AddIngredient(null, "VoidEmblem");
                recipe.AddIngredient(Bluemagic.Sushi.ItemType("SwapToken"));
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }
}