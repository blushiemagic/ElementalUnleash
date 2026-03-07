using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
    public class AvengerSeal : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("30% increased damage");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 30, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.3f;
            player.GetDamage(DamageClass.Ranged) += 0.3f;
            player.GetDamage(DamageClass.Magic) += 0.3f;
            player.GetDamage(DamageClass.Summon) += 0.3f;
            player.GetDamage(DamageClass.Throwing) += 0.3f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "WarriorSeal");
            recipe.AddIngredient(null, "RangerSeal");
            recipe.AddIngredient(null, "SorcerorSeal");
            recipe.AddIngredient(null, "SummonerSeal");
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}