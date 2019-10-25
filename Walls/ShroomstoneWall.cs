using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Walls
{
    public class ShroomstoneWall : ModWall
    {
        public override void SetDefaults()
        {
            dustType = 26;
            WallID.Sets.Conversion.Stone[Type] = true;
            AddMapEntry(new Color(64, 62, 80));
        }
    }
}