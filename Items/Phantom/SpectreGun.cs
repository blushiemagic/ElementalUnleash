using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom
{
    public class SpectreGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Uses wisps as ammo");
        }

        public override void SetDefaults()
        {
            Item.damage = 68;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 42;
            Item.height = 30;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item43;
            Item.autoReuse = true;
            Item.shoot = Mod.Find<ModProjectile>("Wisp").Type;
            Item.shootSpeed = 8f;
            Item.useAmmo = Mod.Find<ModItem>("Wisp").Type;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage *= player.bulletDamage.Base;
        }

        public override Vector2? HoldoutOffset()
        {
            return Vector2.Zero;
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
