using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.ChaosSpirit
{
    public class ChaosPearl : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            CooldownSlot = 1;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] < 180f)
            {
                Player player = Main.player[(int)Projectile.ai[1]];
                Vector2 offset = player.Center - Projectile.Center;
                if (offset != Vector2.Zero)
                {
                    float strength = (180f - Projectile.ai[1]) / 180f;
                    float direction = Projectile.velocity.ToRotation();
                    float target = offset.ToRotation();
                    if (target > direction + MathHelper.Pi)
                    {
                        target -= MathHelper.TwoPi;
                    }
                    else if (target < direction - MathHelper.Pi)
                    {
                        target += MathHelper.TwoPi;
                    }
                    float difference = (target - direction) * 0.5f * strength;
                    Projectile.velocity = Projectile.velocity.Length() * (direction + difference).ToRotationVector2();
                }
            }
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 600f)
            {
                Projectile.Kill();
            }
            Projectile.localAI[1] += 1f;
            Projectile.localAI[1] %= 30f;
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (target.hurtCooldowns[1] <= 0)
            {
                BluemagicPlayer modPlayer = target.GetModPlayer<BluemagicPlayer>();
                modPlayer.constantDamage = 100;
                modPlayer.percentDamage = 1f / 6f;
                if (Main.expertMode)
                {
                    modPlayer.constantDamage *= 2;
                    modPlayer.percentDamage *= 2f;
                }
                modPlayer.chaosDefense = true;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(Mod.Find<ModBuff>("Undead").Type, 150, false);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            Color color = ChaosSpiritArm.GetColor((int)Projectile.ai[0]);
            //color.R = (byte)((255 + color.R) / 2);
            //color.G = (byte)((255 + color.G) / 2);
            //color.B = (byte)((255 + color.B) / 2);
            return color;
        }

        public override void PostDraw(Color lightColor)
        {
            float alpha = 0.25f + Math.Abs(Projectile.localAI[1] - 15f) / 30f;
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Bluemagic/ChaosSpirit/ChaosBitMask").Value, Projectile.position - Main.screenPosition, Color.White * alpha);
        }
    }
}