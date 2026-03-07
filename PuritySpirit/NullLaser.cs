using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.PuritySpirit
{
    public class NullLaser : ModProjectile
    {
        public float warningTime;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Nullification Laser");
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.hide = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            CooldownSlot = 1;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(warningTime);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            warningTime = reader.ReadSingle();
        }

        public override void AI()
        {
            if (Projectile.velocity.X != 0f)
            {
                Projectile.localAI[0] = Projectile.velocity.X;
                Projectile.velocity.X = 0f;
            }
            if (Projectile.velocity.Y != 0f)
            {
                warningTime = Projectile.velocity.Y;
                Projectile.velocity.Y = 0f;
            }
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            if (!npc.active || npc.type != Mod.Find<ModNPC>("PuritySpirit").Type || Projectile.localAI[0] <= 0f)
            {
                Projectile.Kill();
                return;
            }
            Projectile.ai[1] -= 1f;
            Projectile.localAI[0] -= 1f;
            if (Projectile.localAI[0] < 0f)
            {
                Projectile.Kill();
                return;
            }
            if (Projectile.ai[1] <= warningTime)
            {
                Projectile.hostile = true;
            }
            if (Projectile.ai[1] == 0f)
            {
                SetDirection(npc);
                Projectile.hide = false;
            }
            if (Projectile.ai[1] <= 0)
            {
                CreateDust();
            }
        }

        private void SetDirection(NPC npc)
        {
            IList<int> targets = ((PuritySpirit)npc.ModNPC).targets;
            bool needsRotation = true;
            if (targets.Count > 0)
            {
                int player = targets[0];
                Vector2 offset = Main.player[player].Center - Projectile.Center;
                if (offset != Vector2.Zero)
                {
                    Projectile.rotation = (float)Math.Atan2(offset.Y, offset.X);
                    needsRotation = false;
                }
            }
            if (needsRotation)
            {
                Projectile.rotation = -(float)Math.PI / 2f;
            }
            int numChecks = 3;
            Projectile.localAI[1] = 0f;
            Vector2 direction = new Vector2((float)Math.Cos(Projectile.rotation), (float)Math.Sin(Projectile.rotation));
            for (int k = 0; k < numChecks; k++)
            {
                float side = (float)k / (numChecks - 1f);
                Vector2 sidePos = Projectile.Center + direction.RotatedBy(Math.PI / 2) * (side - 0.5f) * Projectile.width;
                int startX = (int)sidePos.X / 16;
                int startY = (int)sidePos.Y / 16;
                Vector2 endCheck = sidePos + direction * 16f * 150f;
                int endX = (int)endCheck.X / 16;
                int endY = (int)endCheck.Y / 16;
                Tuple<int, int> collide;
                if (!Collision.TupleHitLine(startX, startY, endX, endY, 0, 0, new List<Tuple<int, int>>(), out collide))
                {
                    Projectile.localAI[1] += new Vector2((float)(startX - collide.Item1), (float)(startY - collide.Item2)).Length() * 16f;
                }
                else if (collide.Item1 == endX && collide.Item2 == endY)
                {
                    Projectile.localAI[1] += 1800f;
                }
                else
                {
                    Projectile.localAI[1] += new Vector2((float)(startX - collide.Item1), (float)(startY - collide.Item2)).Length() * 16f;
                }
            }
            Projectile.localAI[1] /= numChecks;
        }

        private void CreateDust()
        {
            Color color = new Color(64, 255, 64);
            Vector2 direction = new Vector2((float)Math.Cos(Projectile.rotation), (float)Math.Sin(Projectile.rotation));
            Vector2 center = Projectile.Center + direction * Projectile.localAI[1];
            for (int k = 0; k < 4; k++)
            {
                float angle = Projectile.rotation + (Main.rand.Next(2) * 2 - 1) * (float)Math.PI / 2f;
                float speed = (float)Main.rand.NextDouble() * 2.6f + 1f;
                Vector2 velocity = speed * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                int dust = Dust.NewDust(center, 0, 0, DustID.RainbowMk2, velocity.X, velocity.Y, 0, color, 1.2f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (Main.rand.Next(3) == 0 || (Main.expertMode && Main.rand.Next(3) == 0))
            {
                target.AddBuff(Mod.Find<ModBuff>("Nullified").Type, Main.rand.Next(240, 300));
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Projectile.ai[1] > -warningTime)
            {
                return false;
            }
            float num = 0f;
            Vector2 end = Projectile.Center + Projectile.localAI[1] * new Vector2((float)Math.Cos(Projectile.rotation), (float)Math.Sin(Projectile.rotation));
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, end, Projectile.width, ref num);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Color color = Color.White * 0.9f;
            Vector2 center = Projectile.Center + 0.5f * Projectile.localAI[1] * new Vector2((float)Math.Cos(Projectile.rotation), (float)Math.Sin(Projectile.rotation)) - Main.screenPosition;
            Vector2 drawCenter = new Vector2(1f, 20f);
            Vector2 scale = new Vector2(Projectile.localAI[1] / 2f, Math.Min(-Projectile.ai[1], warningTime) / warningTime);
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, center, null, color, Projectile.rotation, drawCenter, scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}