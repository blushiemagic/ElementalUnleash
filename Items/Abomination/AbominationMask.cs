using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination
{
    [AutoloadEquip(EquipType.Head)]
    public class AbominationMask : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 20;
            item.rare = 1;
            item.vanity = true;
        }

        public override bool DrawHead()
        {
            return false;
        }
    }
}