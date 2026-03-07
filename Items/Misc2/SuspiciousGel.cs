using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc2
{
    public class SuspiciousGel : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 0, 40, 0);
            Item.color = new Color(127, 127, 127, 127);
            Item.alpha = 127;
        }
    }
}