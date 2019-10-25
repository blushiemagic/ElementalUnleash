using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit
{
    public class DanceOfBlades : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Dance the dance of death."
                + "\nPurifies the screen of enemies.");
        }

        public override void SetDefaults()
        {
            item.damage = 500;
            item.melee = true;
            item.width = 80;
            item.height = 80;
            item.useTime = 10;
            item.useAnimation = 10;
            item.channel = true;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.useStyle = 100;
            item.knockBack = 8f;
            item.value = Item.sellPrice(0, 50, 0, 0);
            item.rare = 11;
            item.expert = true;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("DanceOfBlades");
            item.shootSpeed = 0f;
        }

        public override bool UseItemFrame(Player player)
        {
            player.bodyFrame.Y = 3 * player.bodyFrame.Height;
            return true;
        }

        public override void AddRecipes()
        {
            if (Bluemagic.Sushi != null)
            {
                ModRecipe recipe;

                recipe = new ModRecipe(mod);
                recipe.AddIngredient(null, "CleanserBeam");
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