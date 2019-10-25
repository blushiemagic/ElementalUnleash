using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
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
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.magic = true;
            projectile.penetrate = -1;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (Main.myPlayer == projectile.owner)
            {
                if (!player.channel || player.noItems || player.CCed)
                {
                    projectile.Kill();
                }
            }
            projectile.Center = player.MountedCenter;
            projectile.timeLeft = 2;
            player.itemTime = 2;
            player.itemAnimation = 2;

            if (projectile.ai[0] < 120f && projectile.ai[0] % 30f == 0)
            {
                Main.PlaySound(2, projectile.Center, 15);
            }
            if (projectile.ai[0] == 120f)
            {
                Main.PlaySound(29, -1, -1, 104);
            }

            projectile.ai[0] += 1f;
            float interval = 120f;
            if (projectile.ai[0] > 120f)
            {
                interval = 30f;
            }
            if (projectile.ai[0] > 240f)
            {
                interval = 10f;
            }
            if (projectile.ai[0] % interval == 0f && Main.myPlayer == projectile.owner)
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
                    projectile.Kill();
                }
            }

            if (projectile.velocity == Vector2.Zero || projectile.velocity.HasNaNs())
            {
                projectile.velocity = -Vector2.UnitY;
            }
            Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - projectile.Center;
            if (Main.player[projectile.owner].gravDir == -1f)
            {
                target.Y = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - projectile.Center.Y;
            }
            if (target == Vector2.Zero)
            {
                target = -Vector2.UnitY;
            }
            float curRot = projectile.velocity.ToRotation();
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
            projectile.velocity = rotation.ToRotationVector2();
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            Vector2 endPoint = projectile.Center + projectile.velocity * 1000f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, endPoint, 4f, ref point);
        }

        public override bool CanDamage()
        {
            return projectile.ai[0] >= 120f;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += target.defense / 2;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 4;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.ai[0] < 90f)
            {
                float progress = projectile.ai[0] % 30f;
                Vector2 center = Main.player[projectile.owner].itemLocation;
                Texture2D texture = mod.GetTexture("Blushie/RadiantRainbowRayCharge");
                float scale = (30f - progress) / 30f;
                float alpha = 0.7f - 0.4f * scale;
                spriteBatch.Draw(texture, center - Main.screenPosition, null, Color.White * alpha, 0f, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
            }
            if (projectile.ai[0] >= 90f)
            {
                Texture2D texture = mod.GetTexture("Blushie/RadiantRainbowRayCharge");
                float scale = (projectile.ai[0] - 90f) / 30f;
                Vector2 center = Main.player[projectile.owner].Center;
                if (scale > 1f)
                {
                    scale = 1f;
                }
                spriteBatch.Draw(texture, center - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
            }
            if (projectile.ai[0] >= 120f)
            {
                Texture2D texture = Main.projectileTexture[projectile.type];
                Vector2 drawOrigin = projectile.Center - Main.screenPosition;
                float rotation = projectile.velocity.ToRotation();
                int colorOffset = (int)((projectile.ai[0] - 120f) / 10f) % colors.Length;
                Vector2 normal = new Vector2(-projectile.velocity.Y, projectile.velocity.X);
                float colorWidth = 6f;
                for (int k = 0; k < colors.Length; k++)
                {
                    Color color = colors[(k + colors.Length - colorOffset) % colors.Length];
                    float drawOffset = colorWidth * (k - colors.Length / 2f);
                    Vector2 drawPos = drawOrigin + drawOffset * normal;
                    spriteBatch.Draw(texture, drawPos, null, color, rotation, Vector2.Zero, new Vector2(500f, colorWidth / 2f), SpriteEffects.None, 0f);
                }
            }
            
            return false;
        }
    }
}