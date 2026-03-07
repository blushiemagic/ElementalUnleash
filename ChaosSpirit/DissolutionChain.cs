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
    public class DissolutionChain : ModProjectile
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

        private int timer = 0;
        private Color color;

        private float Length
        {
            get
            {
                int interval = Main.expertMode ? 40 : 50;
                if (timer <= interval)
                {
                    return 0f;
                }
                float length = 32f * (timer - interval);
                if (length > 2400f)
                {
                    length = 2400f;
                }
                return length;
            }
        }

        private Vector2 Source
        {
            get
            {
                return new Vector2(Projectile.ai[0], Projectile.ai[1]);
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
        }

        public override void AI()
        {
            if (Projectile.velocity.X != 0f)
            {
                Projectile.localAI[0] = Projectile.velocity.X == -1f ? 0f : Projectile.velocity.X;
                Projectile.velocity.X = 0f;
            }
            if (Projectile.velocity.Y != 0f)
            {
                Projectile.localAI[1] = Projectile.velocity.Y;
                Projectile.velocity.Y = 0f;
            }
            if (timer == 0)
            {
                color = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f);
            }
            if (Projectile.localAI[1] > 0f)
            {
                timer++;
                int interval = Main.expertMode ? 40 : 50;
                if (timer == interval && Projectile.localAI[1] > interval + 10f && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Player player = Main.player[(int)Projectile.localAI[0]];
                    int damage = 150;
                    if (Main.expertMode)
                    {
                        damage = (int)(damage * 1.5f / 2f);
                    }
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, Vector2.Zero, Projectile.type, damage, 0f, Main.myPlayer, Projectile.Center.X, Projectile.Center.Y);
                    Main.projectile[proj].localAI[0] = Projectile.localAI[0];
                    Main.projectile[proj].localAI[1] = Projectile.localAI[1];
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
                }
                if (timer == interval)
                {
                    SoundEngine.PlaySound(SoundID.Item81, Projectile.position);
                }
            }
            Projectile.localAI[1] -= 1f;
            if (Projectile.localAI[1] <= -15f)
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
            if (Projectile.localAI[1] <= 0f)
            {
                return false;
            }
            float length = Length;
            if (length > 0f)
            {
                float num = 0f;
                Vector2 center = Source;
                Vector2 direction = Projectile.Center - center;
                if (direction == Vector2.Zero)
                {
                    direction = new Vector2(0f, 1f);
                }
                direction.Normalize();
                direction *= length;
                Vector2 start = center + direction;
                Vector2 end = center - direction;
                if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 16, ref num))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawCenter = Source - Main.screenPosition;
            Vector2 direction = Projectile.Center - Source;
            if (direction == Vector2.Zero)
            {
                direction = new Vector2(0f, 1f);
            }
            direction.Normalize();
            float alpha = 1f;
            if (Projectile.localAI[1] < 0f)
            {
                alpha = 1f + Projectile.localAI[1] / 15f;
            }
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            float length = Length;
            for (float k = 0f; k < length; k += Projectile.width / 2)
            {
                Main.spriteBatch.Draw(texture, drawCenter + k * direction, null, color * alpha, 0f, origin, 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(texture, drawCenter - k * direction, null, color * alpha, 0f, origin, 1f, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, ChaosSpirit.mainColor * alpha, 0f, origin, 2f, SpriteEffects.None, 0f);
            return false;
        }
    }
}