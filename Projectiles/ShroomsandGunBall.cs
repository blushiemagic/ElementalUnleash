using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Projectiles
{
    public class ShroomsandGunBall : ShroomsandBall
    {
        public override string Texture
        {
            get
            {
                return "Bluemagic/Projectiles/ShroomsandBall";
            }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Shroomsand Ball");
            ProjectileID.Sets.ForcePlateDetection[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.knockBack = 6f;
            Projectile.width = 10;
            Projectile.height = 10;
            //projectile.aiStyle = 10;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            falling = false;
        }
    }
}