using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Tiles
{
    public class DarkBlueIce : BaseMushroomTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBrick[Type] = true;
            Main.tileMerge[Type] = TileID.Sets.Snow;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            soundType = 2;
            soundStyle = 50;
            dustType = 17;
            drop = mod.ItemType("DarkBlueIce");
            AddMapEntry(new Color(93, 127, 255));
            TileID.Sets.Conversion.Ice[Type] = true;
            TileID.Sets.Ices[Type] = true;
            TileID.Sets.IcesSlush[Type] = true;
            TileID.Sets.IcesSnow[Type] = true;
            TileID.Sets.ChecksForMerge[Type] = true;
        }

        public override void FloorVisuals(Player player)
        {
            player.slippy = true;
        }
    }
}