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
            /* Tooltip.SetDefault("10% increased damage and critical strike chance"
                + "\nIncreases your max number of minions by 1");*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.defense = 29;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 12, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.1f;
            player.GetDamage(DamageClass.Ranged) += 0.1f;
            player.GetDamage(DamageClass.Magic) += 0.1f;
            player.GetDamage(DamageClass.Summon) += 0.1f;
            player.GetDamage(DamageClass.Throwing) += 0.1f;
            player.GetCritChance(DamageClass.Melee) += 10;
            player.GetCritChance(DamageClass.Ranged) += 10;
            player.GetCritChance(DamageClass.Magic) += 10;
            player.GetCritChance(DamageClass.Throwing) += 10;
            player.maxMinions += 1;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (head.type == Mod.Find<ModItem>("PuriumHelmet").Type || head.type == Mod.Find<ModItem>("PuriumVisor").Type || head.type == Mod.Find<ModItem>("PuriumHeadgear").Type || head.type == Mod.Find<ModItem>("PuriumMask").Type || head.type == Mod.Find<ModItem>("PuriumHat").Type) && body.type == Mod.Find<ModItem>("PuriumBreastplate").Type && legs.type == Mod.Find<ModItem>("PuriumLeggings").Type;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.GetModPlayer<BluemagicPlayer>().puriumShieldChargeMax += 1200f;
            player.setBonus = "Increases purity shield capacity by 1200";
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "PuriumBar", 20);
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}