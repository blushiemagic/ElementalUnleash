using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.PuritySpirit
{
    public class PureCrystal : ModProjectile
    {
        private int timer = 0;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crystal of Cleansing");
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
        }

        public override void AI()
        {
            if (Projectile.velocity.X != 0f)
            {
                Projectile.localAI[0] = Projectile.velocity.X;
                Projectile.velocity.X = 0f;
            }
            if (Projectile.velocity.Y != 0f)
            {
                Projectile.localAI[1] = Projectile.velocity.Y;
                Projectile.velocity.Y = 0f;
            }
            NPC center = Main.npc[(int)Projectile.ai[0]];
            if (!center.active || center.type != Mod.Find<ModNPC>("PuritySpirit").Type)
            {
                Projectile.Kill();
            }
            if (timer < 120)
            {
                Projectile.alpha = (120 - timer) * 255 / 120;
                timer++;
            }
            else
            {
                Projectile.alpha = 0;
                Projectile.hostile = true;
            }
            Projectile.timeLeft = 2;
            Projectile.ai[1] += 2f * (float)Math.PI / 600f * Projectile.localAI[1];
            Projectile.ai[1] %= 2f * (float)Math.PI;
            Projectile.rotation -= 2f * (float)Math.PI / 120f * Projectile.localAI[1];
            Projectile.Center = center.Center + Projectile.localAI[0] * new Vector2((float)Math.Cos(Projectile.ai[1]), (float)Math.Sin(Projectile.ai[1]));
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            for (int k = 0; k < Player.MaxBuffs; k++)
            {
                if (target.buffType[k] > 0 && target.buffTime[k] > 0 && !BuffID.Sets.NurseCannotRemoveDebuff[target.buffType[k]] && Main.rand.Next(2) == 0)
                {
                    target.DelBuff(k);
                    k--;
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * ((255 - Projectile.alpha) / 255f);
        }

        public override void PostDraw(Color lightColor)
        {
            //Vector2 drawPos = projectile.position - Main.screenPosition;
            //spriteBatch.Draw(ModContent.Request<Texture2D>("Bluemagic/Projectiles/PuritySpirit/PureCrystalShield"), drawPos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            if (!Projectile.hostile)
            {
                return;
            }
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 drawCenter = new Vector2(24f, 24f);
            for (int k = 2; k <= 24; k += 2)
            {
                float scale = 2f * k / 48f;
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Bluemagic/PuritySpirit/PureCrystalRing").Value, drawPos, null, Color.White * ShieldTransparency(k), 0f, drawCenter, scale, SpriteEffects.None, 0f);
            }
        }

        private float ShieldTransparency(int radius)
        {
            switch (radius)
            {
                case 24:
                    return 0.5f;
                case 22:
                    return 0.35f;
                case 20:
                    return 0.25f;
                case 18:
                    return 0.2f;
                case 16:
                    return 0.15f;
                case 14:
                    return 0.1f;
                case 12:
                    return 0.06f;
                case 10:
                    return 0.03f;
                default:
                    return 0.01f;
            }
        }
    }
}