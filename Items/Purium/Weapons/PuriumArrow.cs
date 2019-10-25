using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons
{
    public class PuriumArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Leaves a trail in its path"
                + "\nThe trail does not interfere with your piercing projectiles");
        }

        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 28;
            item.maxStack = 999;
            item.damage = 14;
            item.knockBack = 4f;
            item.consumable = true;
            item.ammo = AmmoID.Arrow;
            item.rare = 11;
            item.ranged = true;
            item.value = Item.sellPrice(0, 0, 0, 25);
            item.shoot = mod.ProjectileType("PuriumArrow");
            item.shootSpeed = 6f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PuriumBar");
            recipe.AddTile(null, "PuriumAnvil");
            recipe.SetResult(this, 111);
            recipe.AddRecipe();
        }
    }
}