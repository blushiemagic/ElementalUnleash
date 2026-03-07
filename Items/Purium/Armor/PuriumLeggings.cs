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
            // Tooltip.SetDefault("12% increased movement speed");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.defense = 21;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 9, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.12f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "PuriumBar", 15);
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}