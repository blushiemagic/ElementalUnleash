using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Salt
{
    public class PureSalt : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The purest form of your rage"
                + "\nIncreases your damage by 0.001% per Pure Salt in your inventory");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 14;
            item.maxStack = 999;
            item.rare = 12;
            item.value = 500;
        }

        public override void UpdateInventory(Player player)
        {
            float increase = 0.00001f * item.stack;
            player.meleeDamage += increase;
            player.rangedDamage += increase;
            player.magicDamage += increase;
            player.minionDamage += increase;
            player.thrownDamage += increase;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(this);
            recipe.needWater = true;
            recipe.SetResult(mod, "Salt", 5);
            recipe.AddRecipe();
        }
    }
}