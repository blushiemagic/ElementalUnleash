using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons
{
    public class PuriumStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Casts a controllable sphere of purity");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 28;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 27;
            Item.useTime = 27;
            Item.channel = true;
            Item.noMelee = true;
            Item.damage = 757;
            Item.knockBack = 6.5f;
            Item.autoReuse = false;
            Item.useTurn = false;
            Item.rare = ItemRarityID.Purple;
            Item.DamageType = DamageClass.Magic;
            Item.value = Item.sellPrice(0, 12, 0, 0);
            Item.UseSound = SoundID.Item28;
            Item.shoot = Mod.Find<ModProjectile>("PuriumStaff").Type;
            Item.mana = 18;
            Item.shootSpeed = 6f;
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