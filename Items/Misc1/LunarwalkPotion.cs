using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
    public class LunarwalkPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("30% increased movement speed and allows the control of gravity"
                + "\nIncompatible with Swiftness");*/
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
            Item.buffType = Mod.Find<ModBuff>("Lunarwalk").Type;
            Item.buffTime = 21600;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SwiftnessPotion);
            recipe.AddIngredient(ItemID.GravitationPotion);
            recipe.AddIngredient(null, "LunarDrop", 5);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();
        }
    }
}