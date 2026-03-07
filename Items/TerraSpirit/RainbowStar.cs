using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.TerraSpirit
{
    public class RainbowStar : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Activates [c/FF0000:G][c/FF7700:O][c/FFFF00:D][c/00FF00:M][c/0000FF:O][c/7700FF:D][c/FF00FF:E]");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 26;
            Item.value = Item.buyPrice(10, 0, 0, 0);
            Item.rare = ItemRarityID.Expert;
            Item.accessory = true;
            Item.defense = 1337;
            Item.lifeRegen = 400;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            if (modPlayer.noGodmode)
            {
                player.statDefense -= Item.defense;
                modPlayer.triedGodmode = true;
            }
            else
            {
                modPlayer.godmode = true;
                if (player.endurance < 1f)
                {
                    player.endurance = 1f;
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}