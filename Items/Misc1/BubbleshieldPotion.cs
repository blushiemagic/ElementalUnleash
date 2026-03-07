using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
    public class BubbleshieldPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Increases defense by 12 and reduces damage taken by 12%"
                + "\nIncompatible with Ironskin and Endurance");*/
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
            Item.buffType = Mod.Find<ModBuff>("Bubbleshield").Type;
            Item.buffTime = 21600;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IronskinPotion);
            recipe.AddIngredient(ItemID.EndurancePotion);
            recipe.AddIngredient(null, "Bubble");
            recipe.AddTile(TileID.Bottles);
            recipe.Register();
        }
    }
}