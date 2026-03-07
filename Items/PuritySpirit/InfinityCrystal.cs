using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit
{
    public class InfinityCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(1, 0, 0, 0);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}