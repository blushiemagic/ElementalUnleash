using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Walls
{
    public class MushroomBrickWall : ModWall
    {
        public override void SetDefaults()
        {
            Main.wallHouse[Type] = true;
            dustType = 26;
            drop = mod.ItemType("MushroomBrickWall");
            AddMapEntry(new Color(64, 62, 80));
        }
    }
}