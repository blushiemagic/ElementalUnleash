using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination
{
    public class ElementalSprayer : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Uses gel for ammo, 75% chance to consume gel"
                + "\nYo I heard you like debuffs, so I...");
        }

        public override void SetDefaults()
        {
            item.useStyle = 5;
            item.autoReuse = true;
            item.useAnimation = 30;
            item.useTime = 5;
            item.width = 54;
            item.height = 14;
            item.shoot = mod.ProjectileType("ElementalSpray");
            item.useAmmo = AmmoID.Gel;
            item.UseSound = SoundID.Item34;
            item.damage = 240;
            item.knockBack = 0.5f;
            item.shootSpeed = 10f;
            item.noMelee = true;
            item.value = Item.sellPrice(0, 15, 0, 0);
            item.rare = 10;
            item.ranged = true;
        }

        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.Next(4) != 0;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, Main.myPlayer, Main.rand.Next(6));
            return false;
        }

        public override void AddRecipes()
        {
            if (Bluemagic.Sushi != null)
            {
                ModRecipe recipe;

                recipe = new ModRecipe(mod);
                recipe.AddIngredient(null, "ElementalYoyo");
                recipe.AddIngredient(Bluemagic.Sushi.ItemType("SwapToken"));
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.SetResult(this);
                recipe.AddRecipe();

                recipe = new ModRecipe(mod);
                recipe.AddIngredient(null, "EyeballTome");
                recipe.AddIngredient(Bluemagic.Sushi.ItemType("SwapToken"));
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.SetResult(this);
                recipe.AddRecipe();

                recipe = new ModRecipe(mod);
                recipe.AddIngredient(null, "ElementalStaff");
                recipe.AddIngredient(Bluemagic.Sushi.ItemType("SwapToken"));
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.SetResult(this);
                recipe.AddRecipe();

                recipe = new ModRecipe(mod);
                recipe.AddIngredient(null, "EyeballGlove");
                recipe.AddIngredient(Bluemagic.Sushi.ItemType("SwapToken"));
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }
}
