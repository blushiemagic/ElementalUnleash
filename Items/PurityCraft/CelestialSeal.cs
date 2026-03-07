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
            /* Tooltip.SetDefault("30% increased magic damage"
                + "\nGreatly increases pickup range and effectiveness of stars");*/
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
            player.manaMagnet = true;
            player.GetDamage(DamageClass.Magic) += 0.3f;
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            modPlayer.manaMagnet2 = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CelestialEmblem);
            recipe.AddIngredient(null, "SorcerorSeal");
            recipe.AddIngredient(null, "InfinityCrystal");
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}