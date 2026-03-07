using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.NPCs.Night
{
    public class StarGel : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                SoundEngine.PlaySound(SoundID.Item28, Projectile.position);
                Projectile.localAI[0] = 1f;
            }
            if (Main.rand.Next(3) == 0)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, Mod.Find<ModDust>("Sparkle").Type, 0f, 0f, 0, Color.White, 1.5f);
                Main.dust[dust].velocity *= 0.5f;
                Main.dust[dust].velocity += Projectile.velocity * 0.5f;
            }
            Projectile.rotation += 0.02f;
            if (Projectile.rotation > MathHelper.TwoPi)
            {
                Projectile.rotation -= MathHelper.TwoPi;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.rand.Next(10) == 0)
            {
                target.AddBuff(Mod.Find<ModBuff>("Liquified").Type, 240, true);
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 4; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, Mod.Find<ModDust>("Sparkle").Type, 0f, 0f, 0, Color.White, 1.5f);
            }
            SoundEngine.PlaySound(SoundID.Item28, Projectile.position);
        }
    }
}
