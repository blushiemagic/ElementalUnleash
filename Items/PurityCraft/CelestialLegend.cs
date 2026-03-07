using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
    public class CelestialLegend : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Turns the holder into a werewolf at night and a merfolk when entering water"
                + "\nIncreases to all stats");*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(30, 2));
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
            player.accMerman = true;
            player.wolfAcc = true;
            if (hideVisual)
            {
                player.hideMerman = true;
                player.hideWolf = true;
            }
            player.lifeRegen += 6;
            player.statDefense += 12;
            player.GetAttackSpeed(DamageClass.Melee) += 0.2f;
            player.GetDamage(DamageClass.Melee) += 0.15f;
            player.GetCritChance(DamageClass.Melee) += 10;
            player.GetDamage(DamageClass.Ranged) += 0.15f;
            player.GetCritChance(DamageClass.Ranged) += 10;
            player.GetDamage(DamageClass.Magic) += 0.15f;
            player.GetCritChance(DamageClass.Magic) += 10;
            player.pickSpeed -= 0.25f;
            player.GetDamage(DamageClass.Summon) += 0.15f;
            player.GetKnockback(DamageClass.Summon).Base += 0.75f;
            player.GetDamage(DamageClass.Throwing) += 0.15f;
            player.GetCritChance(DamageClass.Throwing) += 10;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CelestialShell);
            recipe.AddIngredient(null, "InfinityCrystal", 4);
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}