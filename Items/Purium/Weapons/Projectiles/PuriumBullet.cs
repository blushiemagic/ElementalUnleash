using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons.Projectiles
{
    public class PuriumBullet : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 20;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.MaxUpdates = 16;
        }

        public override void AI()
        {
            if (Projectile.soundDelay > 0)
            {
                Projectile.soundDelay--;
            }
            if (Projectile.localAI[0] == 0f)
            {
                SoundEngine.PlaySound(SoundID.Item12, Projectile.position);
                Projectile.localAI[0] = 1f;
                Projectile.soundDelay = 96;
                float speed = (float)Math.Sqrt(Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y);
                Projectile.velocity *= 2f / speed;
            }
            Dust.NewDust(Projectile.position, 0, 0, Mod.Find<ModDust>("PuriumBullet").Type);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate > 0)
            {
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
                if (Projectile.soundDelay <= 0f)
                {
                    SoundEngine.PlaySound(SoundID.Item12, Projectile.position);
                    Projectile.soundDelay = 96;
                }
            }
            else
            {
                return true;
            }
            return false;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.rand.Next(2) == 0)
            {
                target.AddBuff(BuffID.BrokenArmor, 600, true);
            }
        }
    }
}