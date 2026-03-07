using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination.Projectiles
{
    public class EyeballTome : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.MaxUpdates = 4;
        }

        public override void AI()
        {
            Projectile.frame = (int)Projectile.ai[0];
            Projectile.ai[1] += 0.25f;
            if (Projectile.ai[1] >= 120f && Projectile.velocity == Vector2.Zero)
            {
                float distance = 800f;
                NPC npc = null;
                for (int k = 0; k < 200; k++)
                {
                    NPC check = Main.npc[k];
                    if (check.active && check.CanBeChasedBy(this))
                    {
                        float checkDist = Vector2.Distance(Projectile.Center, check.Center);
                        if (checkDist < distance)
                        {
                            npc = check;
                            distance = checkDist;
                        }
                    }
                }
                if (npc != null)
                {
                    Vector2 offset = npc.Center - Projectile.Center;
                    if (distance > 0f)
                    {
                        offset.Normalize();
                        offset *= 8f;
                        Projectile.velocity = offset;
                    }
                }
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

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            Rectangle frame = new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight);
            Color color = GetAlpha(lightColor).Value;
            for (int k = Projectile.oldPos.Length - 1; k >= 0; k -= 2)
            {
                float alpha = (float)(Projectile.oldPos.Length - k + 1) / (float)(Projectile.oldPos.Length + 2);
                Main.spriteBatch.Draw(texture, Projectile.oldPos[k] - Main.screenPosition, frame, color * alpha, Projectile.rotation, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}
