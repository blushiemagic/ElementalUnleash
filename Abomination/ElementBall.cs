using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Abomination
{
    public class ElementBall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Elemental Ball");
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.alpha = 255;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                PlaySound();
                if (Projectile.ai[0] < 0f)
                {
                    Projectile.alpha = 0;
                }
                if (Projectile.ai[0] == 44f)
                {
                    Projectile.coldDamage = true;
                }
                if (Projectile.ai[0] < 0f && Main.expertMode)
                {
                    CooldownSlot = 1;
                }
                Projectile.Name = GetName();
                Projectile.localAI[0] = 1f;
            }
            CreateDust();
        }

        public virtual void PlaySound()
        {
            SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
        }

        public virtual void CreateDust()
        {
            Color? color = GetColor();
            if (color.HasValue)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Mod.Find<ModDust>("Flame").Type, 0f, 0f, 0, color.Value);
                Main.dust[dust].velocity *= 0.4f;
                Main.dust[dust].velocity += Projectile.velocity;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if ((Main.expertMode || Main.rand.Next(2) == 0) && Projectile.ai[0] >= 0f)
            {
                target.AddBuff((int)Projectile.ai[0], (int)Projectile.ai[1], true);
            }
        }

        public virtual string GetName()
        {
            if (Projectile.ai[0] == 24f)
            {
                return "Fireball";
            }
            if (Projectile.ai[0] == 44f)
            {
                return "Frost Ball";
            }
            if (Projectile.ai[0] == Mod.Find<ModBuff>("EtherealFlames").Type)
            {
                return "Ethereal Fireball";
            }
            if (Projectile.ai[0] == 70f)
            {
                return "Venom Ball";
            }
            if (Projectile.ai[0] == 69f)
            {
                return "Ichor Ball";
            }
            return "Death Bubble";
        }

        public Color? GetColor()
        {
            if (Projectile.ai[0] == 24f)
            {
                return new Color(250, 10, 0);
            }
            if (Projectile.ai[0] == 44f)
            {
                return new Color(0, 230, 230);
            }
            if (Projectile.ai[0] == Mod.Find<ModBuff>("EtherealFlames").Type)
            {
                return new Color(0, 153, 230);
            }
            if (Projectile.ai[0] == 70f)
            {
                return new Color(0, 178, 0);
            }
            if (Projectile.ai[0] == 69f)
            {
                return new Color(230, 192, 0);
            }
            return null;
        }
    }
}