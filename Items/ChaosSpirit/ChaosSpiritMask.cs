using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.ChaosSpirit
{
    [AutoloadEquip(EquipType.Head)]
    public class ChaosSpiritMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Spirit of Chaos Mask");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }

        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            color = drawPlayer.GetImmuneAlphaPure(Color.White, shadow);
        }
    }
}