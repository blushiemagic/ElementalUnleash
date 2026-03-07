using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination
{
    public class ElementalSprayer : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Uses gel for ammo, 75% chance to consume gel"
                + "\nYo I heard you like debuffs, so I...");*/
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.useAnimation = 30;
            Item.useTime = 5;
            Item.width = 54;
            Item.height = 14;
            Item.shoot = Mod.Find<ModProjectile>("ElementalSpray").Type;
            Item.useAmmo = AmmoID.Gel;
            Item.UseSound = SoundID.Item34;
            Item.damage = 240;
            Item.knockBack = 0.5f;
            Item.shootSpeed = 10f;
            Item.noMelee = true;
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.DamageType = DamageClass.Ranged;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.Next(4) != 0;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer, Main.rand.Next(6));
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
                recipe.AddIngredient(null, "EyeballTome");
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
