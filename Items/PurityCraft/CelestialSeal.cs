using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
    public class CelestialSeal : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("30% increased magic damage"
                + "\nGreatly increases pickup range and effectiveness of stars");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.accessory = true;
            item.rare = 11;
            item.value = Item.sellPrice(0, 30, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.manaMagnet = true;
            player.magicDamage += 0.3f;
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            modPlayer.manaMagnet2 = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CelestialEmblem);
            recipe.AddIngredient(null, "SorcerorSeal");
            recipe.AddIngredient(null, "InfinityCrystal");
            recipe.AddTile(null, "PuriumAnvil");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}