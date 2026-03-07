using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination
{
    public class EyeballTome : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Move while using for the best results."
                + "\nYo I heard you like debuffs, so I...");*/
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Red;
            Item.mana = 6;
            Item.UseSound = null;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.damage = 164;
            Item.useAnimation = 10;
            Item.useTime = 5;
            Item.width = 24;
            Item.height = 28;
            Item.shoot = Mod.Find<ModProjectile>("EyeballTome").Type;
            Item.shootSpeed = 0f;
            Item.knockBack = 4f;
            Item.DamageType = DamageClass.Magic;
            Item.value = Item.sellPrice(0, 15, 0, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, Main.myPlayer, Main.rand.Next(6));
            return false;
        }

        public override void AddRecipes()
        {
            if (Bluemagic.Sushi != null)
            {
                Recipe recipe;

                recipe = CreateRecipe();
                recipe.AddIngredient(null, "ElementalYoyo");
                recipe.AddIngredient(Bluemagic.Sushi.Find<ModItem>("SwapToken").Type);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.Register();

                recipe = CreateRecipe();
                recipe.AddIngredient(null, "ElementalSprayer");
                recipe.AddIngredient(Bluemagic.Sushi.Find<ModItem>("SwapToken").Type);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.Register();

                recipe = CreateRecipe();
                recipe.AddIngredient(null, "ElementalStaff");
                recipe.AddIngredient(Bluemagic.Sushi.Find<ModItem>("SwapToken").Type);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.Register();

                recipe = CreateRecipe();
                recipe.AddIngredient(null, "EyeballGlove");
                recipe.AddIngredient(Bluemagic.Sushi.Find<ModItem>("SwapToken").Type);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.Register();
            }
        }
    }
}
