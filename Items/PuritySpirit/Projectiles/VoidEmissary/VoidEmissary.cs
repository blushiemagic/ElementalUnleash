using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic.Projectiles;

namespace Bluemagic.Items.PuritySpirit.Projectiles.VoidEmissary
{
    public class VoidEmissary : Minion
    {
        private VoidEmissaryHand hand1;
        private VoidEmissaryHand hand2;
        private bool handsOpen = false;
        private const float voidPortalCooldown = 600f;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 32;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.timeLeft = 18000;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            hand1.offset = new Vector2(-16f, 24f);
            hand2.offset = new Vector2(16f, 24f);
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

        public override void CheckActive()
        {
            Player player = Main.player[Projectile.owner];
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            if (player.dead)
            {
                modPlayer.voidEmissary = false;
            }
            if (modPlayer.voidEmissary)
            {
                Projectile.timeLeft = 2;
            }
        }

        public override void Behavior()
        {
            if (Projectile.ai[0] == 0f)
            {
                ChooseAttack();
            }
            Projectile.rotation = 0f;
            if (Projectile.ai[0] == 0f)
            {
                IdleBehavior();
            }
            else if (Projectile.ai[0] == 1f)
            {
                VoidPortalAttack();
            }
            else if (Projectile.ai[0] == 2f)
            {
                LaserAttack();
            }
            else if (Projectile.ai[0] == 3f)
            {
                ChargeAttack();
            }
            else
            {
                Projectile.ai[0] = 0f;
                Projectile.netUpdate = true;
            }
            if (Projectile.localAI[0] > 0f)
            {
                Projectile.localAI[0] -= 1f;
            }
            CreateDust();
            SelectFrame();
        }

