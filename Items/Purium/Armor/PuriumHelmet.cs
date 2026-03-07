using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class PuriumHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("15% increased melee damage, 10% increased melee critical strike chance"
                + "\n16% increased melee speed, 4% increased movement speed");*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.defense = 30;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 6, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.15f;
            player.GetCritChance(DamageClass.Melee) += 10;
            player.GetAttackSpeed(DamageClass.Melee) += 0.16f;
            player.moveSpeed += 0.04f;
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