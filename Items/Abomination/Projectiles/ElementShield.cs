using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination.Projectiles
{
    public class ElementShield : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Six-Color Shield");
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.alpha = 75;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                if (Projectile.ai[0] == 1)
                {
                    Projectile.coldDamage = true;
                }
                if (Projectile.ai[0] == 3)
                {
                    Projectile.damage = (int)(1.2f * Projectile.damage);
                }
                Projectile.Name = GetName();
                Projectile.localAI[0] = 1f;
            }
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            if (modPlayer.elementShields <= Projectile.ai[0])
            {
                Projectile.Kill();
                return;
            }
            Projectile.timeLeft = 2;
            Projectile.Center = player.Center;
            if (Projectile.ai[0] > 0f)
            {
                float offset = (Projectile.ai[0] - 1f) / (modPlayer.elementShields - 1);
                float rotation = modPlayer.elementShieldPos / 300f + offset;
                rotation = (rotation % 1f) * 2f * (float)Math.PI;
                Projectile.position += 160f * new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                Projectile.rotation = -rotation;
            }
            LightColor();
            Projectile.frame = (int)Projectile.ai[0];
            Projectile.ai[1] += 1f;
            Projectile.ai[1] %= 300f;
            Projectile.alpha = 75 + (int)(50 * Math.Sin(Projectile.ai[1] * 2f * (float)Math.PI / 300f));
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.Next(2) == 0)
            {
                int debuff = GetDebuff();
                if (debuff > 0)
                {
                    target.AddBuff(debuff, GetDebuffTime());
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.rand.Next(2) == 0)
            {
                int debuff = GetDebuff();
                if (debuff > 0)
                {
                    target.AddBuff(debuff, GetDebuffTime() / 2);
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public string GetName()
        {
            switch ((int)Projectile.ai[0])
            {
                case 0:
                    return "Fire Shield";
                case 1:
                    return "Frost Shield";
                case 2:
                    return "Ethereal Shield";
                case 3:
                    return "Foam Shield";
                case 4:
                    return "Venom Shield";
                case 5:
                    return "Ichor Shield";
                default:
                    return Projectile.Name;
            }
        }

        public void LightColor()
        {
            float r = 0f;
            float g = 0f;
            float b = 0f;
            switch ((int)Projectile.ai[0])
            {
                case 0:
                    r = 1f;
                    g = 0.25f;
                    b = 0.25f;
                    break;
                case 1:
                    r = 0.25f;
                    g = 0.75f;
                    b = 1f;
                    break;
                case 2:
                    r = 0.25f;
                    g = 0.25f;
                    b = 1f;
                    break;
                case 3:
                    r = 0.5f;
                    g = 0.7f;
                    b = 0.75f;
                    break;
                case 4:
                    r = 0.25f;
                    g = 0.75f;
                    b = 0.25f;
                    break;
                case 5:
                    r = 1f;
                    g = 1f;
                    b = 0.25f;
                    break;
            }
            Lighting.AddLight(Projectile.position, r, g, b);
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