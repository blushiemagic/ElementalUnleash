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
            DisplayName.SetDefault("Shroomsand Ball");
            ProjectileID.Sets.ForcePlateDetection[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.knockBack = 6f;
            projectile.width = 10;
            projectile.height = 10;
            //projectile.aiStyle = 10;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.extraUpdates = 1;
            falling = false;
        }
    }
}