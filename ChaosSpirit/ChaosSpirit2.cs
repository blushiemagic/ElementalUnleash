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
    public class ChaosSpirit2 : ModNPC
    {
        private const int size = ChaosSpirit.size;
        public const float armLength = 400f;

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
            NPC.defense = 0;
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = false;
            NPC.chaseable = false;
            NPC.width = size;
            NPC.height = size;
            NPC.value = Item.buyPrice(1, 0, 0, 0);
            NPC.npcSlots = 40f;
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
            Music = MusicID.LunarBoss;
            //bossBag = mod.ItemType("ChaosSpiritBag");
        }

        private List<ChaosOrb> orbs = new List<ChaosOrb>();
        internal List<int> targets = new List<int>();
        private bool canMove = true;
        public float armRotation = 0f;
        private bool syncTargets = false;

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
            writer.Write(armRotation);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.localAI[0] = reader.ReadSingle();
            NPC.localAI[1] = reader.ReadSingle();
            NPC.localAI[2] = reader.ReadSingle();
            NPC.localAI[3] = reader.ReadSingle();
            armRotation = reader.ReadSingle();
        }

        public override void AI()
        {
            Bluemagic.freezeHeroLives = true;
            if (stage == 100)
            {
                Transform();
                return;
            }
            if (!Main.dedServ)
            {
                UpdateChaosOrbs();
            }
            FindPlayers();
            int debuffType = Mod.Find<ModBuff>("ChaosPressure4").Type;
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
            NPC.TargetClosest();
            NPC.rotation = (Main.player[NPC.target].Center - NPC.Center).ToRotation();
            canMove = true;
            switch (stage)
            {
                case -1:
                    RunAway();
                    break;
                case 0:
                    Initialize();
                    break;
                default:
                    DoAttack();
                    break;
            }
            if (canMove)
            {
                Move();
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
            Color color = ChaosSpirit.mainColor;
            color.R += (byte)Main.rand.Next(-5, 6);
            color.G += (byte)Main.rand.Next(-4, 5);
            color.B += (byte)Main.rand.Next(-3, 4);
            return color;
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
                if (Main.netMode == NetmodeID.Server && (syncTargets || targets.Count != originalCount))
                {
                    ModPacket netMessage = GetPacket(ChaosSpiritMessageType.TargetList);
                    netMessage.Write(targets.Count);
                    foreach (int target in targets)
                    {
                        netMessage.Write(target);
                    }
                    netMessage.Send();
                    syncTargets = false;
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
            if (attackProgress == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int k = 0; k < 6; k++)
                {
                    NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Bottom.X, (int)NPC.Bottom.Y, Mod.Find<ModNPC>("ChaosSpiritArm").Type, 0, NPC.whoAmI, k);
                }
            }
            if (attackProgress == 10)
            {
                syncTargets = true;
            }
            if (attackProgress == 0)
            {
                PlaySound(SoundID.Roar);
            }
            attackProgress++;
            if (attackProgress >= 300)
            {
                attackProgress = 0;
                stage++;
            }
        }

        public int RandomTarget()
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
                int interval = Main.expertMode ? 5 : 6;
                if (stage == interval)
                {
                    SetAttack(10);
                }
                else if (stage == 2 * interval)
                {
                    SetAttack(11);
                }
                else
                {
                    SetAttack(1);
                }
                stage++;
                if (stage > 2 * interval)
                {
                    stage = 1;
                }
            }
            if (attack == 1)
            {
                SetArmAttacks();
            }
            else if (attack == 10)
            {
                UltimateAttack1();
            }
            else if (attack == 11)
            {
                UltimateAttack2();
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

        private void SetArmAttacks()
        {
            if (attackProgress == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC[] arms = new NPC[6];
                int index = 0;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].type == Mod.Find<ModNPC>("ChaosSpiritArm").Type && Main.npc[k].ai[0] == NPC.whoAmI)
                    {
                        arms[index] = Main.npc[k];
                        index++;
                        if (index >= arms.Length)
                        {
                            break;
                        }
                    }
                }
                for (int k = 0; k < arms.Length - 1; k++)
                {
                    int choice = Main.rand.Next(k, arms.Length);
                    NPC temp = arms[k];
                    arms[k] = arms[choice];
                    arms[choice] = temp;
                }
                for (int k = 0; k < arms.Length; k++)
                {
                    arms[k].localAI[1] = k + 1;
                    arms[k].localAI[3] = 90f * k;
                    arms[k].netUpdate = true;
                }
            }
            attackProgress++;
            if (attackProgress >= 1000f)
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
            NPC.position += 0.005f * offset;
        }

        public void Damage()
        {
            NPC.dontTakeDamage = true;
            int damage = NPC.lifeMax / (BluemagicWorld.downedChaosSpirit ? 6 : 8);
            if (damage < NPC.lifeMax / (BluemagicWorld.downedChaosSpirit ? 6f : 8f))
            {
                damage++;
            }
            NPC.SimpleStrikeNPC(damage, 0, false, 0);
            PlaySoundSafe(SoundID.Roar);
            NPC.dontTakeDamage = false;
        }

        public override bool CheckDead()
        {
            NPC.active = true;
            NPC.life = 1;
            NPC.dontTakeDamage = true;
            stage = 100;
            return false;
        }

        private void Transform()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Bottom.X, (int)NPC.Bottom.Y, Mod.Find<ModNPC>("ChaosSpirit3").Type);
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
            return false;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return false;
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
        }

        private void PlaySoundSafe(SoundStyle s)
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
             * Todo: No????
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
                //TODO: No????
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
            }
        }
    }
}