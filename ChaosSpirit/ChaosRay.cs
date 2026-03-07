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
    public class ChaosRay : ModProjectile
    {
        private const float length = 2400f;

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            CooldownSlot = 1;
        }

        private bool didHit = false;

        private float TrueRotation
        {
            get
            {
                float t = Projectile.localAI[1] / 240f;
                return (1f - t) * Projectile.ai[1] + t * Projectile.localAI[0];
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
        }

        public override void AI()
        {
            if (Projectile.knockBack != 0f)
            {
                Projectile.localAI[0] = Projectile.knockBack;
                Projectile.knockBack = 0f;
            }
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            if (!npc.active || npc.type != Mod.Find<ModNPC>("ChaosSpiritArm").Type || Projectile.localAI[0] > 2f * MathHelper.TwoPi || Projectile.localAI[0] < -2f * MathHelper.TwoPi)
            {
                Projectile.Kill();
                return;
            }
            Projectile.Center = npc.Center;
            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] > 240f)
            {
                Projectile.Kill();
                return;
            }
            NPC spirit = Main.npc[(int)npc.ai[0]];
            if (Main.netMode != NetmodeID.MultiplayerClient && !didHit && spirit.active && spirit.ModNPC is ChaosSpirit2 && Colliding(Projectile.Hitbox, spirit.Hitbox).Value)
            {
                ((ChaosSpirit2)spirit.ModNPC).Damage();
                didHit = true;
            }
            CreateDust();
        }

        private Color GetColor()
        {
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            return ChaosSpiritArm.GetColor((int)npc.ai[1]);
        }

        private void CreateDust()
        {
            Color color = GetColor();
            Vector2 direction = TrueRotation.ToRotationVector2();
            Vector2 center = Projectile.Center + direction * length;
            for (int k = 0; k < 4; k++)
            {
                float angle = TrueRotation + (Main.rand.Next(2) * 2 - 1) * (float)Math.PI / 2f;
                float speed = (float)Main.rand.NextDouble() * 2.6f + 1f;
                Vector2 velocity = speed * angle.ToRotationVector2();
                int dust = Dust.NewDust(center, 0, 0, DustID.RainbowMk2, velocity.X, velocity.Y, 0, color, 1.2f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (target.hurtCooldowns[1] <= 0)
            {
                BluemagicPlayer modPlayer = target.GetModPlayer<BluemagicPlayer>();
                modPlayer.constantDamage = 360;
                modPlayer.percentDamage = 0.6f;
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
            float num = 0f;
            Vector2 end = Projectile.Center + length * TrueRotation.ToRotationVector2();
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, end, Projectile.width, ref num);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Color color = GetColor();
            float trueRotation = TrueRotation;
            Vector2 direction = trueRotation.ToRotationVector2();
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            for (float k = Projectile.width * 1.5f; k < length; k += Projectile.width)
            {
                Vector2 drawPos = Projectile.Center + k * direction - Main.screenPosition;
                Main.spriteBatch.Draw(texture, drawPos, null, color, trueRotation, origin, 1f, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}