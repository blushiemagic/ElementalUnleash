using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Blushie
{
    public class BlushieCharmProj : ModProjectile
    {
        private const int xRange = 600;
        private const int yRange = 320;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Charm of Blushie");
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Main.myPlayer == Projectile.owner)
            {
                if (!player.channel || player.noItems || player.CCed)
                {
                    Projectile.Kill();
                }
            }
            Projectile.Center = player.MountedCenter;
            Projectile.timeLeft = 2;
            player.itemTime = 2;
            player.itemAnimation = 2;

            Projectile.ai[0] += 1f;
            Projectile.damage = player.statLife;
            if (Projectile.ai[0] >= 240f && Main.myPlayer == Projectile.owner)
            {
                int useMana = Projectile.damage / 200;
                if (player.statMana < useMana && player.manaFlower)
                {
                    player.QuickMana();
                }
                if (player.statMana >= useMana)
                {
                    player.statMana -= useMana;
                }
                else
                {
                    Projectile.Kill();
                }
            }

            if (Projectile.localAI[1] == 0f)
            {
                Projectile.localAI[1] = 39f;
                Projectile.localAI[0] = Main.rand.Next(xRange);
            }
            Projectile.localAI[0] = Next(Projectile.localAI[0]);
        }

        private float Next(float seed)
        {
            return (seed * Projectile.localAI[1] + 97f) % (4 * xRange * yRange);
        }

        public override bool? CanDamage()
        {
            return Projectile.ai[0] > 120f;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox.X -= xRange;
            hitbox.Width += 2 * xRange;
            hitbox.Y -= yRange;
            hitbox.Height += 2 * yRange;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage.Flat += target.defense / 2;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.penetrate++;
            target.AddBuff(BuffID.OnFire, 300);
            target.AddBuff(BuffID.Frostburn, 300);
            target.AddBuff(BuffID.Lovestruck, 300);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawCenter = Projectile.Center - Main.screenPosition;
            Texture2D texture = ModContent.Request<Texture2D>("Bluemagic/Blushie/BlushieCharmHeart").Value;
            float scale = Projectile.ai[0] / 60f;
            if (scale > 1f)
            {
                scale = 1f;
            }
            Main.spriteBatch.Draw(texture, drawCenter, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(1.2f * scale, 1f), SpriteEffects.None, 0f);
            texture = ModContent.Request<Texture2D>("Bluemagic/Blushie/BlushieCharmTop").Value;
            Vector2 drawTop = drawCenter + new Vector2(0f, -yRange);
            Main.spriteBatch.Draw(texture, drawTop, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(scale, 1f), SpriteEffects.None, 0f);

            float distance = (Projectile.ai[0] - 60f) / 60f;
            if (distance < 0f)
            {
                distance = 0f;
            }
            if (distance > 1f)
            {
                distance = 1f;
            }
            Color color = new Color(160, 160, 160, 255);
            DrawLine(Main.spriteBatch, drawTop + new Vector2(-150f, 0f), 160f * distance);
            DrawLine(Main.spriteBatch, drawTop + new Vector2(-300f, 0f), 480f * distance);
            DrawLine(Main.spriteBatch, drawTop + new Vector2(-450f, 0f), 320f * distance);
            DrawLine(Main.spriteBatch, drawTop + new Vector2(150f, 0f), 160f * distance);
            DrawLine(Main.spriteBatch, drawTop + new Vector2(300f, 0f), 480f * distance);
            DrawLine(Main.spriteBatch, drawTop + new Vector2(450f, 0f), 320f * distance);
            texture = ModContent.Request<Texture2D>("Bluemagic/Blushie/BlushieCharmCrystalBlue").Value;
            Main.spriteBatch.Draw(texture, drawTop + new Vector2(-150f, 160f * distance), null, Color.White * scale, 0f, new Vector2(texture.Width / 2, 0), 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, drawTop + new Vector2(-300f, 480f * distance), null, Color.White * scale, 0f, new Vector2(texture.Width / 2, 0), 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, drawTop + new Vector2(-450f, 32f + 320f * distance), null, Color.White * scale, 0f, new Vector2(texture.Width / 2, 0), 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, drawTop + new Vector2(-450f, 320f * distance), null, Color.White * scale, 0f, new Vector2(texture.Width / 2, 0), 1f, SpriteEffects.None, 0f);
            texture = ModContent.Request<Texture2D>("Bluemagic/Blushie/BlushieCharmCrystalPink").Value;
            Main.spriteBatch.Draw(texture, drawTop + new Vector2(150f, 160f * distance), null, Color.White * scale, 0f, new Vector2(texture.Width / 2, 0), 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, drawTop + new Vector2(300f, 480f * distance), null, Color.White * scale, 0f, new Vector2(texture.Width / 2, 0), 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, drawTop + new Vector2(450f, 32f + 320f * distance), null, Color.White * scale, 0f, new Vector2(texture.Width / 2, 0), 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, drawTop + new Vector2(450f, 320f * distance), null, Color.White * scale, 0f, new Vector2(texture.Width / 2, 0), 1f, SpriteEffects.None, 0f);

            float seed = Projectile.localAI[0];
            texture = ModContent.Request<Texture2D>("Bluemagic/Blushie/BlushieCharmSparkle").Value;
            Vector2 topLeft = drawTop + new Vector2(-xRange, 0f);
            for (int k = (int)Projectile.ai[0] - 120; k < (int)Projectile.ai[0]; k++)
            {
                if (k > 120)
                {
                    Main.spriteBatch.Draw(texture, topLeft + new Vector2(seed % (2 * xRange), seed % (2 * yRange)), null, Color.White, 0.1f * (Projectile.ai[0] - k), new Vector2(texture.Width / 2, texture.Height / 2), 2f * (Projectile.ai[0] - k) / 120f, SpriteEffects.None, 0f);
                }
                seed = Next(seed);
            }
            
            return false;
        }

        private void DrawLine(SpriteBatch spriteBatch, Vector2 start, float height)
        {
            Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, start - new Vector2(2f, 0f), null, new Color(160, 160, 160), 0f, Vector2.Zero, new Vector2(0.25f, height / 16f), SpriteEffects.None, 0f);
        }
    }
}