using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom
{
    public class PhantomSphere : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Summons a phantom sphere around you");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 28;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 28;
            Item.useTime = 28;
            Item.damage = 53;
            Item.knockBack = 2f;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.autoReuse = false;
            Item.useTurn = false;
            Item.rare = ItemRarityID.Yellow;
            Item.shootSpeed = 0f;
            Item.mana = 18;
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item43;
            Item.shoot = Mod.Find<ModProjectile>("PhantomSphere").Type;
        }

        public override void AddRecipes()
        {
            if (Bluemagic.Sushi != null)
            {
                Recipe recipe;

                recipe = CreateRecipe();
                recipe.AddIngredient(null, "PhantomBlade");
                recipe.AddIngredient(Bluemagic.Sushi.Find<ModItem>("SwapToken").Type);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.Register();

                recipe = CreateRecipe();
                recipe.AddIngredient(null, "SpectreGun");
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