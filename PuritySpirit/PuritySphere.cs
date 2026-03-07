using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.PuritySpirit
{
    public class PuritySphere : ModProjectile
    {
        public const float radius = 240f;
        public const int strikeTime = 20;
        private int timer = -60;
        public int maxTimer;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Purity Eye");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 120;
            CooldownSlot = 1;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
            writer.Write(maxTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
            maxTimer = reader.ReadInt32();
        }

        public override void AI()
        {
            if (Projectile.velocity.X != 0f)
            {
                Projectile.localAI[0] = Projectile.velocity.X == -1f ? 0f : Projectile.velocity.X;
                Projectile.velocity.X = 0f;
            }
            if (Projectile.velocity.Y != 0f)
            {
                Projectile.localAI[1] = Projectile.velocity.Y;
                Projectile.velocity.Y = 0f;
            }
            if (Projectile.knockBack != 0f)
            {
                maxTimer = (int)Projectile.knockBack;
                Projectile.knockBack = 0f;
            }
            if (timer < 0)
            {
                Projectile.alpha = -timer * 3;
            }
            else
            {
                Projectile.alpha = 0;
                Projectile.hostile = true;
            }
            if (Projectile.localAI[0] != 255f)
            {
                Player player = Main.player[(int)Projectile.localAI[0]];
                if (!player.active || player.dead)
                {
                    Projectile.localAI[0] = 255f;
                }
            }
            Vector2 center = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            if (timer < 0 && Projectile.localAI[0] != 255f)
            {
                Vector2 newCenter = Main.player[(int)Projectile.localAI[0]].Center;
                Projectile.position += newCenter - center;
                Projectile.ai[0] = newCenter.X;
                Projectile.ai[1] = newCenter.Y;
                center = newCenter;
            }
            float rotateSpeed = 2f * (float)Math.PI / 60f / 4f * Projectile.localAI[1];
            if (timer < maxTimer)
            {
                Projectile.Center = Projectile.Center.RotatedBy(rotateSpeed, center);
            }
            else
            {
                Vector2 offset = Projectile.Center - center;
                offset.Normalize();
                offset *= radius * ((float)strikeTime + maxTimer - timer) / (float)strikeTime;
                Projectile.Center = center + offset;
            }
            if (timer == maxTimer)
            {
                BluemagicPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<BluemagicPlayer>();
                if (modPlayer.heroLives > 0)
                {
                    SoundEngine.PlaySound(SoundID.Item12);
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.Item12, Projectile.Center);
                }
                Projectile.hostile = true;
            }
            if (timer >= maxTimer + strikeTime)
            {
                Projectile.Kill();
            }
            timer++;
            Projectile.rotation += rotateSpeed * -5f * Projectile.localAI[1];
            Projectile.spriteDirection = Projectile.localAI[1] < 0 ? -1 : 1;
            if (Projectile.frame < 4)
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 8)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;
                    Projectile.frame %= 4;
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * ((255 - Projectile.alpha / 2) / 255f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            for (int k = Projectile.oldPos.Length - 1; k >= 0; k--)
            {
                if (k % 4 == 0)
                {
                    Color alpha = GetAlpha(lightColor).Value * (1f - (float)k / (float)Projectile.oldPos.Length);
                    Main.spriteBatch.Draw(texture, Projectile.oldPos[k] + Projectile.Size / 2f - Main.screenPosition, new Rectangle(0, Projectile.frame * texture.Height / 4, texture.Width, texture.Height / 4), alpha, Projectile.oldRot[k], new Vector2(20, 20), 1f, SpriteEffects.None, 0f);
                }
            }
            return true;
        }
    }
}