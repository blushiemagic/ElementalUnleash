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
            /* Tooltip.SetDefault("Cleanse your foes, one line at a time."
                + "\nWipes out everything within the beam."
                + "\nNo enemy armor can survive the destruction!");*/
        }

        public override void SetDefaults()
        {
            Item.damage = 212;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 64;
            Item.height = 24;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.UseSound = SoundID.Item13;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 1f;
            Item.value = Item.sellPrice(0, 50, 0, 0);
            Item.rare = ItemRarityID.Purple;
            Item.expert = true;
            Item.autoReuse = false;
            Item.shoot = Mod.Find<ModProjectile>("CleanserBeam").Type;
            Item.shootSpeed = 14f;
        }

        public override void AddRecipes()
        {
            if (Bluemagic.Sushi != null)
            {
                Recipe recipe;

                recipe = CreateRecipe();
                recipe.AddIngredient(null, "DanceOfBlades");
                recipe.AddIngredient(Bluemagic.Sushi.Find<ModItem>("SwapToken").Type);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.Register();

                recipe = CreateRecipe();
                recipe.AddIngredient(null, "PrismaticShocker");
                recipe.AddIngredient(Bluemagic.Sushi.Find<ModItem>("SwapToken").Type);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.Register();

                recipe = CreateRecipe();
                recipe.AddIngredient(null, "VoidEmblem");
                recipe.AddIngredient(Bluemagic.Sushi.Find<ModItem>("SwapToken").Type);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.Register();
            }
        }
    }
}