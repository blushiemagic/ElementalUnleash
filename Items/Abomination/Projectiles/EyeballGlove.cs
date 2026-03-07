using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination.Projectiles
{
    public class EyeballGlove : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.MaxUpdates = 2;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 5;
        }

        public override void AI()
        {
            Projectile.frame = (int)Projectile.ai[0];
            Projectile.velocity.Y += Projectile.velocity.Y < 0f ? 0.1f : 0.2f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.9f;
            }
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Projectile.ai[0] == 3f)
            {
                modifiers.FinalDamage.Flat += 20;
            }
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (Projectile.ai[0] == 3f)
            {
                modifiers.FinalDamage.Flat += 20;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int debuff = GetDebuff();
            if (debuff > 0)
            {
                target.AddBuff(debuff, GetDebuffTime());
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int debuff = GetDebuff();
            if (debuff > 0)
            {
                target.AddBuff(debuff, GetDebuffTime() / 2);
            }
        }

        public int GetDebuff()
        {
            switch ((int)Projectile.ai[0])
            {
            case 0:
                return BuffID.OnFire;
            case 1:
                return BuffID.Frostburn;
            case 2:
                return Mod.Find<ModBuff>("EtherealFlames").Type;
            case 3:
                return 0;
            case 4:
                return BuffID.Venom;
            case 5:
                return BuffID.Ichor;
            default:
                return 0;
            }
        }

        public int GetDebuffTime()
        {
            switch ((int)Projectile.ai[0])
            {
            case 0:
                return 600;
            case 1:
                return 400;
            case 2:
                return 300;
            case 3:
                return 0;
            case 4:
                return 400;
            case 5:
                return 900;
            default:
                return 0;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(127 + lightColor.R / 2, 127 + lightColor.G / 2, 127 + lightColor.B / 2, lightColor.A);
        }
    }
}
