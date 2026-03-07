using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
    public class DestroyerSeal : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("25% increased damage"
                + "\n20% increased critical strike chance");*/
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 30, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.25f;
            player.GetDamage(DamageClass.Ranged) += 0.25f;
            player.GetDamage(DamageClass.Magic) += 0.25f;
            player.GetDamage(DamageClass.Summon) += 0.25f;
            player.GetDamage(DamageClass.Throwing) += 0.25f;
            player.GetCritChance(DamageClass.Melee) += 20;
            player.GetCritChance(DamageClass.Ranged) += 20;
            player.GetCritChance(DamageClass.Magic) += 20;
            player.GetCritChance(DamageClass.Throwing) += 20;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "AvengerSeal");
            recipe.AddIngredient(ItemID.DestroyerEmblem);
            recipe.AddIngredient(null, "InfinityCrystal");
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}