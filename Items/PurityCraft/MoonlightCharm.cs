using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
    [AutoloadEquip(EquipType.HandsOn)]
    public class MoonlightCharm : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Provides huge life regeneration and greatly reduces the cooldown of healing potions"
                + "\nIncreases pickup range and effectiveness of hearts");*/
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 24;
            Item.accessory = true;
            Item.lifeRegen = 10;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 25, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            modPlayer.lifeMagnet2 = true;
            player.pStone = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CharmofMyths);
            recipe.AddIngredient(null, "InfinityCrystal", 2);
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}