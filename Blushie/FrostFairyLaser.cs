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
    public class FrostFairyLaser : ModProjectile
    {
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
            Projectile.aiStyle = ProjAIStyleID.ThickLaser;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override bool PreAI()
        {
            int uuid = Projectile.GetByUUID(Projectile.owner, Projectile.ai[1]);
            if (uuid < 0)
            {
                Projectile.Kill();
                return false;
            }
            Projectile wings = Main.projectile[uuid];
            if (wings.active && wings.type == Mod.Find<ModProjectile>("FrostFairyWingsProj").Type)
            {
                Projectile.Center = wings.Center;
                if (Projectile.ai[0] == 0f)
                {
                    Projectile.Center += new Vector2(-18f, -13f);
                }
                else if (Projectile.ai[0] == 1f)
                {
                    Projectile.Center += new Vector2(18f, -13f);
                }
                else if (Projectile.ai[0] == 2f)
                {
                    Projectile.Center += new Vector2(-16f, 15f);
                }
                else
                {
                    Projectile.Center += new Vector2(16f, 15f);
                }
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
            float length = 1000f;
            Projectile.localAI[1] = MathHelper.Lerp(Projectile.localAI[1], length, 0.5f);

            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 10f)
            {
                Projectile.Kill();
            }
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
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, endPoint, 16f, ref point);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage.Flat += target.defense / 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 direction = Projectile.velocity * Projectile.localAI[1];
            float rotation = direction.ToRotation();
            float length = Projectile.localAI[1];
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Color color = new Color(100, 190, 255);
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, rotation, new Vector2(0f, 2f), new Vector2(length / 4f, 4f), SpriteEffects.None, 0f);
            color = new Color(127, 255, 255);
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, rotation, new Vector2(0f, 2f), new Vector2(length / 4f, 3f), SpriteEffects.None, 0f);
            color = new Color(255, 255, 255);
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, rotation, new Vector2(0f, 2f), new Vector2(length / 4f, 2f), SpriteEffects.None, 0f);
            
            return false;
        }
    }
}