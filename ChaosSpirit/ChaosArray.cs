using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Bluemagic.ChaosSpirit
{
    public class ChaosArray : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Array of Chaos");
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

        public override void AI()
        {
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            if (!npc.active || npc.type != Mod.Find<ModNPC>("ChaosSpirit").Type)
            {
                Projectile.Kill();
                return;
            }
            Projectile.Center = npc.Center;
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] >= 255f)
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
            if (Projectile.ai[1] >= 90f && Projectile.ai[1] < 240f)
            {
                for (int x = -2400; x <= 2400; x += 160)
                {
                    for (int y = -2400; y <= 2400; y += 160)
                    {
                        if (x == 0 && y == 0)
                        {
                            continue;
                        }
                        Vector2 testPos = Projectile.position + new Vector2(x, y);
                        if (Collision.CheckAABBvAABBCollision(testPos, new Vector2(Projectile.width, Projectile.height), new Vector2(targetHitbox.X, targetHitbox.Y), new Vector2(targetHitbox.Width, targetHitbox.Height)))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawCenter = Projectile.Center - Main.screenPosition;
            float alpha = 1f;
            if (Projectile.ai[1] < 60f)
            {
                alpha = 0.5f - (Math.Abs(Projectile.ai[1] - 30f) / 30f);
            }
            else if (Projectile.ai[1] < 90f)
            {
                alpha = (Projectile.ai[1] - 60f) / 30f;
            }
            else if (Projectile.ai[1] > 240f)
            {
                alpha = (255f - Projectile.ai[1]) / 15f;
            }
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            for (int x = -2400; x <= 2400; x += 160)
            {
                for (int y = -2400; y <= 2400; y += 160)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }
                    int hash = (7 * x + 13 * y) / 200;
                    float hue = (hash % 24) / 24f;
                    Color color = Main.hslToRgb(hue, 1f, 0.5f) * alpha;
                    Main.spriteBatch.Draw(texture, drawCenter + new Vector2(x, y), null, color, 0f, origin, 1f, SpriteEffects.None, 0f);
                }
            }
            return false;
        }
    }
}