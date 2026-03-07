using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom.Projectiles
{
    public class PhantomHammerFriendly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Phantom Hammer");
        }

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.alpha = 70;
            Projectile.timeLeft = 300;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.rotation += 0.5f;
            if (Projectile.velocity.Y < 0f)
            {
                Projectile.velocity.Y += 0.15f;
            }
            else
            {
                Projectile.velocity.Y += 0.5f;
            }
            if (Projectile.velocity.Y > 32f)
            {
                Projectile.velocity.Y = 32f;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * 0.85f;
        }
    }
}