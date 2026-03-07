using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.ChaosSpirit
{
    public class DissonanceOrb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            CooldownSlot = 1;
        }

        private int timer = 0;
        private bool synced = false;

        public override void AI()
        {
            if (Main.netMode == NetmodeID.Server && !synced)
            {
                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.whoAmI);
                synced = true;
            }
            if (timer == 0)
            {
                float direction = 1f;
                if (Projectile.ai[0] < 0f)
                {
                    Projectile.ai[0] += 1f;
                    Projectile.ai[0] *= -1f;
                    direction = -1f;
                }
                Vector2 target = Main.player[(int)Projectile.ai[0]].Center;
                Projectile.ai[0] = (target - Projectile.Center).ToRotation();
                Projectile.localAI[0] = direction;
            }
            timer++;
            if (timer > 180)
            {
                Projectile.ai[0] += Projectile.localAI[0] * (0.005f + ((timer - 180f) / 180f * 0.015f));
            }
            if (timer > 360)
            {
                Projectile.Kill();
            }
            Projectile.rotation += 0.05f;
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
            if (timer > 120)
            {
                float num = 0f;
                int other = Main.projectileIdentity[Projectile.owner, (int)Projectile.ai[1]];
                if (other >= 0)
                {
                    Vector2 laserTarget = Main.projectile[other].Center;
                    Vector2 offset = laserTarget - Projectile.Center;
                    Vector2 end = Projectile.Center + offset;
                    if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, end, 16, ref num))
                    {
                        return true;
                    }
                }
                {
                    num = 0f;
                    Vector2 direction = Projectile.ai[0].ToRotationVector2();
                    Vector2 end = Projectile.Center + 2400f * direction;
                    if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, end, 16, ref num))
                    {
                        return true;
                    }
                }
            }
            return base.Colliding(projHitbox, targetHitbox);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture;
            Vector2 origin;
            Vector2 drawPos;

            float laserAlpha = 0f;
            if (timer > 120)
            {
                laserAlpha = 1f;
            }
            else if (Math.Abs(timer - 60) < 20)
            {
                laserAlpha = (20f - Math.Abs(timer - 60)) / 40f;
            }
            else if (timer > 100)
            {
                laserAlpha = (timer - 100) / 20f;
            }
            if (laserAlpha > 0f)
            {
                texture = ModContent.Request<Texture2D>("Bluemagic/ChaosSpirit/DissonanceRay").Value;
                Color color = Color.White * laserAlpha;
                origin = new Vector2(texture.Width / 2, texture.Height / 2);
                int other = Main.projectileIdentity[Projectile.owner, (int)Projectile.ai[1]];
                if (other >= 0)
                {
                    Vector2 laserTarget = Main.projectile[other].Center;
                    Vector2 direction = laserTarget - Projectile.Center;
                    float length = direction.Length();
                    direction.Normalize();
                    float rotation = direction.ToRotation();
                    for (float k = 8f; k < length; k += 16f)
                    {
                        drawPos = Projectile.Center + k * direction - Main.screenPosition;
                        float useRotation = rotation;
                        if (Main.rand.Next(2) == 0)
                        {
                            useRotation += MathHelper.Pi;
                        }
                        Main.spriteBatch.Draw(texture, drawPos, null, color, useRotation, origin, 1f, SpriteEffects.None, 0f);
                    }
                }

                {
                    Vector2 direction = Projectile.ai[0].ToRotationVector2();
                    float length = 2400f;
                    float rotation = Projectile.ai[0];
                    for (float k = 8f; k < length; k += 16f)
                    {
                        drawPos = Projectile.Center + k * direction - Main.screenPosition;
                        float useRotation = rotation;
                        if (Main.rand.Next(2) == 0)
                        {
                            useRotation += MathHelper.Pi;
                        }
                        Main.spriteBatch.Draw(texture, drawPos, null, color, useRotation, origin, 1f, SpriteEffects.None, 0f);
                    }
                }
            }

            texture = TextureAssets.Projectile[Projectile.type].Value;
            drawPos = Projectile.Center - Main.screenPosition;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Main.spriteBatch.Draw(texture, drawPos, null, Color.White, Projectile.rotation, origin, 0.5f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, drawPos, null, Color.White, -Projectile.rotation, origin, 0.5f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, drawPos, null, Color.White, -Projectile.rotation, origin, 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, drawPos, null, Color.White, Projectile.rotation, origin, 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}