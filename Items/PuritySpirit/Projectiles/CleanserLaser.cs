using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit.Projectiles
{
    public class CleanserLaser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cleanser Beam");
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = ProjAIStyleID.ThickLaser;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
        }

        public override bool PreAI()
        {
            int uuid = Projectile.GetByUUID(Projectile.owner, Projectile.ai[1]);
            if (uuid < 0)
            {
                Projectile.Kill();
                return false;
            }
            Projectile cannon = Main.projectile[uuid];
            if (cannon.active && cannon.type == Mod.Find<ModProjectile>("CleanserBeam").Type)
            {
                Vector2 direction = Vector2.Normalize(cannon.velocity);
                Projectile.Center = cannon.Center + 16f * direction + new Vector2(0f, -cannon.gfxOffY);
                Projectile.velocity = direction;
            }
            else
            {
                Projectile.Kill();
                return false;
            }
            if (Projectile.velocity.HasNaNs() || Projectile.velocity == Vector2.Zero)
            {
                Projectile.velocity = -Vector2.UnitY;
            }
            float rotation = Projectile.velocity.ToRotation();
            Projectile.rotation = rotation - MathHelper.PiOver2;
            Projectile.velocity = rotation.ToRotationVector2();

            int startTileX = (int)Projectile.Center.X / 16;
            int startTileY = (int)Projectile.Center.Y / 16;
            Vector2 endPoint = Projectile.Center + Projectile.velocity * 16f * 150f;
            int endTileX = (int)endPoint.X / 16;
            int endTileY = (int)endPoint.Y / 16;
            Tuple<int, int> collideTile;
            float length;
            if (!Collision.TupleHitLine(startTileX, startTileY, endTileX, endTileY, 0, 0, new List<Tuple<int, int>>(), out collideTile))
            {
                length = new Vector2(collideTile.Item1 - startTileX, collideTile.Item2 - startTileY).Length() * 16f;
            }
            else if (collideTile.Item1 == endTileX && collideTile.Item2 == endTileY)
            {
                length = 2400f;
            }
            else
            {
                length = new Vector2(collideTile.Item1 - startTileX, collideTile.Item2 - startTileY).Length() * 16f;
            }
            Projectile.localAI[1] = MathHelper.Lerp(Projectile.localAI[1], length, 0.5f);

            Projectile.localAI[0] += 1f;
            Projectile.localAI[0] %= 40f;

            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            Vector2 endPoint = Projectile.Center + Projectile.velocity * Projectile.localAI[1];
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, endPoint, 4f, ref point);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage.Flat += target.defense / 2;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.penetrate++;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 unit = Projectile.velocity * Projectile.localAI[1];
            float length = unit.Length();
            unit.Normalize();
            byte colorStrength = (byte)(100f + 100f * Math.Sin(Projectile.localAI[0] / 40f * MathHelper.TwoPi));
            Color color = new Color(colorStrength, 255, colorStrength) * 0.8f;
            for (float k = 0; k <= length; k += 4f)
            {
                Vector2 drawPos = Projectile.Center + unit * k - Main.screenPosition;
                Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, new Vector2(2, 2), 1f, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}