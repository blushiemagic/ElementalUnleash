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
            Tooltip.SetDefault("Causes crystals to fall and greatly increases length of invincibility after taking damage");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 24;
            item.accessory = true;
            item.rare = 11;
            item.value = Item.sellPrice(0, 25, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            modPlayer.crystalCloak = true;
            player.longInvince = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.StarVeil);
            recipe.AddIngredient(null, "InfinityCrystal", 2);
            recipe.AddTile(null, "PuriumAnvil");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}