using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Projectiles
{
    public class ShroomsandBall : SandBall
    {
        public override void SetDefaults()
        {
            projectile.knockBack = 6f;
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.hostile = true;
            projectile.penetrate = -1;
            tileType = mod.TileType("Shroomsand");
            dustType = 17;
        }
    }
}