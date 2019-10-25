using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Blushie
{
    public class FirePulse : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "Terraria/FlameRing";
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frosty Laser Beam");
            ProjectileID.Sets.NeedsUUID[projectile.type] = true;
            ProjectileID.Sets.MinionShot[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.alpha = 63;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 1200 / 8;
        }

        public override void AI()
        {
            projectile.Center = Main.player[projectile.owner].Center;
            projectile.ai[0] += 1f;
            if (projectile.ai[0] > 1200f / 8f)
            {
                projectile.Kill();
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 center = projectile.Center;
            Vector2 scale = new Vector2(projectile.ai[0] * 8f);
            
            return Ellipse.Collides(center - scale, 2f * scale, new Vector2(targetHitbox.X, targetHitbox.Y), new Vector2(targetHitbox.Width, targetHitbox.Height));
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += target.defense / 2;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 1800);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float scale = projectile.ai[0] * 8f / 200f;
            Rectangle frame = new Rectangle(0, 400 * (int)((projectile.ai[0] / 5f) % 3), 400, 400);
            Vector2 drawPos = projectile.Center - Main.screenPosition;
            Vector2 origin = new Vector2(200f, 200f);
            Texture2D texture = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(texture, drawPos, frame, Color.White * 0.75f, 0f, origin, scale, SpriteEffects.None, 0f);            
            return false;
        }
    }
}