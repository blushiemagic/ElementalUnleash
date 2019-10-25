using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class PuriumLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("12% increased movement speed");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.defense = 21;
            item.rare = 11;
            item.value = Item.sellPrice(0, 9, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.12f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PuriumBar", 15);
            recipe.AddTile(null, "PuriumAnvil");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}