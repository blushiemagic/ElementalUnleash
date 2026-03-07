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
    public class DarkLightningProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dark Lightning");
        }

        public override void SetDefaults()
        {
            Projectile.width = 1600;
            Projectile.height = 1600;
            Projectile.friendly = true;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }

        public override void AI()
        {
            Projectile.Center = Main.player[Projectile.owner].Center;
            Projectile.alpha += 10;
            if (Projectile.alpha >= 200)
            {
                Projectile.Kill();
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage.Flat += target.defense / 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> positions = new List<Vector2>();
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].friendly && !Main.npc[k].dontTakeDamage && Projectile.Hitbox.Intersects(Main.npc[k].Hitbox))
                {
                    positions.Add(Main.npc[k].Center);
                }
            }
            while (positions.Count < 9)
            {
                positions.Add(Projectile.Center + new Vector2((Main.rand.NextFloat() - 0.5f) * Projectile.width * 0.5f, (Main.rand.NextFloat() - 0.5f) * Projectile.height * 0.5f));
            }
            Queue<Vector2> process = new Queue<Vector2>();
            const int splitFactor = 2;
            for (int k = 0; k < splitFactor + 1; k++)
            {
                process.Enqueue(Projectile.Center);
            }
            while (positions.Count > 0)
            {
                Vector2 start = process.Dequeue();
                int index = -1;
                for (int k = 0; k < positions.Count; k++)
                {
                    float distance = Vector2.Distance(start, positions[k]);
                    if (index < 0 || distance < Vector2.Distance(start, positions[index]))
                    {
                        index = k;
                    }
                }
                Vector2 end = positions[index];
                DrawArc(Main.spriteBatch, start - Main.screenPosition, end - Main.screenPosition);
                positions.RemoveAt(index);
                for (int k = 0; k < splitFactor; k++)
                {
                    process.Enqueue(end);
                }
            }
            return false;
        }

        private void DrawArc(SpriteBatch spriteBatch, Vector2 start, Vector2 end)
        {
            Vector2 unit = end - start;
            float length = unit.Length();
            unit.Normalize();
            Vector2 normal = new Vector2(-unit.Y, unit.X);
            float k = 0f;
            float lastOffset = 0f;
            while (k < length)
            {
                float next = k + 64f + 256f * Main.rand.NextFloat();
                float offset = 64f * Main.rand.NextFloat() - 32f;
                if (next >= length)
                {
                    offset = 0f;
                    if (next > length + 64f)
                    {
                        next = length + 64f;
                    }
                }
                Vector2 lineStart = start + k * unit + lastOffset * normal;
                Vector2 lineEnd = start + next * unit + offset * normal;
                DrawLine(spriteBatch, lineStart, lineEnd, 1f);
                for (int i = 0; i <= (int)(next - k) / 64; i++)
                {
                    float rotation = MathHelper.TwoPi * Main.rand.NextFloat();
                    float lineLength = 16f + 48f * Main.rand.NextFloat();
                    Vector2 point = Vector2.Lerp(lineStart, lineEnd, Main.rand.NextFloat());
                    Vector2 point2 = point + lineLength * rotation.ToRotationVector2();
                    DrawLine(spriteBatch, point, point2, 0.4f);
                }
                k = next;
                lastOffset = offset;
            }
        }

        private void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, float scale)
        {
            Texture2D texture = TextureAssets.Extra[ExtrasID.CultistLightingArc].Value;
            float drawScale = 0.5f * scale;
            for (int k = 0; k < 3; k++)
            {
                if (k == 0)
                {
                    drawScale = 0.6f * scale;
                    DelegateMethods.c_1 = new Color(204, 115, 219) * 0.5f;
                }
                else if (k == 1)
                {
                    drawScale = 0.4f * scale;
                    DelegateMethods.c_1 = new Color(251, 113, 255) * 0.5f;
                }
                else
                {
                    drawScale = 0.2f * scale;
                    DelegateMethods.c_1 = new Color(255, 255, 255) * 0.5f;
                }
                DelegateMethods.f_1 = 1f;
                Utils.DrawLaser(Main.spriteBatch, texture, start, end, new Vector2(drawScale), new Utils.LaserLineFraming(DelegateMethods.LightningLaserDraw));
            }
        }
    }
}