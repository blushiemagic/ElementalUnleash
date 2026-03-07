using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons
{
    public class PuriumRepeater : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("50% chance not to consume ammo");
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.noMelee = true;
            Item.damage = 253;
            Item.knockBack = 3f;
            Item.autoReuse = true;
            Item.useTurn = false;
            Item.rare = ItemRarityID.Purple;
            Item.DamageType = DamageClass.Ranged;
            Item.value = Item.sellPrice(0, 12, 0, 0);
            Item.UseSound = SoundID.Item5;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.useAmmo = AmmoID.Arrow;
            Item.shootSpeed = 12f;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.Next(2) == 0;
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