using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class PuriumBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("10% increased damage and critical strike chance"
                + "\nIncreases your max number of minions by 1");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.defense = 29;
            item.rare = 11;
            item.value = Item.sellPrice(0, 12, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeDamage += 0.1f;
            player.rangedDamage += 0.1f;
            player.magicDamage += 0.1f;
            player.minionDamage += 0.1f;
            player.thrownDamage += 0.1f;
            player.meleeCrit += 10;
            player.rangedCrit += 10;
            player.magicCrit += 10;
            player.thrownCrit += 10;
            player.maxMinions += 1;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (head.type == mod.ItemType("PuriumHelmet") || head.type == mod.ItemType("PuriumVisor") || head.type == mod.ItemType("PuriumHeadgear") || head.type == mod.ItemType("PuriumMask") || head.type == mod.ItemType("PuriumHat")) && body.type == mod.ItemType("PuriumBreastplate") && legs.type == mod.ItemType("PuriumLeggings");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.GetModPlayer<BluemagicPlayer>().puriumShieldChargeMax += 1200f;
            player.setBonus = "Increases purity shield capacity by 1200";
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PuriumBar", 20);
            recipe.AddTile(null, "PuriumAnvil");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}