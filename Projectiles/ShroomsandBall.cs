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
            Projectile.knockBack = 6f;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            tileType = Mod.Find<ModTile>("Shroomsand").Type;
            dustType = 17;
        }
    }
}