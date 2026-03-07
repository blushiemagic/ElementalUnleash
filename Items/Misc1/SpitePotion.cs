using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
    public class SpitePotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("12% increased damage and critical strike chance"
                + "\nIncompatible with Wrath and Rage");*/
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 24;
            Item.maxStack = 30;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 1000;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;
            Item.buffType = Mod.Find<ModBuff>("Spite").Type;
            Item.buffTime = 21600;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.WrathPotion, 2);
            recipe.AddIngredient(null, "ScytheBlade");
            recipe.AddTile(TileID.Bottles);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.RagePotion, 2);
            recipe.AddIngredient(null, "ScytheBlade");
            recipe.AddTile(TileID.Bottles);
            recipe.Register();
        }
    }
}