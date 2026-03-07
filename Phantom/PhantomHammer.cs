using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Phantom
{
    public class PhantomHammer : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.alpha = 70;
            Projectile.timeLeft = 600;
            Projectile.maxPenetrate = -1;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.rotation += 0.5f;
            Projectile.ai[1] += 1f;
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            if (Projectile.ai[1] <= 100f)
            {
                Projectile.Center = npc.Center;
            }
            else if (Projectile.ai[1] == 101f)
            {
                Vector2 move = Main.player[npc.target].Center - Projectile.Center;
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                if (Main.expertMode)
                {
                    Vector2 prediction = Main.player[npc.target].velocity;
                    prediction *= magnitude / 7f;
                    prediction *= Main.rand.NextFloat();
                    move += prediction;
                }
                if (magnitude > 0f)
                {
                    move *= 7f / magnitude;
                }
                Projectile.velocity = move;
                SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * 0.85f;
        }
    }
}