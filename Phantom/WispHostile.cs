using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Phantom
{
    public class WispHostile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wisp");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.alpha = 70;
            Projectile.penetrate = 1;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            if (Projectile.ai[1] > 0f)
            {
                Projectile.Center = Main.npc[(int)Projectile.ai[0]].Center;
                Projectile.ai[1] -= 1f;
                return;
            }
            Vector2 move = new Vector2(0f, 0f);
            float distance = 400f;
            Player target = null;
            for (int k = 0; k < 255; k++)
            {
                if (Main.player[k].active && !Main.player[k].dead)
                {
                    Vector2 newMove = Main.player[k].Center - Projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        distance = distanceTo;
                        target = Main.player[k];
                    }
                }
            }
            if (target != null)
            {
                AdjustMagnitude(ref move);
                Projectile.velocity = (5 * Projectile.velocity + move) / 6f;
                AdjustMagnitude(ref Projectile.velocity);
                if (Projectile.Hitbox.Intersects(target.Hitbox))
                {
                    target.immune = false;
                    target.immuneTime = 0;
                    Projectile.Damage();
                    if (!target.immune && target.immuneTime <= 0)
                    {
                        target.immune = true;
                        target.immuneTime = 60;
                    }
                }
            }
            for (int k2 = 0; k2 < 3; k2++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, Mod.Find<ModDust>("Phantom").Type);
                Main.dust[dust].velocity /= 2f;
            }
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 7f)
            {
                vector *= 7f / magnitude;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.rand.Next(4) == 0)
            {
                target.AddBuff(Mod.Find<ModBuff>("EtherealFlames").Type, 300, true);
            }
            Projectile.Kill();
        }
    }
}