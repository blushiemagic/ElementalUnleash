using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class PuriumMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("50% increased minion damage"
                + "\nIncreases maximum number of minions by 4");*/
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.defense = 5;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 6, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) += 0.5f;
            player.maxMinions += 4;
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