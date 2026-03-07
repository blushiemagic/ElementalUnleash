using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.ChaosSpirit
{
    public class ChaosBit : ModProjectile
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
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] > 600f)
            {
                Projectile.Kill();
            }
            Projectile.localAI[0] += 1f;
            Projectile.localAI[0] %= 30f;
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
            float alpha = 0.25f + Math.Abs(Projectile.localAI[0] - 15f) / 30f;
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Bluemagic/ChaosSpirit/ChaosBitMask").Value, Projectile.position - Main.screenPosition, Color.White * alpha);
        }
    }
}