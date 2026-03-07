using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
    public class CrystalDestroyerScope : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Increases view range for guns (<right> to zoom out)"
                + "\n25% increased ranged damage and critical strike chance");*/
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 30, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.scope = true;
            player.GetDamage(DamageClass.Ranged) += 0.25f;
            player.GetCritChance(DamageClass.Ranged) += 25;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SniperScope);
            recipe.AddIngredient(null, "RangerSeal");
            recipe.AddIngredient(null, "InfinityCrystal");
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}