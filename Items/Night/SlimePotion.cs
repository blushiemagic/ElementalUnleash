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
            Tooltip.SetDefault("Transforms you into a Slime");
        }

        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 24;
            item.maxStack = 30;
            item.rare = 3;
            item.value = 1000;
            item.useStyle = 2;
            item.useAnimation = 17;
            item.useTime = 17;
            item.useTurn = true;
            item.UseSound = SoundID.Item3;
            item.consumable = true;
            item.buffType = mod.BuffType("SlimePotion");
            item.buffTime = 7200;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(null, "SuspiciousGel");
            recipe.AddIngredient(ItemID.Blinkroot);
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}