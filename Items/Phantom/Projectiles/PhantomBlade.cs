using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom.Projectiles
{
    public class PhantomBlade : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 12;
            Projectile.alpha = 70;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.manualDirectionChange = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.itemAnimation == 0)
            {
                Projectile.active = false;
                return;
            }
            if (player.direction + player.gravDir == 0)
            {
                Projectile.spriteDirection = -1;
            }
            else
            {
                Projectile.spriteDirection = 1;
            }
            Projectile.direction = player.direction;
            Projectile.position = player.itemLocation;
            Projectile.rotation = player.itemRotation;
            Projectile.rotation -= (float)Math.PI / 4f;
            if (player.direction == -1)
            {
                Projectile.rotation -= (float)Math.PI / 2f;
            }
            if (player.gravDir == -1)
            {
                if (player.direction == 1)
                {
                    Projectile.rotation += (float)Math.PI / 2f;
                }
                else
                {
                    Projectile.rotation -= (float)Math.PI / 2f;
                }
            }
            Projectile.position.X += 2 * 14f * (float)Math.Cos(Projectile.rotation);
            Projectile.position.Y += 2 * 14f * (float)Math.Sin(Projectile.rotation);
            int y = Main.rand.Next(Projectile.height) - Projectile.height / 2;
            int x = Main.rand.Next(Projectile.width);
            float rotatedX = x * (float)Math.Cos(Projectile.rotation) - y * (float)Math.Sin(Projectile.rotation);
            float rotatedY = x * (float)Math.Sin(Projectile.rotation) + y * (float)Math.Cos(Projectile.rotation);
            Dust.NewDust(Projectile.position + new Vector2(rotatedX, rotatedY), 0, 0, Mod.Find<ModDust>("SpectreDust").Type);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            Vector2 endPoint = Projectile.position + Projectile.width * Projectile.rotation.ToRotationVector2();
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.position, endPoint, Projectile.height, ref point);
        }

        public override Color? GetAlpha(Color color)
        {
            return Color.White * ((255 - Projectile.alpha) / 255f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawPos = Projectile.position - Main.screenPosition;
            Color color = GetAlpha(lightColor).Value;
            float rotation = Projectile.rotation;
            Vector2 origin = new Vector2(0f, texture.Height / 2);
            SpriteEffects effects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            Main.spriteBatch.Draw(texture, drawPos, null, color, rotation, origin, 1f, effects, 0f);
            return false;
        }
    }
}