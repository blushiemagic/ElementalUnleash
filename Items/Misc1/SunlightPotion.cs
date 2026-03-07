using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
    public class SunlightPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Emits a strong aura of light and increases night vision"
                + "\nIncompatible with Shine");*/
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 24;
            Item.maxStack = 30;
            Item.rare = ItemRarityID.Orange;
            Item.value = 1000;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;
            Item.buffType = Mod.Find<ModBuff>("Sunlight").Type;
            Item.buffTime = 21600;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ShinePotion);
            recipe.AddIngredient(ItemID.NightOwlPotion);
            recipe.AddIngredient(null, "SolarDrop", 5);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();
        }
    }
}