        private void ChooseAttack()
        {
            List<NPC> targets = new List<NPC>();
            bool flag = false;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].CanBeChasedBy(Projectile) && NPCInRange(Main.npc[k]))
                {
                    targets.Add(Main.npc[k]);
                    if (Main.npc[k].boss)
                    {
                        flag = true;
                    }
                }
            }
            if (targets.Count == 0)
            {
                return;
            }
            if ((flag || targets.Count > 1) && Projectile.localAI[0] <= 0f)
            {
                Projectile.ai[0] = 1f;
                Projectile.ai[1] = 0f;
                Projectile.netUpdate = true;
                return;
            }
            Player player = Main.player[Projectile.owner];
            bool canHitLine = false;
            float distance = -1f;
            float rotation = 0f;
            foreach (NPC npc in targets)
            {
                bool testCanHitLine = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
                if (testCanHitLine)
                {
                    canHitLine = true;
                }
                if (!canHitLine || testCanHitLine)
                {
                    float testDistance = Vector2.Distance(player.Center, npc.Center);
                    if (distance < 0f || testDistance < distance)
                    {
                        distance = testDistance;
                        rotation = (npc.Center - Projectile.Center).ToRotation();
                    }
                }
            }
            if (distance == 0f)
            {
                rotation = -MathHelper.PiOver2;
            }
            Projectile.ai[0] = canHitLine ? 2f : 3f;
            Projectile.ai[1] = rotation;
            Projectile.localAI[1] = 0f;
            Projectile.netUpdate = true;
        }

        private void IdleBehavior()
        {
            Vector2 target = GetIdleTarget();
            Vector2 offset = target - Projectile.Center;
            if (offset.Length() > 2000f)
            {
                Projectile.Center = target;
            }
            else if (offset.Length() > 16f)
            {
                Vector2 velocityDir = Projectile.velocity;
                Vector2 offsetDir = offset;
                velocityDir.Normalize();
                offsetDir.Normalize();
                float maxSpeed = 8f;
                if (offset.Length() > 160f)
                {
                    maxSpeed *= 2f;
                }
                if (Vector2.Dot(velocityDir, offsetDir) <= 0)
                {
                    Projectile.velocity *= 0.8f;
                }
                if (Projectile.velocity.Length() > maxSpeed)
                {
                    Projectile.velocity *= 0.8f;
                }
                Projectile.velocity *= 0.9f;
                Projectile.velocity += maxSpeed * 0.1f * offsetDir;
                if (offset.X > 0f)
                {
                    Projectile.direction = 1;
                }
                else if (offset.X < 0f)
                {
                    Projectile.direction = -1;
                }
                Projectile.spriteDirection = -Projectile.direction;
            }
            else
            {
                Projectile.velocity *= 0.9f;
            }
            IdleUpdateHand(ref hand1, new Vector2(-16f, 24f));
            IdleUpdateHand(ref hand2, new Vector2(16f, 24f));
            handsOpen = false;
        }

        private Vector2 GetIdleTarget()
        {
            int myNum = 0;
            int num = 0;
            for (int k = 0; k < 1000; k++)
            {
                Projectile proj = Main.projectile[k];
                if (proj.active && proj.owner == Projectile.owner && proj.type == Projectile.type)
                {
                    if (k < Projectile.whoAmI)
                    {
                        myNum++;
                    }
                    num++;
                }
            }
            float rotation = (float)myNum / (float)num * MathHelper.TwoPi;
            rotation -= MathHelper.PiOver2;
            Vector2 offset = 96f * rotation.ToRotationVector2();
            return Main.player[Projectile.owner].Center + offset;
        }

        private void IdleUpdateHand(ref VoidEmissaryHand hand, Vector2 target)
        {
            if (hand.velocity == Vector2.Zero)
            {
                float rotation = MathHelper.TwoPi * (float)Main.rand.NextDouble();
                hand.velocity = 0.5f * rotation.ToRotationVector2();
            }
            if (Vector2.Distance(hand.offset, target) >= 4f)
            {
                float dirToTarget = (target - hand.offset).ToRotation();
                float disturb = MathHelper.Pi * (float)Main.rand.NextDouble() - MathHelper.PiOver2;
                hand.velocity = (dirToTarget + disturb).ToRotationVector2();
                if (Vector2.Distance(hand.offset, target) < 5f)
                {
                    hand.velocity *= 0.5f;
                }
            }
            hand.offset += hand.velocity;
            hand.rotation = 0f;
        }

        private void VoidPortalAttack()
        {
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] <= 60f)
            {
                Vector2 target = Projectile.position + new Vector2(Projectile.width / 2, Projectile.height * 2 / 3);
                for (int k = 0; k < 3; k++)
                {
                    CreateChargeDust(target, 48);
                }
            }
            else
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    for (int k = 0; k < 200; k++)
                    {
                        if (Main.npc[k].CanBeChasedBy(Projectile) && NPCInRange(Main.npc[k]))
                        {
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Main.npc[k].Center.X, Main.npc[k].Center.Y, 0f, 0f, Mod.Find<ModProjectile>("VoidPortal").Type, Projectile.damage * 2, Projectile.knockBack, Projectile.owner);
                        }
                    }
                }
                Projectile.ai[0] = 0f;
                Projectile.localAI[0] = voidPortalCooldown;
                Projectile.netUpdate = true;
            }
            Projectile.velocity *= 0.95f;
            VoidPortalUpdateHand(ref hand1, new Vector2(-16f, 8f));
            VoidPortalUpdateHand(ref hand2, new Vector2(16f, 8f));
            handsOpen = false;
        }

        private void VoidPortalUpdateHand(ref VoidEmissaryHand hand, Vector2 target)
        {
            Vector2 offset = target - hand.offset;
            float distance = offset.Length();
            if (distance != 0f)
            {
                if (distance > 0.5f)
                {
                    offset.Normalize();
                    if (distance <= 4f)
                    {
                        offset *= 0.5f;
                    }
                }
                hand.offset += offset;
            }
            hand.velocity = Vector2.Zero;
            hand.rotation = 0f;
        }

        private void LaserAttack()
        {
            Projectile.velocity *= 0.8f;
            if (Math.Abs(Projectile.ai[1]) > MathHelper.PiOver2)
            {
                Projectile.direction = -1;
            }
            else
            {
                Projectile.direction = 1;
            }
            Projectile.spriteDirection = -Projectile.direction;
            Vector2 direction = Projectile.ai[1].ToRotationVector2();
            if (Projectile.localAI[1] < 15f)
            {
                CreateChargeDust(Projectile.Center + 30f * direction, 16);
            }
            if (Projectile.localAI[1] == 15f && Projectile.owner == Main.myPlayer)
            {
                int laser = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, direction.X, direction.Y, Mod.Find<ModProjectile>("VoidLaser").Type, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, Projectile.whoAmI);
            }
            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] >= 60f)
            {
                Projectile.ai[0] = 0f;
                Projectile.netUpdate = true;
            }
            Vector2 normal = new Vector2(-direction.Y, direction.X);
            hand1.offset = 24 * direction + 16f * normal;
            hand2.offset = 24 * direction - 16f * normal;
            hand1.rotation = Projectile.ai[1];
            hand2.rotation = Projectile.ai[1];
            if (Projectile.spriteDirection == -1)
            {
                hand1.offset.X *= -1f;
                hand2.offset.X *= -1f;
                hand1.rotation = MathHelper.Pi - hand1.rotation;
                hand2.rotation = MathHelper.Pi - hand2.rotation;
            }
            handsOpen = true;
        }

        private void ChargeAttack()
        {
            IdleBehavior();
            Projectile.rotation = Projectile.ai[1] + MathHelper.PiOver2;
            if (Math.Abs(Projectile.ai[1]) > MathHelper.PiOver2)
            {
                Projectile.direction = -1;
            }
            else
            {
                Projectile.direction = 1;
            }
            Projectile.spriteDirection = -Projectile.direction;
            Projectile.velocity = 12f * Projectile.ai[1].ToRotationVector2();
            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] >= 30f)
            {
                Projectile.rotation = 0f;
                Projectile.ai[0] = 0f;
                Projectile.netUpdate = true;
            }
            hand1.offset = new Vector2(-16f, 24f);
            hand2.offset = new Vector2(16f, 24f);
            hand1.rotation = 0f;
            hand2.rotation = 0f;
            handsOpen = false;
        }

        private bool NPCInRange(NPC npc)
        {
            Vector2 playerCenter = Main.player[Projectile.owner].Center;
            Vector2 npcCenter = npc.Center;
            return Math.Abs(playerCenter.X - npcCenter.X) <= 600f && Math.Abs(playerCenter.Y - npcCenter.Y) <= 350f;
        }

        private void CreateChargeDust(Vector2 center, int radius)
        {
            int dust = Dust.NewDust(center - new Vector2(radius, radius), 2 * radius, 2 * radius, Mod.Find<ModDust>("CleanserBeamCharge").Type, 0f, 0f, 70);
            Main.dust[dust].customData = center;
        }

        private void CreateDust()
        {
            if (Main.rand.Next(3) == 0)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height / 3, Mod.Find<ModDust>("PuriumFlame").Type);
                Main.dust[dust].velocity.Y -= 1.2f;
                Main.dust[dust].velocity += Projectile.velocity * 0.5f;
            }
            Lighting.AddLight(Projectile.Center, 0.6f, 0.9f, 0.3f);
        }

        private void SelectFrame()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 8)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 3;
            }
        }

        public override bool MinionContactDamage()
        {
            return Projectile.ai[0] == 3f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void PostDraw(Color lightColor)
        {
            DrawHand(hand1, Main.spriteBatch);
            DrawHand(hand2, Main.spriteBatch);
            if (Projectile.ai[0] == 3f)
            {
                Texture2D texture = ModContent.Request<Texture2D>("Bluemagic/Items/PuritySpirit/Projectiles/VoidEmissary/VoidEmissaryCharge").Value;
                Vector2 position = Projectile.Center - Main.screenPosition;
                float alpha = Math.Abs((Projectile.localAI[1] % 10f) / 5f - 1f);
                alpha = 0.1f + 0.9f * alpha;
                Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
                Main.spriteBatch.Draw(texture, position, null, Color.White * alpha, Projectile.rotation, origin, 1f, SpriteEffects.None, 0f);
            }
        }

        private void DrawHand(VoidEmissaryHand hand, SpriteBatch spriteBatch)
        {
            Texture2D texture = ModContent.Request<Texture2D>("Bluemagic/Items/PuritySpirit/Projectiles/VoidEmissary/VoidEmissaryHand").Value;
            Vector2 position = hand.offset;
            Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Height / 2 - 2);
            if (handsOpen)
            {
                frame.Y += texture.Height / 2;
            }
            float rotation = hand.rotation + Projectile.rotation;
            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
            {
                position.X = -hand.offset.X;
                rotation = MathHelper.Pi - rotation;
            }
            position = position.RotatedBy(Projectile.rotation);
            position += Projectile.Center - Main.screenPosition;
            Vector2 origin = new Vector2(frame.Width / 2, frame.Height / 2);
            Main.spriteBatch.Draw(texture, position, frame, Color.White, rotation, origin, 1f, effects, 0f);
        }
    }

    struct VoidEmissaryHand
    {
        public Vector2 offset;
        public float rotation;
        public Vector2 velocity;
    }
}