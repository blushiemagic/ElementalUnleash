using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Tools
{
    public class PuriumDrill : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Can mine Frostbyte");
        }

        public override void SetDefaults()
        {
            Item.damage = 90;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.width = 20;
            Item.height = 12;
            Item.useTime = 6;
            Item.useAnimation = 11;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.pick = 250;
            Item.tileBoost += 4;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(0, 12, 0, 0);
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item23;
            Item.autoReuse = true;
            Item.shoot = Mod.Find<ModProjectile>("PuriumDrill").Type;
            Item.shootSpeed = 40f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "PuriumBar", 15);
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}