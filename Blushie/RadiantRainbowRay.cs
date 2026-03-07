using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Blushie
{
    public class RadiantRainbowRay : ModProjectile
    {
        private static Color[] colors = new Color[]
        {
            new Color(255, 0, 0),
            new Color(255, 128, 0),
            new Color(255, 255, 0),
            new Color(0, 255, 0),
            new Color(0, 255, 255),
            new Color(0, 0, 255),
            new Color(128, 0, 255)
        };

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
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

            if (Projectile.ai[0] < 120f && Projectile.ai[0] % 30f == 0)
            {
                SoundEngine.PlaySound(SoundID.Item15, Projectile.Center);
            }
            if (Projectile.ai[0] == 120f)
            {
                SoundEngine.PlaySound(SoundID.Zombie104);
            }

            Projectile.ai[0] += 1f;
            float interval = 120f;
            if (Projectile.ai[0] > 120f)
            {
                interval = 30f;
            }
            if (Projectile.ai[0] > 240f)
            {
                interval = 10f;
            }
            if (Projectile.ai[0] % interval == 0f && Main.myPlayer == Projectile.owner)
            {
                int useMana = player.inventory[player.selectedItem].mana;
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

            if (Projectile.velocity == Vector2.Zero || Projectile.velocity.HasNaNs())
            {
                Projectile.velocity = -Vector2.UnitY;
            }
            Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - Projectile.Center;
            if (Main.player[Projectile.owner].gravDir == -1f)
            {
                target.Y = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - Projectile.Center.Y;
            }
            if (target == Vector2.Zero)
            {
                target = -Vector2.UnitY;
            }
            float curRot = Projectile.velocity.ToRotation();
            float targetRot = target.ToRotation();
            if (targetRot > curRot + MathHelper.Pi)
            {
                targetRot -= MathHelper.TwoPi;
            }
            if (curRot > targetRot + MathHelper.Pi)
            {
                curRot -= MathHelper.TwoPi;
            }
            float rotation = 0.9f * curRot + 0.1f * targetRot;
            Projectile.velocity = rotation.ToRotationVector2();
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            Vector2 endPoint = Projectile.Center + Projectile.velocity * 1000f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, endPoint, 4f, ref point);
        }

        public override bool? CanDamage()
        {
            return Projectile.ai[0] >= 120f;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage.Flat += target.defense / 2;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 4;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[0] < 90f)
            {
                float progress = Projectile.ai[0] % 30f;
                Vector2 center = Main.player[Projectile.owner].itemLocation;
                Texture2D texture = ModContent.Request<Texture2D>("Bluemagic/Blushie/RadiantRainbowRayCharge").Value;
                float scale = (30f - progress) / 30f;
                float alpha = 0.7f - 0.4f * scale;
                Main.spriteBatch.Draw(texture, center - Main.screenPosition, null, Color.White * alpha, 0f, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
            }
            if (Projectile.ai[0] >= 90f)
            {
                Texture2D texture = ModContent.Request<Texture2D>("Bluemagic/Blushie/RadiantRainbowRayCharge").Value;
                float scale = (Projectile.ai[0] - 90f) / 30f;
                Vector2 center = Main.player[Projectile.owner].Center;
                if (scale > 1f)
                {
                    scale = 1f;
                }
                Main.spriteBatch.Draw(texture, center - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
            }
            if (Projectile.ai[0] >= 120f)
            {
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                Vector2 drawOrigin = Projectile.Center - Main.screenPosition;
                float rotation = Projectile.velocity.ToRotation();
                int colorOffset = (int)((Projectile.ai[0] - 120f) / 10f) % colors.Length;
                Vector2 normal = new Vector2(-Projectile.velocity.Y, Projectile.velocity.X);
                float colorWidth = 6f;
                for (int k = 0; k < colors.Length; k++)
                {
                    Color color = colors[(k + colors.Length - colorOffset) % colors.Length];
                    float drawOffset = colorWidth * (k - colors.Length / 2f);
                    Vector2 drawPos = drawOrigin + drawOffset * normal;
                    Main.spriteBatch.Draw(texture, drawPos, null, color, rotation, Vector2.Zero, new Vector2(500f, colorWidth / 2f), SpriteEffects.None, 0f);
                }
            }
            
            return false;
        }
    }
}