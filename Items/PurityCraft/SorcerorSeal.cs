using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
    public class SorcerorSeal : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("30% increased magic damage");
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
            player.GetDamage(DamageClass.Magic) += 0.3f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SorcererEmblem);
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