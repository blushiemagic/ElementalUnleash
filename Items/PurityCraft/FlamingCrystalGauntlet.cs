using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
    [AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
    public class FlamingCrystalGauntlet : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Increases melee knockback and inflicts fire damage on attack"
                + "\n25% increased melee damage and speed");*/
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 30, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.kbGlove = true;
            player.GetAttackSpeed(DamageClass.Melee) += 0.25f;
            player.GetDamage(DamageClass.Melee) += 0.25f;
            player.magmaStone = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FireGauntlet);
            recipe.AddIngredient(null, "WarriorSeal");
            recipe.AddIngredient(null, "InfinityCrystal");
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}