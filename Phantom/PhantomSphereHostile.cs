using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Phantom
{
    public class PhantomSphereHostile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Phantom Sphere");
        }

        public override void SetDefaults()
        {
            Projectile.width = 192;
            Projectile.height = 192;
            Projectile.alpha = 70;
            Projectile.hostile = true;
            Projectile.timeLeft = 1200;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.maxPenetrate = -1;
        }

        public override void AI()
        {
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] == 120f)
            {
                Player player = Main.player[Main.npc[(int)Projectile.ai[0]].target];
                Vector2 offset = player.Center - Projectile.Center;
                if (Main.expertMode)
                {
                    Vector2 prediction = player.velocity;
                    prediction *= offset.Length() / 6f;
                    prediction *= Main.rand.NextFloat();
                    offset += prediction;
                }
                if (offset != Vector2.Zero)
                {
                    offset.Normalize();
                    offset *= 6f;
                }
                Projectile.velocity = offset;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Ellipse.Collides(new Vector2(projHitbox.X, projHitbox.Y), new Vector2(projHitbox.Width, projHitbox.Height), new Vector2(targetHitbox.X, targetHitbox.Y), new Vector2(targetHitbox.Width, targetHitbox.Height));
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * 0.85f;
        }
    }
}