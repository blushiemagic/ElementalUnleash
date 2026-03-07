using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Bluemagic.ChaosSpirit
{
    public class ChaosSpirit : ModNPC
    {
        internal const int size = 120;
        public static readonly Color mainColor = new Color(204, 166, 51);
        public const float warningRadius = 1600f;
        public const float killRadius = 2400f;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Spirit of Chaos");
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 400000;
            NPC.damage = 200;
            NPC.defense = 120;
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = false;
            NPC.width = size;
            NPC.height = size;
            NPC.value = Item.buyPrice(1, 0, 0, 0);
            NPC.npcSlots = 100f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = null;
            NPC.alpha = 255;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            Music = MusicID.Boss5;
            //bossBag = mod.ItemType("ChaosSpiritBag");
        }

        private List<ChaosOrb> orbs = new List<ChaosOrb>();
        private int damageTotal = 0;
        private bool saidRushMessage = false;
        internal List<int> targets = new List<int>();
        private bool canMove = true;

        internal static int dpsCap
        {
            get
            {
                return BluemagicWorld.downedChaosSpirit ? 20000 : 10000;
            }
        }

        private int stage
        {
            get
            {
                return (int)NPC.ai[0];
            }
            set
            {
                NPC.ai[0] = value;
            }
        }

        private int attack
        {
            get
            {
                return (int)NPC.ai[1];
            }
            set
            {
                NPC.ai[1] = value;
            }
        }

        private int attackProgress
        {
            get
            {
                return (int)NPC.ai[2];
            }
            set
            {
                NPC.ai[2] = value;
            }
        }

        private int attackCooldown
        {
            get
            {
                return (int)NPC.ai[3];
            }
            set
            {
                NPC.ai[3] = value;
            }
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax / Main.GameModeInfo.EnemyMaxLifeMultiplier * 1.2f * balance);
            NPC.damage = (int)(NPC.damage * 0.75f);
            NPC.defense = 140;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 2f;
            return null;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.localAI[0]);
            writer.Write(NPC.localAI[1]);
            writer.Write(NPC.localAI[2]);
            writer.Write(NPC.localAI[3]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.localAI[0] = reader.ReadSingle();
            NPC.localAI[1] = reader.ReadSingle();
            NPC.localAI[2] = reader.ReadSingle();
            NPC.localAI[3] = reader.ReadSingle();
        }

        public override void AI()
        {
            Bluemagic.freezeHeroLives = true;
            if (!Main.dedServ)
            {
                UpdateChaosOrbs();
            }
            FindPlayers();
            int debuffStrength = 1;
            if (NPC.life <= NPC.lifeMax * 0.75f)
            {
                debuffStrength = 2;
            }
            if (NPC.life <= NPC.lifeMax * 0.5f)
            {
                debuffStrength = 3;
            }
            if (NPC.life <= NPC.lifeMax * 0.25f)
            {
                debuffStrength = 4;
            }
            int debuffType = Mod.Find<ModBuff>("ChaosPressure" + debuffStrength).Type;
            foreach (int target in targets)
            {
                Main.player[target].AddBuff(debuffType, 2, false);
            }
            NPC.timeLeft = NPC.activeTime;
            if (stage > 0 && targets.Count == 0)
            {
                attackProgress = 0;
                stage = -1;
                NPC.netUpdate = true;
            }
            damageTotal -= dpsCap;
            if (damageTotal < 0)
            {
                damageTotal = 0;
            }
            NPC.TargetClosest();
            NPC.rotation = (Main.player[NPC.target].Center - NPC.Center).ToRotation();
            if (stage == 1)
            {
                SetAttack(10);
                stage++;
            }
            else if (stage == 2 && attack == 0 && attackCooldown == 0 && NPC.life <= NPC.lifeMax * 0.75f)
            {
                SetAttack(10);
                stage++;
            }
            else if (stage == 3 && attack == 0 && attackCooldown == 0 && NPC.life <= NPC.lifeMax * 0.5f)
            {
                SetAttack(11);
                stage++;
            }
            else if (stage == 4 && attack == 0 && attackCooldown == 0 && NPC.life <= NPC.lifeMax * 0.25f)
            {
                SetAttack(10);
                stage++;
            }
            canMove = true;
            switch (stage)
            {
                case -1:
                    RunAway();
                    break;
                case 0:
                    Initialize();
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                    DoAttack();
                    break;
            }
            if (canMove)
            {
                Move();
            }
            if (NPC.life <= 1 && stage == 5 && attack == 0)
            {
                Transform();
            }
        }

        private void UpdateChaosOrbs()
        {
            foreach (ChaosOrb orb in orbs)
            {
                orb.Update();
            }
            float x = size * Main.rand.NextFloat();
            float y = size * Main.rand.NextFloat();
            float xSpeed = Main.rand.NextFloat() * 2f - 1f;
            float ySpeed = -1f;
            for (int k = 0; k < 5; k++)
            {
                orbs.Add(new ChaosOrb(new Vector2(x, y), new Vector2(xSpeed, ySpeed), RandomOrbColor()));
            }
            while (orbs[0].strength <= 0f)
            {
                orbs.RemoveAt(0);
            }
        }

        public static Color RandomOrbColor()
        {
            if (Main.rand.Next(3) != 0)
            {
                Color color = mainColor;
                color.R += (byte)Main.rand.Next(-5, 6);
                color.G += (byte)Main.rand.Next(-4, 5);
                color.B += (byte)Main.rand.Next(-3, 4);
                return color;
            }
            int choice = Main.rand.Next(6);
            switch (choice)
            {
            case 0:
            default:
                return new Color(255, 0, 0);
            case 1:
                return new Color(255, 255, 0);
            case 2:
                return new Color(0, 255, 0);
            case 3:
                return new Color(0, 255, 255);
            case 4:
                return new Color(0, 0, 255);
            case 5:
                return new Color(255, 0, 255);
            }
        }

        public void FindPlayers()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int originalCount = targets.Count;
                targets.Clear();
                for (int k = 0; k < 255; k++)
                {
                    if (Main.player[k].active && Main.player[k].GetModPlayer<BluemagicPlayer>().heroLives > 0)
                    {
                        targets.Add(k);
                    }
                }
                if (Main.netMode == NetmodeID.Server && targets.Count != originalCount)
                {
                    ModPacket netMessage = GetPacket(ChaosSpiritMessageType.TargetList);
                    netMessage.Write(targets.Count);
                    foreach (int target in targets)
                    {
                        netMessage.Write(target);
                    }
                    netMessage.Send();
                }
            }
        }

        private void SetAttack(int newAttack)
        {
            attack = newAttack;
            attackProgress = 0;
            attackCooldown = 60;
            NPC.netUpdate = true;
        }

        public void RunAway()
        {
            Bluemagic.freezeHeroLives = false;
            attackProgress++;
            if (attackProgress >= 360)
            {
                NPC.active = false;
            }
        }

        private void Initialize()
        {
            canMove = false;
            if (Main.netMode != NetmodeID.MultiplayerClient && attackProgress == 1)
            {
                Vector2 center = NPC.Center;
                for (int k = 0; k < 255; k++)
                {
                    Player player = Main.player[k];
                    if (player.active && Vector2.Distance(player.Center, NPC.Center) <= warningRadius)
                    {
                        player.GetModPlayer<BluemagicPlayer>().heroLives = 3;
                        if (Main.netMode == NetmodeID.Server)
                        {
                            ModPacket netMessage = GetPacket(ChaosSpiritMessageType.HeroPlayer);
                            netMessage.Write(3);
                            netMessage.Send(k);
                        }
                    }
                }
            }
            attackProgress++;
            if (attackProgress >= 120)
            {
                attackProgress = 0;
                stage++;
            }
        }

        private int RandomTarget()
        {
            if (targets.Count == 0)
            {
                return 255;
            }
            return targets[Main.rand.Next(targets.Count)];
        }

        private void DoAttack()
        {
            if (attack == 0 && attackCooldown <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                SetAttack(1 + Main.rand.Next(4));
            }
            if (attack == 10)
            {
                UltimateAttack1();
            }
            else if (attack == 11)
            {
                UltimateAttack2();
            }
            else if (attack == 1)
            {
                LaserOrbAttack();
            }
            else if (attack == 2)
            {
                CrossLaserAttack();
            }
            else if (attack == 3)
            {
                ChainAttack();
            }
            else if (attack == 4)
            {
                GridAttack();
            }
            if (attack == 0 && attackCooldown > 0)
            {
                attackCooldown--;
            }
        }

        private void UltimateAttack1()
        {
            canMove = false;
            if (Main.netMode != NetmodeID.MultiplayerClient && attackProgress == 0)
            {
                int target = RandomTarget();
                NPC.localAI[0] = target;
                Vector2 targetPos = Main.player[target].Center;
                NPC.localAI[1] = targetPos.X;
                NPC.localAI[2] = targetPos.Y;
                NPC.netUpdate = true;
            }
            if (Main.netMode == NetmodeID.Server && attackProgress == 10)
            {
                NPC.netUpdate = true;
            }
            if (attackProgress % 20 == 0)
            {
                PlaySound(SoundID.Item15);
            }
            if (attackProgress == 120)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 targetPos = new Vector2(NPC.localAI[1], NPC.localAI[2]);
                    Vector2 offset = targetPos - NPC.Center;
                    float rotation = MathHelper.PiOver2;
                    if (offset != Vector2.Zero)
                    {
                        rotation = offset.ToRotation();
                    }
                    targetPos = Main.player[(int)NPC.localAI[0]].Center;
                    offset = targetPos - NPC.Center;
                    float newRotation = MathHelper.PiOver2;
                    if (offset != Vector2.Zero)
                    {
                        newRotation = offset.ToRotation();
                    }
                    if (newRotation < rotation - MathHelper.Pi)
                    {
                        newRotation += MathHelper.TwoPi;
                    }
                    else if (newRotation > rotation + MathHelper.Pi)
                    {
                        newRotation -= MathHelper.TwoPi;
                    }
                    int damage = 900;
                    if (Main.expertMode)
                    {
                        damage = (int)(damage * 1.5f / 2f);
                    }
                    float rotSpeed = newRotation > rotation ? 0.001f : -0.001f;
                    int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, Mod.Find<ModProjectile>("CataclysmicRay").Type, damage, rotSpeed, Main.myPlayer, NPC.whoAmI, rotation);
                    NPC.localAI[3] = proj;
                    NPC.netUpdate = true;
                }
                PlaySound(SoundID.Zombie104);
            }
            attackProgress++;
            if (attackProgress > 120)
            {
                Projectile laser = Main.projectile[(int)NPC.localAI[3]];
                if (laser.active && laser.type == Mod.Find<ModProjectile>("CataclysmicRay").Type)
                {
                    NPC.rotation = laser.ai[1] + laser.localAI[0];
                }
                else
                {
                    attack = 0;
                    attackProgress = 0;
                }
            }
            else
            {
                Vector2 offset = new Vector2(NPC.localAI[1], NPC.localAI[2]) - NPC.Center;
                NPC.rotation = MathHelper.PiOver2;
                if (offset != Vector2.Zero)
                {
                    NPC.rotation = offset.ToRotation();
                }
            }
        }

        private void UltimateAttack2()
        {
            canMove = false;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (attackProgress == 0)
                {
                    Talk("Mods.Bluemagic.ChaosPressureStart");
                    float angle = Main.rand.NextFloat() * MathHelper.TwoPi;
                    Vector2 offset = 320f * angle.ToRotationVector2();
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + offset, Vector2.Zero, Mod.Find<ModProjectile>("HolySphere").Type, 0, 0f, Main.myPlayer, NPC.whoAmI);
                    
                }
                if (attackProgress == 30)
                {
                    Talk("Mods.Bluemagic.ChaosPressureLight");
                }
            }
            attackProgress++;
            if (attackProgress == 260f)
            {
                PlaySound(SoundID.Zombie104);
            }
            if (attackProgress >= 300f)
            {
                attack = 0;
                attackProgress = 0;
            }
        }

        private void LaserOrbAttack()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient && attackProgress == 0)
            {
                int target = RandomTarget();
                Player player = Main.player[target];
                Vector2 targetPos = player.Center;
                float rotation = MathHelper.TwoPi * Main.rand.NextFloat();
                int damage = 200;
                if (Main.expertMode)
                {
                    damage = (int)(damage * 1.5f / 2f);
                }
                int numOrbs = Main.expertMode ? 5 : 3;
                int[] laserOrbs = new int[numOrbs];
                float ai0 = target;
                if (Main.rand.Next(2) == 0)
                {
                    target *= -1;
                    target -= 1;
                }
                for (int k = 0; k < numOrbs; k++)
                {
                    Vector2 offset = 320f * (rotation + MathHelper.TwoPi * k / (float)numOrbs).ToRotationVector2();
                    laserOrbs[k] = Projectile.NewProjectile(NPC.GetSource_FromThis(), targetPos + offset, Vector2.Zero, Mod.Find<ModProjectile>("DissonanceOrb").Type, damage, 0f, Main.myPlayer, target);
                }
                for (int k = 0; k < numOrbs; k++)
                {
                    Main.projectile[laserOrbs[k]].ai[1] = Main.projectile[laserOrbs[(k + 1) % numOrbs]].projUUID;
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, laserOrbs[k]);
                }
            }
            if (attackProgress % 20 == 0 && attackProgress < 120)
            {
                PlaySound(SoundID.Item15);
            }
            else if (attackProgress % 40 == 0 && attackProgress >= 120)
            {
                PlaySound(SoundID.Item13);
            }
            attackProgress++;
            if (attackProgress >= 360)
            {
                attack = 0;
                attackProgress = 0;
            }
        }

        private void CrossLaserAttack()
        {
            int interval = Main.expertMode ? 30 : 40;
            if (attackProgress % interval == 0 && attackProgress < 240 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Player player = Main.player[RandomTarget()];
                float rotation = MathHelper.TwoPi * Main.rand.NextFloat();
                if (player.velocity.Length() >= 1.5f)
                {
                    rotation = player.velocity.ToRotation();
                }
                int damage = 200;
                if (Main.expertMode)
                {
                    damage = (int)(damage * 1.5f / 2f);
                }
                Projectile.NewProjectile(NPC.GetSource_FromThis(), player.Center, Vector2.Zero, Mod.Find<ModProjectile>("CrossFracture").Type, damage, 0f, Main.myPlayer, rotation);
            }
            attackProgress++;
            if (attackProgress >= 300)
            {
                attack = 0;
                attackProgress = 0;
            }
        }

        private void ChainAttack()
        {
            canMove = false;
            if (attackProgress == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                int target = RandomTarget();
                float syncTarget = target;
                if (syncTarget == 0f)
                {
                    syncTarget = -1f;
                }
                int damage = 200;
                if (Main.expertMode)
                {
                    damage = (int)(damage * 1.5f / 2f);
                }
                Projectile.NewProjectile(NPC.GetSource_FromThis(), Main.player[target].Center, new Vector2(syncTarget, 300f), Mod.Find<ModProjectile>("DissolutionChain").Type, damage, 0f, Main.myPlayer, NPC.Center.X, NPC.Center.Y);
            }
            attackProgress++;
            if (attackProgress >= 300)
            {
                attack = 0;
                attackProgress = 0;
            }
        }

        private void GridAttack()
        {
            if (attackProgress == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                int damage = 200;
                if (Main.expertMode)
                {
                    damage = (int)(damage * 1.5f / 2f);
                }
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, Mod.Find<ModProjectile>("ChaosArray").Type, damage, 0f, Main.myPlayer, NPC.whoAmI);
            }
            attackProgress++;
            if (attackProgress >= 240)
            {
                attack = 0;
                attackProgress = 0;
            }
        }

        private void Move()
        {
            Vector2 closest = NPC.Center;
            float distance = -1f;
            foreach (int playerIndex in targets)
            {
                Player player = Main.player[playerIndex];
                float playerDistance = Vector2.Distance(player.Center, NPC.Center);
                if (distance < 0f || playerDistance < distance)
                {
                    closest = player.Center;
                    distance = playerDistance;
                }
            }
            Vector2 offset = closest - NPC.Center;
            NPC.position += 0.01f * offset;
        }

        public override bool CheckDead()
        {
            NPC.active = true;
            NPC.life = 1;
            NPC.dontTakeDamage = true;
            return false;
        }

        private void Transform()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Bottom.X, (int)NPC.Bottom.Y, Mod.Find<ModNPC>("ChaosSpirit2").Type);
                NPC.active = false;
                if (Main.netMode == NetmodeID.Server)
                {
                    ModPacket packet = GetPacket(ChaosSpiritMessageType.DeActivate);
                    packet.Send();
                }
            }
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            BluemagicPlayer modPlayer = target.GetModPlayer<BluemagicPlayer>();
            modPlayer.constantDamage = NPC.damage;
            modPlayer.percentDamage = 1f / 3f;
            if (Main.expertMode)
            {
                modPlayer.percentDamage *= 1.5f;
            }
            modPlayer.chaosDefense = true;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(Mod.Find<ModBuff>("Undead").Type, 300, false);
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return CanBeHitByPlayer(player);
        }

        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            ModifyHit(ref modifiers);
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            OnHit(damageDone);
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return CanBeHitByPlayer(Main.player[projectile.owner]);
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            ModifyHit(ref modifiers);
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            OnHit(damageDone);
        }

        private bool? CanBeHitByPlayer(Player player)
        {
            if (!targets.Contains(player.whoAmI))
            {
                return false;
            }
            return null;
        }

        private void ModifyHit(ref NPC.HitModifiers damage)
        {
            damage.SetMaxDamage(NPC.lifeMax / 8);
        }

        private void OnHit(int damage)
        {
            damageTotal += damage * 60;
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                ModPacket netMessage = GetPacket(ChaosSpiritMessageType.Damage);
                netMessage.Write(damage * 60);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    netMessage.Write(Main.myPlayer);
                }
                netMessage.Send();
            }
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            if (damageTotal >= dpsCap * 60)
            {
                if (!saidRushMessage && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Talk("Mods.Bluemagic.ChaosDpsCap");
                    saidRushMessage = true;
                }
                modifiers.SourceDamage *= 0;
            }
        }

        public override void FindFrame(int frameSize)
        {
            NPC.frameCounter += 1.0;
            NPC.frameCounter %= 50.0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            foreach (ChaosOrb orb in orbs)
            {
                orb.Draw(spriteBatch, NPC.position, Mod);
            }
            //spriteBatch.Draw(ModContent.Request<Texture2D>("Bluemagic/ChaosSpirit/ChaosEye"), npc.Center - Main.screenPosition, null, Color.White, npc.rotation, new Vector2(size / 2, size / 2), 1f, SpriteEffects.None, 0f);
            Texture2D eyeTexture = ModContent.Request<Texture2D>("Bluemagic/ChaosSpirit/ChaosEye2").Value;
            Vector2 eyePos = NPC.Center + size / 4 * NPC.rotation.ToRotationVector2();
            int frameNum = (int)(NPC.frameCounter / 10.0);
            Rectangle frame = new Rectangle(0, 0, eyeTexture.Width, eyeTexture.Height / 5);
            frame.Y = frameNum * frame.Height;
            Main.spriteBatch.Draw(eyeTexture, eyePos - Main.screenPosition, frame, Color.White, NPC.rotation, new Vector2(frame.Width / 2, frame.Height / 2), 1f, SpriteEffects.None, 0f);
            if (damageTotal >= dpsCap * 60)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Bluemagic/Mounts/ChaosShield").Value, NPC.Center - Main.screenPosition, null, Color.White * 0.5f, 0f, new Vector2(32, 32), 2.5f, SpriteEffects.None, 0f);
            }

            if (attack == 10 && attackProgress < 120)
            {
                Texture2D targetTexture = ModContent.Request<Texture2D>("Bluemagic/ChaosSpirit/Target").Value;
                Vector2 targetPos = new Vector2(NPC.localAI[1], NPC.localAI[2]);
                Color color = Main.hslToRgb((attackProgress / 60f) % 1f, 1f, 0.5f);
                Main.spriteBatch.Draw(targetTexture, targetPos - Main.screenPosition, null, color, 0f, new Vector2(targetTexture.Width / 2, targetTexture.Height / 2), 1f, SpriteEffects.None, 0f);
            }
            return false;
        }

        private void Talk(string key, byte r = 255, byte g = 255, byte b = 255)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Main.NewText(Language.GetTextValue(key), r, g, b);
            }
            else
            {
                NetworkText text = NetworkText.FromKey(key);
                ChatHelper.BroadcastChatMessage(text, new Color(r, g, b));
            }
        }

        private void PlaySound(SoundStyle s)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                if (targets.Contains(Main.myPlayer))
                {
                    SoundEngine.PlaySound(s);
                }
                else
                {
                    SoundEngine.PlaySound(s, NPC.position);
                }
            }
            /*else
            {
                ModPacket netMessage = GetPacket(ChaosSpiritMessageType.PlaySound);
                netMessage.Write(type);
                netMessage.Write(style);
                netMessage.Send();
            }*/
        }

        private ModPacket GetPacket(ChaosSpiritMessageType type)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)MessageType.ChaosSpirit);
            packet.Write(NPC.whoAmI);
            packet.Write((byte)type);
            return packet;
        }

        public void HandlePacket(BinaryReader reader)
        {
            ChaosSpiritMessageType type = (ChaosSpiritMessageType)reader.ReadByte();
            if (type == ChaosSpiritMessageType.HeroPlayer)
            {
                Player player = Main.player[Main.myPlayer];
                player.GetModPlayer<BluemagicPlayer>().heroLives = reader.ReadInt32();
            }
            else if (type == ChaosSpiritMessageType.TargetList)
            {
                int numTargets = reader.ReadInt32();
                targets.Clear();
                for (int k = 0; k < numTargets; k++)
                {
                    targets.Add(reader.ReadInt32());
                }
            }
            else if (type == ChaosSpiritMessageType.DeActivate)
            {
                NPC.active = false;
            }
            else if (type == ChaosSpiritMessageType.PlaySound)
            {
                // Todo: No????
                /*int soundType = reader.ReadInt32();
                int style = reader.ReadInt32();
                if (targets.Contains(Main.myPlayer))
                {
                    SoundEngine.PlaySound(soundType, -1, -1, style);
                }
                else
                {
                    SoundEngine.PlaySound(soundType, (int)NPC.position.X, (int)NPC.position.Y, style);
                }*/
            }
            else if (type == ChaosSpiritMessageType.Damage)
            {
                int damage = reader.ReadInt32();
                damageTotal += damage;
                if (Main.netMode == NetmodeID.Server)
                {
                    ModPacket netMessage = GetPacket(ChaosSpiritMessageType.Damage);
                    int ignore = reader.ReadInt32();
                    netMessage.Write(damage);
                    netMessage.Send(-1, ignore);
                }
            }
        }
    }

    class ChaosOrb
    {
        internal Vector2 position;
        internal Vector2 velocity;
        internal Color color;
        internal float strength;

        internal ChaosOrb(Vector2 position, Vector2 velocity, Color color)
        {
            this.position = position;
            this.velocity = velocity;
            this.color = color;
            this.strength = 1f;
        }

        internal void Update()
        {
            position += velocity;
            strength -= 0.02f;
        }

        internal void Draw(SpriteBatch spriteBatch, Vector2 offset, Mod mod)
        {
            Texture2D texture = ModContent.Request<Texture2D>("Bluemagic/ChaosSpirit/ChaosOrb").Value;
            Main.spriteBatch.Draw(texture, offset + position - Main.screenPosition, null, color * strength, 0f, new Vector2(texture.Width / 2, texture.Height / 2), 0.5f + 0.5f * strength, SpriteEffects.None, 0f);
        }
    }

    enum ChaosSpiritMessageType : byte
    {
        HeroPlayer,
        TargetList,
        DeActivate,
        PlaySound,
        Damage
    }
}