using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Walls
{
    public class ShroomstoneWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            DustType = DustID.Bone;
            WallID.Sets.Conversion.Stone[Type] = true;
            AddMapEntry(new Color(64, 62, 80));
        }
    }
}