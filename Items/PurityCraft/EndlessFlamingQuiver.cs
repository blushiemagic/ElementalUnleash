using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
    public class EndlessFlamingQuiver : ModItem
    {
        public override void SetDefaults()
        {
            Item.shootSpeed = 3.5f;
            Item.shoot = ProjectileID.FireArrow;
            Item.damage = 7;
            Item.width = 26;
            Item.height = 26;
            Item.DamageType = DamageClass.Ranged;
            Item.ammo = AmmoID.Arrow;
            Item.knockBack = 2f;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 20, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.EndlessQuiver);
            recipe.AddIngredient(null, "InfinityCrystal");
            recipe.AddIngredient(ItemID.FlamingArrow, 3996);
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}