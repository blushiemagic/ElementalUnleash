using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc2
{
    public class SuspiciousGel : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 99;
            item.rare = 11;
            item.value = Item.sellPrice(0, 0, 40, 0);
            item.color = new Color(127, 127, 127, 127);
            item.alpha = 127;
        }
    }
}