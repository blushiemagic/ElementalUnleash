using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.NPCs.Night
{
    public class StarGel : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.alpha = 255;
            projectile.penetrate = -1;
            projectile.hostile = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
        }

        public override void AI()
        {
            if (projectile.localAI[0] == 0f)
            {
                Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 28);
                projectile.localAI[0] = 1f;
            }
            if (Main.rand.Next(3) == 0)
            {
                int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, mod.DustType("Sparkle"), 0f, 0f, 0, Color.White, 1.5f);
                Main.dust[dust].velocity *= 0.5f;
                Main.dust[dust].velocity += projectile.velocity * 0.5f;
            }
            projectile.rotation += 0.02f;
            if (projectile.rotation > MathHelper.TwoPi)
            {
                projectile.rotation -= MathHelper.TwoPi;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.Next(10) == 0)
            {
                target.AddBuff(mod.BuffType("Liquified"), 240, true);
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 4; k++)
            {
                int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, mod.DustType("Sparkle"), 0f, 0f, 0, Color.White, 1.5f);
            }
            Main.PlaySound(2, projectile.position, 28);
        }
    }
}
