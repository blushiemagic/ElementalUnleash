using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
    public class EndlessSilverPouch : ModItem
    {
        public override void SetDefaults()
        {
            Item.shootSpeed = 4f;
            Item.shoot = ProjectileID.Bullet;
            Item.damage = 9;
            Item.width = 26;
            Item.height = 26;
            Item.DamageType = DamageClass.Ranged;
            Item.ammo = AmmoID.Bullet;
            Item.knockBack = 3f;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 20, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.EndlessMusketPouch);
            recipe.AddIngredient(null, "InfinityCrystal");
            recipe.AddIngredient(ItemID.SilverBullet, 3996);
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}