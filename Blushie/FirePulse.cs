using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
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
                return "Terraria/Images/FlameRing";
            }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Frosty Laser Beam");
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.alpha = 63;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1200 / 8;
        }

        public override void AI()
        {
            Projectile.Center = Main.player[Projectile.owner].Center;
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 1200f / 8f)
            {
                Projectile.Kill();
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 center = Projectile.Center;
            Vector2 scale = new Vector2(Projectile.ai[0] * 8f);
            
            return Ellipse.Collides(center - scale, 2f * scale, new Vector2(targetHitbox.X, targetHitbox.Y), new Vector2(targetHitbox.Width, targetHitbox.Height));
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SourceDamage.Flat += target.defense / 2;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 1800);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            float scale = Projectile.ai[0] * 8f / 200f;
            Rectangle frame = new Rectangle(0, 400 * (int)((Projectile.ai[0] / 5f) % 3), 400, 400);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 origin = new Vector2(200f, 200f);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, drawPos, frame, Color.White * 0.75f, 0f, origin, scale, SpriteEffects.None, 0f);            
            return false;
        }
    }
}