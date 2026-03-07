using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit.Projectiles
{
    public class PrismaticShocker : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || player.inventory[player.selectedItem].type != Mod.Find<ModItem>("PrismaticShocker").Type)
            {
                Projectile.Kill();
                return;
            }
            if (Projectile.ai[0] == 0f)
            {
                Vector2 offset = Projectile.Center - player.Center;
                Projectile.ai[0] = offset.Length();
                Projectile.ai[1] = offset.ToRotation();
            }
            Projectile.ai[1] += 0.02f;
            Projectile.ai[1] %= MathHelper.TwoPi;
            if (Projectile.ai[0] == 0f)
            {
                Projectile.Center = player.Center;
            }
            else
            {
                Projectile.Center = player.Center + Projectile.ai[0] * Projectile.ai[1].ToRotationVector2();
            }
            Projectile.rotation -= 0.05f;
            Projectile.localAI[0] += 0.2f;
            Projectile.localAI[0] %= 4f;
            Color light = GetAlpha(Color.White).Value;
            Lighting.AddLight(Projectile.Center, light.R / 255f, light.G / 255f, light.B / 255f);
            Projectile.timeLeft = 2;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int k = Projectile.whoAmI + 1; k < 1000; k++)
            {
                Projectile proj = Main.projectile[k];
                if (proj.active && proj.owner == Projectile.owner && proj.type == Projectile.type)
                {
                    if (BeamCheck(proj, targetHitbox))
                    {
                        return true;
                    }
                }
            }
            return null;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 4;
        }

        private bool BeamCheck(Projectile proj, Rectangle targetHitbox)
        {
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, proj.Center, 6f, ref point);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            int r = (Main.DiscoR + 4 * 255) / 5;
            int g = (Main.DiscoG + 4 * 255) / 5;
            int b = (Main.DiscoB + 4 * 255) / 5;
            return new Color(r, g, b);
        }

        public Color BeamColor()
        {
            int r = (Main.DiscoR + 255) / 2;
            int g = (Main.DiscoG + 255) / 2;
            int b = (Main.DiscoB + 255) / 2;
            return new Color(r, g, b);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            for (int k = Projectile.whoAmI + 1; k < 1000; k++)
            {
                Projectile proj = Main.projectile[k];
                if (proj.active && proj.owner == Projectile.owner && proj.type == Projectile.type)
                {
                    DrawBeamTo(Main.spriteBatch, proj, lightColor);
                }
            }
            return true;
        }

        private void DrawBeamTo(SpriteBatch spriteBatch, Projectile proj, Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("Bluemagic/Items/PuritySpirit/Projectiles/PrismaticShockerBeam").Value;
            Vector2 unit = proj.Center - Projectile.Center;
            float length = unit.Length();
            unit.Normalize();
            float rotation = unit.ToRotation();
            Color color = BeamColor() * 0.6f;   
            for (float k = Projectile.localAI[0]; k <= length; k += 8f)
            {
                Vector2 drawPos = Projectile.Center + unit * k - Main.screenPosition;
                Main.spriteBatch.Draw(texture, drawPos, null, color, rotation, new Vector2(4, 4), 1f, SpriteEffects.None, 0f);
            }
        }
    }
}