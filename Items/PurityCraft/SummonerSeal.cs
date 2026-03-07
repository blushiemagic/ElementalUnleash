using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
    public class SummonerSeal : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("30% increased summon damage");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 25, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Summon) += 0.3f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SummonerEmblem);
            recipe.AddIngredient(null, "InfinityCrystal");
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.AvengerEmblem);
            recipe.AddIngredient(null, "InfinityCrystal");
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}