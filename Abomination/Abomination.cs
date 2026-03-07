using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Abomination
{
    [AutoloadBossHead]
    public class Abomination : ModNPC
    {
        private static int hellLayer
        {
            get
            {
                return Main.maxTilesY - 200;
            }
        }

        private int difficulty
        {
            get
            {
                int result = 0;
                if (Main.expertMode)
                {
                    result++;
                }
                if (NPC.downedMoonlord)
                {
                    result++;
                }
                return result;
            }
        }

        private const int sphereRadius = 300;

        private float attackCool
        {
            get
            {
                return NPC.ai[0];
            }
            set
            {
                NPC.ai[0] = value;
            }
        }

        private float moveCool
        {
            get
            {
                return NPC.ai[1];
            }
            set
            {
                NPC.ai[1] = value;
            }
        }

        private float rotationSpeed
        {
            get
            {
                return NPC.ai[2];
            }
            set
            {
                NPC.ai[2] = value;
            }
        }

        private float captiveRotation
        {
            get
            {
                return NPC.ai[3];
            }
            set
            {
                NPC.ai[3] = value;
            }
        }

        private int moveTime = 300;
        private int moveTimer = 60;
        internal int laserTimer = 0;
        internal int laser1 = -1;
        internal int laser2 = -1;
        private bool dontDamage = false;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The Abomination");
            Main.npcFrameCount[NPC.type] = 2;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 40000;
            NPC.damage = 100;
            NPC.defense = 55;
            if (NPC.downedMoonlord)
            {
                NPC.lifeMax = 80000;
                NPC.damage = 120;
                NPC.defense = 80;
            }
            NPC.knockBackResist = 0f;
            NPC.width = 100;
            NPC.height = 100;
            NPC.value = Item.buyPrice(0, 20, 0, 0);
            NPC.npcSlots = 15f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.buffImmune[24] = true;
            Music = MusicID.Boss2;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        { 
            NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * balance);
            NPC.damage = (int)(NPC.damage * 0.75f);
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient && NPC.localAI[0] == 0f)
            {
                for (int k = 0; k < 5; k++)
                {
                    int captive = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, Mod.Find<ModNPC>("CaptiveElement").Type);
                    Main.npc[captive].ai[0] = NPC.whoAmI;
                    Main.npc[captive].ai[1] = k;
                    Main.npc[captive].ai[2] = 50 * (k + 1);
                    CaptiveElement.SetPosition(Main.npc[captive]);
                    Main.npc[captive].netUpdate = true;
                }
                NPC.netUpdate = true;
                NPC.localAI[0] = 1f;
            }
            Player player = Main.player[NPC.target];
            if (!player.active || player.dead || player.position.Y < hellLayer * 16)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead || player.position.Y < hellLayer * 16)
                {
                    NPC.velocity = new Vector2(0f, 10f);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                    return;
                }
            }
            moveCool -= 1f;
            if (Main.netMode != NetmodeID.MultiplayerClient && moveCool <= 0f)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                double angle = Main.rand.NextDouble() * 2.0 * Math.PI;
                int distance = sphereRadius + Main.rand.Next(200);
                Vector2 moveTo = player.Center + (float)distance * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                moveCool = (float)moveTime + (float)Main.rand.Next(100);
                NPC.velocity = (moveTo - NPC.Center) / moveCool;
                rotationSpeed = (float)(Main.rand.NextDouble() + Main.rand.NextDouble());
                if (rotationSpeed > 1f)
                {
                    rotationSpeed = 1f + (rotationSpeed - 1f) / 2f;
                }
                if (Main.rand.Next(2) == 0)
                {
                    rotationSpeed *= -1;
                }
                rotationSpeed *= 0.01f;
                NPC.netUpdate = true;
            }
            if (Vector2.Distance(Main.player[NPC.target].position, NPC.position) > sphereRadius)
            {
                moveTimer--;
            }
            else
            {
                moveTimer += 3;
                if (moveTime >= 300 && moveTimer > 60)
                {
                    moveTimer = 60;
                }
            }
            if (moveTimer <= 0)
            {
                moveTimer += 60;
                moveTime -= 3;
                if (moveTime < 99)
                {
                    moveTime = 99;
                    moveTimer = 0;
                }
                NPC.netUpdate = true;
            }
            else if (moveTimer > 60)
            {
                moveTimer -= 60;
                moveTime += 3;
                NPC.netUpdate = true;
            }
            captiveRotation += rotationSpeed;
            if (captiveRotation < 0f)
            {
                captiveRotation += 2f * (float)Math.PI;
            }
            if (captiveRotation >= 2f * (float)Math.PI)
            {
                captiveRotation -= 2f * (float)Math.PI;
            }
            attackCool -= 1f;
            if (Main.netMode != NetmodeID.MultiplayerClient && difficulty > 1 && attackCool > 0f && attackCool <= 60f && (int)Math.Ceiling(attackCool) % 30 == 0)
            {
                Shoot(player);
            }
            if (Main.netMode != NetmodeID.MultiplayerClient && attackCool <= 0)
            {
                attackCool = 200f + 200f * (float)NPC.life / (float)NPC.lifeMax + (float)Main.rand.Next(200);
                Shoot(player);
                NPC.netUpdate = true;
            }
            if (difficulty > 0)
            {
                ExpertLaser();
            }
            if (Main.rand.Next(2) == 0)
            {
                float radius = (float)Math.Sqrt(Main.rand.Next(sphereRadius * sphereRadius));
                double angle = Main.rand.NextDouble() * 2.0 * Math.PI;
                Dust.NewDust(new Vector2(NPC.Center.X + radius * (float)Math.Cos(angle), NPC.Center.Y + radius * (float)Math.Sin(angle)), 0, 0, Mod.Find<ModDust>("Sparkle").Type, 0f, 0f, 0, default(Color), 1.5f);
            }
        }

        private void Shoot(Player player)
        {
            Vector2 delta = player.Center - NPC.Center;
            float magnitude = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
            if (magnitude > 0)
            {
                delta *= 5f / magnitude;
            }
            else
            {
                delta = new Vector2(0f, 5f);
            }
            int damage = (NPC.damage - 30) / 2;
            if (Main.expertMode)
            {
                damage = (int)(damage / Main.GameModeInfo.EnemyDamageMultiplier);
            }
            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, delta.X, delta.Y, Mod.Find<ModProjectile>("ElementBall").Type, damage, 3f, Main.myPlayer, BuffID.OnFire, 600f);
        }

        private void ExpertLaser()
        {
            laserTimer--;
            if (laserTimer <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (NPC.localAI[0] == 2f)
                {
                    int laser1Index;
                    int laser2Index;
                    if (laser1 < 0)
                    {
                        laser1Index = NPC.whoAmI;
                    }
                    else
                    {
                        for (laser1Index = 0; laser1Index < 200; laser1Index++)
                        {
                            if (Main.npc[laser1Index].type == Mod.Find<ModNPC>("CaptiveElement").Type && laser1 == Main.npc[laser1Index].ai[1])
                            {
                                break;
                            }
                        }
                    }
                    if (laser2 < 0)
                    {
                        laser2Index = NPC.whoAmI;
                    }
                    else
                    {
                        for (laser2Index = 0; laser2Index < 200; laser2Index++)
                        {
                            if (Main.npc[laser2Index].type == Mod.Find<ModNPC>("CaptiveElement").Type && laser2 == Main.npc[laser2Index].ai[1])
                            {
                                break;
                            }
                        }
                    }
                    Vector2 pos = Main.npc[laser1Index].Center;
                    int damage = Main.npc[laser1Index].damage / 2;
                    if (Main.expertMode)
                    {
                        damage = (int)(damage / Main.GameModeInfo.EnemyDamageMultiplier);
                    }
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), pos.X, pos.Y, 0f, 0f, Mod.Find<ModProjectile>("ElementLaser").Type, damage, 0f, Main.myPlayer, laser1Index, laser2Index);
                }
                else
                {
                    NPC.localAI[0] = 2f;
                }
                laserTimer = 500 + Main.rand.Next(100);
                laserTimer = 60 + laserTimer * NPC.life / NPC.lifeMax;
                laser1 = Main.rand.Next(6) - 1;
                laser2 = Main.rand.Next(5) - 1;
                if (laser2 >= laser1)
                {
                    laser2++;
                }
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((short)moveTime);
            writer.Write((short)moveTimer);
            if (Main.expertMode)
            {
                writer.Write((short)laserTimer);
                writer.Write((byte)(laser1 + 1));
                writer.Write((byte)(laser2 + 1));
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            moveTime = reader.ReadInt16();
            moveTimer = reader.ReadInt16();
            if (Main.expertMode)
            {
                laserTimer = reader.ReadInt16();
                laser1 = reader.ReadByte() - 1;
                laser2 = reader.ReadByte() - 1;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (attackCool < 50f)
            {
                NPC.frame.Y = frameHeight;
            }
            else if (difficulty > 1 && attackCool < 110f)
            {
                NPC.frame.Y = frameHeight;
            }
            else
            {
                NPC.frame.Y = 0;
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < hit.Damage / NPC.lifeMax * 100.0; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default(Color), 1f);
            }
            if (Main.netMode != NetmodeID.MultiplayerClient && NPC.life <= 0)
            {
                Vector2 spawnAt = NPC.Center + new Vector2(0f, (float)NPC.height / 2f);
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)spawnAt.X, (int)spawnAt.Y, Mod.Find<ModNPC>("AbominationRun").Type);
            }
        }

        public override bool PreKill()
        {
            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.expertMode || Main.rand.Next(2) == 0)
            {
                target.AddBuff(BuffID.OnFire, 600, true);
            }
        }

        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            dontDamage = (player.Center - NPC.Center).Length() > sphereRadius;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[projectile.owner];
            dontDamage = player.active && (player.Center - NPC.Center).Length() > sphereRadius;
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            if (dontDamage)
            {
                modifiers.SourceDamage *= 0;
                modifiers.DisableCrit();
                dontDamage = false;
                SoundEngine.PlaySound(NPC.HitSound, NPC.position);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Bluemagic/Abomination/HolySphere").Value, NPC.Center - Main.screenPosition, null, Color.White * (70f / 255f), 0f, new Vector2(sphereRadius, sphereRadius), 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Bluemagic/Abomination/HolySphereBorder").Value, NPC.Center - Main.screenPosition, null, Color.White * 0.5f, 0f, new Vector2(sphereRadius, sphereRadius), 1f, SpriteEffects.None, 0f);
            if (difficulty > 0 && laserTimer <= 60 && (laser1 == -1 || laser2 == -1))
            {
                float rotation = laserTimer / 30f;
                if (laser1 == -1)
                {
                    rotation *= -1f;
                }
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Bluemagic/Abomination/Rune").Value, NPC.Center - Main.screenPosition, null, new Color(255, 10, 0), rotation, new Vector2(64, 64), 1f, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}