using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Tools
{
    public class PuriumJackhammer : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 130;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.width = 20;
            Item.height = 12;
            Item.useTime = 7;
            Item.useAnimation = 15;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.hammer = 110;
            Item.tileBoost += 4;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 8;
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item23;
            Item.autoReuse = true;
            Item.shoot = Mod.Find<ModProjectile>("PuriumJackhammer").Type;
            Item.shootSpeed = 46f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "PuriumBar", 10);
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}