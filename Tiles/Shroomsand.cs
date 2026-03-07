using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Tiles
{
    public class Shroomsand : BaseMushroomTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBrick[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSand[Type] = true;
            DustType = DustID.CorruptPlants;
            AddMapEntry(new Color(93, 127, 255));
            TileID.Sets.Conversion.Sand[Type] = true;
            TileID.Sets.ForAdvancedCollision.ForSandshark[Type] = true;
            TileID.Sets.Suffocate[Type] = true;
            TileID.Sets.Falling[Type] = true;
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            return TileUtils.TileFrame_Sand(i, j, Mod.Find<ModProjectile>("ShroomsandBall").Type);
        }
    }
}