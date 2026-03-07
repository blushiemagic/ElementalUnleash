using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
    public class InfinityScarab : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Greatly increases your max number of minions"
                + "\nGreatly increases the damage and knockback of your minions");*/
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 30, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Summon) += 0.3f;
            player.GetKnockback(DamageClass.Summon).Base += 2.5f;
            player.maxMinions += 3;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PapyrusScarab);
            recipe.AddIngredient(null, "SummonerSeal");
            recipe.AddIngredient(null, "InfinityCrystal");
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}