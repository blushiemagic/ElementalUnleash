using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons
{
    public class PuriumLightbeam : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.scale = 1.1f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.damage = 79;
            Item.knockBack = 5f;
            Item.autoReuse = false;
            Item.useTurn = false;
            Item.rare = ItemRarityID.Purple;
            Item.DamageType = DamageClass.Melee;
            Item.value = Item.sellPrice(0, 12, 0, 0);
            Item.UseSound = SoundID.Item9;
            Item.shoot = Mod.Find<ModProjectile>("PuriumSpear").Type;
            Item.shootSpeed = 5f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "PuriumBar", 12);
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}