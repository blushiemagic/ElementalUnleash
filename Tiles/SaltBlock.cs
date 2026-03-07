using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Tiles
{
    public class SaltBlock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBrick[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileSand[Type] = true;
            DustType = DustID.Glass;
            AddMapEntry(new Color(200, 200, 255));
            TileID.Sets.Suffocate[Type] = true;
            TileID.Sets.Falling[Type] = true;
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            return TileUtils.TileFrame_Sand(i, j, Mod.Find<ModProjectile>("SaltBlockBall").Type);
        }
    }
}