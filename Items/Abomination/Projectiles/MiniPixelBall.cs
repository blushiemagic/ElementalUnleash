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
            // DisplayName.SetDefault("Mini Captive Element");
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.alpha = 255;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 3f)
            {
                Projectile.alpha = 0;
            }
            if (Projectile.ai[0] == 1f)
            {
                Projectile.coldDamage = true;
            }
            Color? color = GetColor();
            if (color.HasValue)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Mod.Find<ModDust>("Pixel").Type, 0f, 0f, 0, color.Value);
                Main.dust[dust].velocity += Projectile.velocity;
                Main.dust[dust].scale = 0.8f;
            }
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

        public Color? GetColor()
        {
            switch ((int)Projectile.ai[0])
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
    }
}