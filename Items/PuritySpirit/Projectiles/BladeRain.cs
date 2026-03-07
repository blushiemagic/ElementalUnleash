using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit.Projectiles
{
    public class BladeRain : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dance of Blades");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.alpha = 30;
        }

        public override void AI()
        {
            if (Projectile.position.Y > Main.player[Projectile.owner].position.Y + 400)
            {
                Projectile.Kill();
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * 0.8f;
        }
    }
}