using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
    [AutoloadEquip(EquipType.Neck)]
    public class CrystalVeil : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Causes crystals to fall and greatly increases length of invincibility after taking damage");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 25, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            modPlayer.crystalCloak = true;
            player.longInvince = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.StarVeil);
            recipe.AddIngredient(null, "InfinityCrystal", 2);
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}