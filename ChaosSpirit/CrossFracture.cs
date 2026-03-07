using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.ChaosSpirit
{
    public class CrossFracture : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            CooldownSlot = 1;
        }

        public override void AI()
        {
            if (Projectile.ai[1] == 0f)
            {
                Projectile.localAI[0] = Main.rand.NextFloat();
            }
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] == 120f)
            {
                SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            }
            if (Projectile.ai[1] >= 150f)
            {
                Projectile.Kill();
            }
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (target.hurtCooldowns[1] <= 0)
            {
                BluemagicPlayer modPlayer = target.GetModPlayer<BluemagicPlayer>();
                modPlayer.constantDamage = 200;
                modPlayer.percentDamage = 1f / 3f;
                if (Main.expertMode)
                {
                    modPlayer.constantDamage = (int)(modPlayer.constantDamage * 1.5f);
                    modPlayer.percentDamage *= 1.5f;
                }
                modPlayer.chaosDefense = true;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(Mod.Find<ModBuff>("Undead").Type, 300, false);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (Vector2.Distance(target.Center, Projectile.Center) >= 600f)
            {
                return false;
            }
            return base.CanHitNPC(target);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Projectile.ai[1] >= 120f)
            {
                float width = Projectile.width * ((150f - Projectile.ai[1]) / 30f);
                float num = 0f;
                Vector2 offset = 2400f * Projectile.ai[0].ToRotationVector2();
                Vector2 start = Projectile.Center + offset;
                Vector2 end = Projectile.Center - offset;
                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, width, ref num))
                {
                    return true;
                }
                num = 0f;
                offset = 2400f * (Projectile.ai[0] + MathHelper.PiOver2).ToRotationVector2();
                start = Projectile.Center + offset;
                end = Projectile.Center - offset;
                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, width, ref num))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Color color = Main.hslToRgb(Projectile.localAI[0], 1f, 0.6f);
            float laserAlpha = 0f;
            if (Projectile.ai[1] >= 120f)
            {
                laserAlpha = 1f;
            }
            else if (Math.Abs(Projectile.ai[1] - 60) < 10)
            {
                laserAlpha = (10f - Math.Abs(Projectile.ai[1] - 60f)) / 40f;
            }
            float scale = 1f;
            if (Projectile.ai[1] >= 120f)
            {
                scale = (150f - Projectile.ai[1]) / 30f;
            }
            if (laserAlpha > 0f && scale > 0f)
            {
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
                Vector2 drawPos = Projectile.Center - Main.screenPosition;
                Vector2 drawScale = new Vector2(2400f, scale);
                float rotation = Projectile.ai[0];
                Color drawColor = color * laserAlpha;
                Main.spriteBatch.Draw(texture, drawPos, null, drawColor, rotation, origin, drawScale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(texture, drawPos, null, drawColor, rotation + MathHelper.PiOver2, origin, drawScale, SpriteEffects.None, 0f);
            }

            if (Projectile.ai[1] < 120f)
            {
                Texture2D texture = ModContent.Request<Texture2D>("Bluemagic/ChaosSpirit/CrossFractureHolder").Value;
                Vector2 drawCenter = Projectile.Center - Main.screenPosition;
                Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
                for (int k = 0; k < 4; k++)
                {
                    float rotation = Projectile.ai[0] + MathHelper.PiOver2 * k;
                    Vector2 drawPos = drawCenter + 240f * rotation.ToRotationVector2();
                    Main.spriteBatch.Draw(texture, drawPos, null, color, rotation, origin, 1f, SpriteEffects.None, 0f);
                }
            }
            return false;
        }
    }
}