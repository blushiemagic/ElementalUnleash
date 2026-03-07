using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom.Projectiles
{
    public class MiniHammer : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 19;
            Projectile.height = 19;
            Projectile.timeLeft = 200;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Projectile.velocity.X < 0f)
            {
                Projectile.rotation -= 0.5f;
            }
            else
            {
                Projectile.rotation += 0.5f;
            }
        }
    }
}