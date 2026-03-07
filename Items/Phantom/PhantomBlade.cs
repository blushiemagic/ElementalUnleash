using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom
{
    public class PhantomBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Projects a phantom blade when swung");
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 32;
            Item.useTime = 32;
            Item.damage = 102;
            Item.knockBack = 6f;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.rare = ItemRarityID.Yellow;
            Item.DamageType = DamageClass.Melee;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.UseSound = SoundID.Item1;
            Item.shoot = Mod.Find<ModProjectile>("PhantomBlade").Type;
            Item.shootSpeed = 0f;
        }

        public override void AddRecipes()
        {
            if (Bluemagic.Sushi != null)
            {
                Recipe recipe;

                recipe = CreateRecipe();
                recipe.AddIngredient(null, "SpectreGun");
                recipe.AddIngredient(Bluemagic.Sushi.Find<ModItem>("SwapToken").Type);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.Register();

                recipe = CreateRecipe();
                recipe.AddIngredient(null, "PhantomSphere");
                recipe.AddIngredient(Bluemagic.Sushi.Find<ModItem>("SwapToken").Type);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.Register();

                recipe = CreateRecipe();
                recipe.AddIngredient(null, "PaladinStaff");
                recipe.AddIngredient(Bluemagic.Sushi.Find<ModItem>("SwapToken").Type);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.Register();
            }
        }
    }
}