using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Tools
{
    public class PuriumPickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Can mine Frostbyte");
        }

        public override void SetDefaults()
        {
            Item.damage = 90;
            Item.DamageType = DamageClass.Melee;
            Item.width = 20;
            Item.height = 12;
            Item.scale = 1.15f;
            Item.useTime = 6;
            Item.useAnimation = 11;
            Item.pick = 250;
            Item.tileBoost += 4;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(0, 12, 0, 0);
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
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