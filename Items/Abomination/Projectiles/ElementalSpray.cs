using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination.Projectiles
{
    public class ElementalSpray : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 4;
            Projectile.extraUpdates = 2;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            if (Projectile.timeLeft > 60)
            {
                Projectile.timeLeft = 60;
            }
            if (Projectile.ai[1] > 6f)
            {
                Projectile.ai[1] += 1f;
                if (Main.rand.Next(2) == 0)
                {
                    int dustType = DustType();
                    Dust dust;
                    if (dustType == 171)
                    {
                        int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100);
                        dust = Main.dust[dustIndex];
                        if (Main.rand.Next(3) != 0)
                        {
                            dust.scale *= 3f;
                            dust.noGravity = true;
                            dust.velocity *= 2f;
                        }
                        dust.scale *= 1.15f;
                        dust.velocity *= 1.2f;
                    }
                    else if (dustType == Mod.Find<ModDust>("Bubble").Type)
                    {
                        int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 0, default(Color), 0.75f);
                        dust = Main.dust[dustIndex];
                        if (Main.rand.Next(3) != 0)
                        {
                            dust.scale *= 1.5f;
                            dust.velocity *= 2f;
                        }
                        dust.velocity *= 1.2f;
                    }
                    else
                    {
                        int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100);
                        dust = Main.dust[dustIndex];
                        if (Main.rand.Next(3) != 0)
                        {
                            dust.noGravity = true;
                            dust.scale *= 3f;
                            dust.velocity *= 2f;
                        }
                        dust.scale *= 1.5f;
                        dust.velocity *= 1.2f;
                    }
                    if (Projectile.ai[1] == 7f)
                    {
                        dust.scale *= 0.25f;
                    }
                    else if (Projectile.ai[1] == 8f)
                    {
                        dust.scale *= 0.5f;
                    }
                    else if (Projectile.ai[1] == 9f)
                    {
                        dust.scale *= 0.75f;
                    }
                }
            }
            else
            {
                Projectile.ai[1] += 1f;
            }
            Projectile.rotation += 0.3f * Projectile.direction;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox.X -= 30;
            hitbox.Y -= 30;
            hitbox.Width += 60;
            hitbox.Height += 60;
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

        public int DustType()
        {
            switch ((int)Projectile.ai[0])
            {
            case 0:
                return 6;
            case 1:
                return 135;
            case 2:
                return Mod.Find<ModDust>("EtherealFlame").Type;
            case 3:
                return Mod.Find<ModDust>("Bubble").Type;
            case 4:
                return 171;
            case 5:
                return 169;
            default:
                return 6;
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
