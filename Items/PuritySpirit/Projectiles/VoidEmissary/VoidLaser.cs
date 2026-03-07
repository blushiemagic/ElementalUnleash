using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit.Projectiles.VoidEmissary
{
    public class VoidLaser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = ProjAIStyleID.ThickLaser;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override bool PreAI()
        {
            Projectile minion = Main.projectile[(int)Projectile.ai[1]];
            if (minion.active && minion.type == Mod.Find<ModProjectile>("VoidEmissary").Type && minion.ai[0] == 2f)
            {
                Vector2 direction = minion.ai[1].ToRotationVector2();
                Projectile.Center = minion.Center + 30f * direction + new Vector2(0f, -minion.gfxOffY);
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
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, endPoint, 20f, ref point);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 position = Projectile.Center - Main.screenPosition;
            Vector2 origin = new Vector2(0, texture.Height / 2);
            Vector2 scale = new Vector2(Projectile.localAI[1] / 2f, 1f);
            Main.spriteBatch.Draw(texture, position, null, Color.White, Projectile.velocity.ToRotation(), origin, scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}