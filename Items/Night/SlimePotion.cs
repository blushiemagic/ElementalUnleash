using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Night
{
    public class SlimePotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Transforms you into a Slime");
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
            Item.buffType = Mod.Find<ModBuff>("SlimePotion").Type;
            Item.buffTime = 7200;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(null, "SuspiciousGel");
            recipe.AddIngredient(ItemID.Blinkroot);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();
        }
    }
}