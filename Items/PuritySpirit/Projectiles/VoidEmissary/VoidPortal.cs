using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit.Projectiles.VoidEmissary
{
    public class VoidPortal : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 10)
            {
                Projectile.frame = 1 - Projectile.frame;
                Projectile.frameCounter = 0;
            }
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 60f)
            {
                Projectile.Kill();
            }
            if (Projectile.ai[1] == 1f)
            {
                Projectile.ai[1] = 2f;
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[1] == 2f)
            {
                return false;
            }
            return base.CanHitNPC(target);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.penetrate++;
            if (Projectile.ai[1] == 0f)
            {
                Projectile.ai[1] = 1f;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}