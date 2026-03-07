using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Tiles
{
    public class Shroomstone : BaseMushroomTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBrick[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            HitSound = SoundID.Tink;
            DustType = DustID.CorruptPlants;
            MinPick = 65;
            AddMapEntry(new Color(93, 127, 255));
            TileID.Sets.Conversion.Stone[Type] = true;
            TileID.Sets.Stone[Type] = true;
        }
    }
}