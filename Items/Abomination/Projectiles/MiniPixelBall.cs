using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination.Projectiles
{
    public class MiniPixelBall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mini Captive Element");
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionShot[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.alpha = 255;
            projectile.timeLeft = 600;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.extraUpdates = 1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (projectile.ai[0] == 3f)
            {
                projectile.alpha = 0;
            }
            if (projectile.ai[0] == 1f)
            {
                projectile.coldDamage = true;
            }
            Color? color = GetColor();
            if (color.HasValue)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("Pixel"), 0f, 0f, 0, color.Value);
                Main.dust[dust].velocity += projectile.velocity;
                Main.dust[dust].scale = 0.8f;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.ai[0] == 3f)
            {
                damage += 20;
            }
        }

        public override void ModifyHitPvp(Player target, ref int damage, ref bool crit)
        {
            if (projectile.ai[0] == 3f)
            {
                damage += 20;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int debuff = GetDebuff();
            if (debuff > 0)
            {
                target.AddBuff(debuff, GetDebuffTime());
            }
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            int debuff = GetDebuff();
            if (debuff > 0)
            {
                target.AddBuff(debuff, GetDebuffTime() / 2);
            }
        }

        public Color? GetColor()
        {
            switch ((int)projectile.ai[0])
            {
            case 0:
                return new Color(250, 10, 0);
            case 1:
                return new Color(0, 230, 230);
            case 2:
                return new Color(0, 153, 230);
            case 3:
                return null;
            case 4:
                return new Color(0, 178, 0);
            case 5:
                return new Color(230, 192, 0);
            default:
                return null;
            }
        }

        public int GetDebuff()
        {
            switch ((int)projectile.ai[0])
            {
            case 0:
                return BuffID.OnFire;
            case 1:
                return BuffID.Frostburn;
            case 2:
                return mod.BuffType("EtherealFlames");
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
            switch ((int)projectile.ai[0])
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
    }
}