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
            Tooltip.SetDefault("15% increased melee damage, 10% increased melee critical strike chance"
                + "\n16% increased melee speed, 4% increased movement speed");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.defense = 30;
            item.rare = 11;
            item.value = Item.sellPrice(0, 6, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeDamage += 0.15f;
            player.meleeCrit += 10;
            player.meleeSpeed += 0.16f;
            player.moveSpeed += 0.04f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PuriumBar", 10);
            recipe.AddTile(null, "PuriumAnvil");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}