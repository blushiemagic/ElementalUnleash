using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class PuriumHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("15% increased throwing damage, 10% increased throwing critical strike chance"
                + "\n20% increased throwing velocity, 33% chance to not consume thrown item");*/
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.defense = 20;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 6, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Throwing) += 0.15f;
            player.GetCritChance(DamageClass.Throwing) += 10;
            player.ThrownVelocity += 0.2f;
            player.ThrownCost33 = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "PuriumBar", 10);
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}