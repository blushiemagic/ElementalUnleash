using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Bluemagic.Phantom
{
    public class PhantomBladeHostile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Phantom Blade");
        }

        public override void SetDefaults()
        {
            Projectile.width = 400;
            Projectile.height = 24;
            Projectile.alpha = 70;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.maxPenetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Projectile.ai[1] == 0f)
            {
                Projectile.rotation = -MathHelper.Pi * 0.75f;
            }
            NPC center = Main.npc[(int)Projectile.ai[0]];
            Projectile.position = center.Center;
            Projectile.rotation += (float)Math.PI * 1.5f / 60f;
            Projectile.position.X += (float)Math.Cos(Projectile.rotation);
            Projectile.position.Y += (float)Math.Sin(Projectile.rotation);

            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] > 60f)
            {
                Projectile.Kill();
            }

            int y = Main.rand.Next(Projectile.height) - Projectile.height / 2;
            int x = Main.rand.Next(Projectile.width);
            float rotatedX = x * (float)Math.Cos(Projectile.rotation) - y * (float)Math.Sin(Projectile.rotation);
            float rotatedY = x * (float)Math.Sin(Projectile.rotation) + y * (float)Math.Cos(Projectile.rotation);
            Dust.NewDust(Projectile.position + new Vector2(rotatedX, rotatedY), 0, 0, Mod.Find<ModDust>("Phantom").Type);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            Vector2 endPoint = Projectile.position + Projectile.width * Projectile.rotation.ToRotationVector2();
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.position, endPoint, Projectile.height, ref point);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.rand.Next(2) == 0)
            {
                target.AddBuff(Mod.Find<ModBuff>("EtherealFlames").Type, 300, true);
            }
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
            SpriteEffects effects = SpriteEffects.None;
            Main.spriteBatch.Draw(texture, drawPos, null, color, rotation, origin, 1f, effects, 0f);
            return false;
        }
    }
}