using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
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
            DisplayName.SetDefault("Spirit of Chaos");
            NPCID.Sets.MustAlwaysDraw[npc.type] = true;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 400000;
            npc.damage = 200;
            npc.defense = 0;
            npc.knockBackResist = 0f;
            npc.dontTakeDamage = false;
            npc.chaseable = false;
            npc.width = size;
            npc.height = size;
            npc.value = Item.buyPrice(1, 0, 0, 0);
            npc.npcSlots = 40f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = null;
            npc.alpha = 255;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
            music = MusicID.LunarBoss;
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
                return (int)npc.ai[0];
            }
            set
            {
                npc.ai[0] = value;
            }
        }

        private int attack
        {
            get
            {
                return (int)npc.ai[1];
            }
            set
            {
                npc.ai[1] = value;
            }
        }

        private int attackProgress
        {
            get
            {
                return (int)npc.ai[2];
            }
            set
            {
                npc.ai[2] = value;
            }
        }

        private int attackCooldown
        {
            get
            {
                return (int)npc.ai[3];
            }
            set
            {
                npc.ai[3] = value;
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax / Main.expertLife * 1.2f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.75f);
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 2f;
            return null;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(npc.localAI[0]);
            writer.Write(npc.localAI[1]);
            writer.Write(npc.localAI[2]);
            writer.Write(npc.localAI[3]);
            writer.Write(armRotation);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            npc.localAI[0] = reader.ReadSingle();
            npc.localAI[1] = reader.ReadSingle();
            npc.localAI[2] = reader.ReadSingle();
            npc.localAI[3] = reader.ReadSingle();
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
            int debuffType = mod.BuffType("ChaosPressure4");
            foreach (int target in targets)
            {
                Main.player[target].AddBuff(debuffType, 2, false);
            }
            npc.timeLeft = NPC.activeTime;
            if (stage > 0 && targets.Count == 0)
            {
                attackProgress = 0;
                stage = -1;
                npc.netUpdate = true;
            }
            npc.TargetClosest();
            npc.rotation = (Main.player[npc.target].Center - npc.Center).ToRotation();
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
            if (Main.netMode != 1)
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
                if (Main.netMode == 2 && (syncTargets || targets.Count != originalCount))
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
            npc.netUpdate = true;
        }

        public void RunAway()
        {
            Bluemagic.freezeHeroLives = false;
            attackProgress++;
            if (attackProgress >= 360)
            {
                npc.active = false;
            }
        }

        private void Initialize()
        {
            canMove = false;
            if (attackProgress == 0 && Main.netMode != 1)
            {
                for (int k = 0; k < 6; k++)
                {
                    NPC.NewNPC((int)npc.Bottom.X, (int)npc.Bottom.Y, mod.NPCType("ChaosSpiritArm"), 0, npc.whoAmI, k);
                }
            }
            if (attackProgress == 10)
            {
                syncTargets = true;
            }
            if (attackProgress == 0)
            {
                PlaySound(15, 0);
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
            if (attack == 0 && attackCooldown <= 0 && Main.netMode != 1)
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
            if (Main.netMode != 1 && attackProgress == 0)
            {
                int target = RandomTarget();
                npc.localAI[0] = target;
                Vector2 targetPos = Main.player[target].Center;
                npc.localAI[1] = targetPos.X;
                npc.localAI[2] = targetPos.Y;
                npc.netUpdate = true;
            }
            if (Main.netMode == 2 && attackProgress == 10)
            {
                npc.netUpdate = true;
            }
            if (attackProgress % 20 == 0)
            {
                PlaySound(2, 15);
            }
            if (attackProgress == 120)
            {
                if (Main.netMode != 1)
                {
                    Vector2 targetPos = new Vector2(npc.localAI[1], npc.localAI[2]);
                    Vector2 offset = targetPos - npc.Center;
                    float rotation = MathHelper.PiOver2;
                    if (offset != Vector2.Zero)
                    {
                        rotation = offset.ToRotation();
                    }
                    targetPos = Main.player[(int)npc.localAI[0]].Center;
                    offset = targetPos - npc.Center;
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
                    int proj = Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("CataclysmicRay"), damage, rotSpeed, Main.myPlayer, npc.whoAmI, rotation);
                    npc.localAI[3] = proj;
                    npc.netUpdate = true;
                }
                PlaySound(29, 104);
            }
            attackProgress++;
            if (attackProgress > 120)
            {
                Projectile laser = Main.projectile[(int)npc.localAI[3]];
                if (laser.active && laser.type == mod.ProjectileType("CataclysmicRay"))
                {
                    npc.rotation = laser.ai[1] + laser.localAI[0];
                }
                else
                {
                    attack = 0;
                    attackProgress = 0;
                }
            }
            else
            {
                Vector2 offset = new Vector2(npc.localAI[1], npc.localAI[2]) - npc.Center;
                npc.rotation = MathHelper.PiOver2;
                if (offset != Vector2.Zero)
                {
                    npc.rotation = offset.ToRotation();
                }
            }
        }

        private void UltimateAttack2()
        {
            canMove = false;
            if (Main.netMode != 1)
            {
                if (attackProgress == 0)
                {
                    Talk("Mods.Bluemagic.ChaosPressureStart");
                    float angle = Main.rand.NextFloat() * MathHelper.TwoPi;
                    Vector2 offset = 320f * angle.ToRotationVector2();
                    Projectile.NewProjectile(npc.Center + offset, Vector2.Zero, mod.ProjectileType("HolySphere"), 0, 0f, Main.myPlayer, npc.whoAmI);
                    
                }
                if (attackProgress == 30)
                {
                    Talk("Mods.Bluemagic.ChaosPressureLight");
                }
            }
            attackProgress++;
            if (attackProgress == 260f)
            {
                PlaySound(29, 104);
            }
            if (attackProgress >= 300f)
            {
                attack = 0;
                attackProgress = 0;
            }
        }

        private void SetArmAttacks()
        {
            if (attackProgress == 0 && Main.netMode != 1)
            {
                NPC[] arms = new NPC[6];
                int index = 0;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].type == mod.NPCType("ChaosSpiritArm") && Main.npc[k].ai[0] == npc.whoAmI)
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
            Vector2 closest = npc.Center;
            float distance = -1f;
            foreach (int playerIndex in targets)
            {
                Player player = Main.player[playerIndex];
                float playerDistance = Vector2.Distance(player.Center, npc.Center);
                if (distance < 0f || playerDistance < distance)
                {
                    closest = player.Center;
                    distance = playerDistance;
                }
            }
            Vector2 offset = closest - npc.Center;
            npc.position += 0.005f * offset;
        }

        public void Damage()
        {
            npc.dontTakeDamage = true;
            int damage = npc.lifeMax / (BluemagicWorld.downedChaosSpirit ? 6 : 8);
            if (damage < npc.lifeMax / (BluemagicWorld.downedChaosSpirit ? 6f : 8f))
            {
                damage++;
            }
            npc.StrikeNPCNoInteraction(damage, 0f, 0);
            PlaySoundSafe(15, 0);
            npc.dontTakeDamage = false;
        }

        public override bool CheckDead()
        {
            npc.active = true;
            npc.life = 1;
            npc.dontTakeDamage = true;
            stage = 100;
            return false;
        }

        private void Transform()
        {
            if (Main.netMode != 1)
            {
                NPC.NewNPC((int)npc.Bottom.X, (int)npc.Bottom.Y, mod.NPCType("ChaosSpirit3"));
                npc.active = false;
                if (Main.netMode == 2)
                {
                    ModPacket packet = GetPacket(ChaosSpiritMessageType.DeActivate);
                    packet.Send();
                }
            }
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            BluemagicPlayer modPlayer = target.GetModPlayer<BluemagicPlayer>();
            modPlayer.constantDamage = npc.damage;
            modPlayer.percentDamage = 1f / 3f;
            if (Main.expertMode)
            {
                modPlayer.percentDamage *= 1.5f;
            }
            modPlayer.chaosDefense = true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("Undead"), 300, false);
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
            npc.frameCounter += 1.0;
            npc.frameCounter %= 50.0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            foreach (ChaosOrb orb in orbs)
            {
                orb.Draw(spriteBatch, npc.position, mod);
            }
            //spriteBatch.Draw(mod.GetTexture("ChaosSpirit/ChaosEye"), npc.Center - Main.screenPosition, null, Color.White, npc.rotation, new Vector2(size / 2, size / 2), 1f, SpriteEffects.None, 0f);
            Texture2D eyeTexture = mod.GetTexture("ChaosSpirit/ChaosEye2");
            Vector2 eyePos = npc.Center + size / 4 * npc.rotation.ToRotationVector2();
            int frameNum = (int)(npc.frameCounter / 10.0);
            Rectangle frame = new Rectangle(0, 0, eyeTexture.Width, eyeTexture.Height / 5);
            frame.Y = frameNum * frame.Height;
            spriteBatch.Draw(eyeTexture, eyePos - Main.screenPosition, frame, Color.White, npc.rotation, new Vector2(frame.Width / 2, frame.Height / 2), 1f, SpriteEffects.None, 0f);

            if (attack == 10 && attackProgress < 120)
            {
                Texture2D targetTexture = mod.GetTexture("ChaosSpirit/Target");
                Vector2 targetPos = new Vector2(npc.localAI[1], npc.localAI[2]);
                Color color = Main.hslToRgb((attackProgress / 60f) % 1f, 1f, 0.5f);
                spriteBatch.Draw(targetTexture, targetPos - Main.screenPosition, null, color, 0f, new Vector2(targetTexture.Width / 2, targetTexture.Height / 2), 1f, SpriteEffects.None, 0f);
            }
            return false;
        }

        private void Talk(string key, byte r = 255, byte g = 255, byte b = 255)
        {
            if (Main.netMode != 2)
            {
                Main.NewText(Language.GetTextValue(key), r, g, b);
            }
            else
            {
                NetworkText text = NetworkText.FromKey(key);
                NetMessage.BroadcastChatMessage(text, new Color(r, g, b));
            }
        }

        private void PlaySound(int type, int style)
        {
            if (Main.netMode != 2)
            {
                if (targets.Contains(Main.myPlayer))
                {
                    Main.PlaySound(type, -1, -1, style);
                }
                else
                {
                    Main.PlaySound(type, (int)npc.position.X, (int)npc.position.Y, style);
                }
            }
        }

        private void PlaySoundSafe(int type, int style)
        {
            if (Main.netMode != 2)
            {
                if (targets.Contains(Main.myPlayer))
                {
                    Main.PlaySound(type, -1, -1, style);
                }
                else
                {
                    Main.PlaySound(type, (int)npc.position.X, (int)npc.position.Y, style);
                }
            }
            else
            {
                ModPacket netMessage = GetPacket(ChaosSpiritMessageType.PlaySound);
                netMessage.Write(type);
                netMessage.Write(style);
                netMessage.Send();
            }
        }

        private ModPacket GetPacket(ChaosSpiritMessageType type)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)MessageType.ChaosSpirit);
            packet.Write(npc.whoAmI);
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
                npc.active = false;
            }
            else if (type == ChaosSpiritMessageType.PlaySound)
            {
                int soundType = reader.ReadInt32();
                int style = reader.ReadInt32();
                if (targets.Contains(Main.myPlayer))
                {
                    Main.PlaySound(soundType, -1, -1, style);
                }
                else
                {
                    Main.PlaySound(soundType, (int)npc.position.X, (int)npc.position.Y, style);
                }
            }
            else if (type == ChaosSpiritMessageType.Damage)
            {
            }
        }
    }
}