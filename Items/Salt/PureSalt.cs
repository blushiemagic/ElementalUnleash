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
            /* Tooltip.SetDefault("The purest form of your rage"
                + "\nIncreases your damage by 0.001% per Pure Salt in your inventory");*/
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 14;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Expert;
            Item.value = 500;
        }

        public override void UpdateInventory(Player player)
        {
            float increase = 0.00001f * Item.stack;
            player.GetDamage(DamageClass.Melee) += increase;
            player.GetDamage(DamageClass.Ranged) += increase;
            player.GetDamage(DamageClass.Magic) += increase;
            player.GetDamage(DamageClass.Summon) += increase;
            player.GetDamage(DamageClass.Throwing) += increase;
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(ModContent.ItemType<Salt>(), 5);
            recipe.AddIngredient(this);
            recipe.AddCondition(Condition.NearWater);
            recipe.Register();
        }
    }
}