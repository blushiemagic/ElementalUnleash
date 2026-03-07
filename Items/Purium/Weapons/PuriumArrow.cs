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
            /* Tooltip.SetDefault("Leaves a trail in its path"
                + "\nThe trail does not interfere with your piercing projectiles");*/
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 28;
            Item.maxStack = 999;
            Item.damage = 14;
            Item.knockBack = 4f;
            Item.consumable = true;
            Item.ammo = AmmoID.Arrow;
            Item.rare = ItemRarityID.Purple;
            Item.DamageType = DamageClass.Ranged;
            Item.value = Item.sellPrice(0, 0, 0, 25);
            Item.shoot = Mod.Find<ModProjectile>("PuriumArrow").Type;
            Item.shootSpeed = 6f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(111);
            recipe.AddIngredient(null, "PuriumBar");
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}