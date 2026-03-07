using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom.Projectiles
{
    public class PhantomSphere : ModProjectile
    {
        private const int maxTime = 600;
        private const int fadeOutTime = 100;
        private const int fadeInTime = 50;

        public override void SetDefaults()
        {
            Projectile.width = 192;
            Projectile.height = 192;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            for (int k = 0; k < 1000; k++)
            {
                if (k != Projectile.whoAmI && Main.projectile[k].type == Projectile.type && Main.projectile[k].owner == Projectile.owner && Main.projectile[k].ai[0] < Projectile.ai[0])
                {
                    if (Projectile.ai[0] < maxTime - fadeOutTime)
                    {
                        Projectile.ai[0] = maxTime - fadeOutTime;
                    }
                    break;
                }
            }
            Projectile.Center = Main.player[Projectile.owner].Center;
            if (Projectile.ai[0] < fadeInTime)
            {
                Projectile.alpha = (int)((fadeInTime - Projectile.ai[0]) * 185f / 50f) + 70;
            }
            if (Projectile.ai[0] >= maxTime - fadeOutTime)
            {
                Projectile.alpha = 255 - (int)((maxTime - Projectile.ai[0]) * 185f / 100f);
            }
            if (Projectile.ai[0] >= maxTime)
            {
                Projectile.Kill();
            }
            Projectile.ai[0] += 1f;
            for (int x = 0; x < 3; x++)
            {
                if (Main.rand.Next(30) == 0)
                {
                    float angle = (float)Main.rand.Next(36) * 2f * (float)Math.PI / 36f;
                    float xOff = (float)Projectile.width * 0.5f * (float)Math.Cos(angle);
                    float yOff = (float)Projectile.width * 0.5f * (float)Math.Sin(angle);
                    Dust.NewDust(Projectile.Center + new Vector2(xOff, yOff), 0, 0, Mod.Find<ModDust>("SpectreDust").Type);
                }
            }
            Lighting.AddLight(Projectile.Center, 0.05f, 0.15f, 0.2f);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Ellipse.Collides(new Vector2(projHitbox.X, projHitbox.Y), new Vector2(projHitbox.Width, projHitbox.Height), new Vector2(targetHitbox.X, targetHitbox.Y), new Vector2(targetHitbox.Width, targetHitbox.Height));
        }

        public override Color? GetAlpha(Color color)
        {
            return Color.White * ((255 - Projectile.alpha) / 255f);
        }
    }
}