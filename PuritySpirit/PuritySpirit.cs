using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using System.Reflection;
using Terraria.GameContent.ItemDropRules;
using Bluemagic.Items.PuritySpirit;

namespace Bluemagic.PuritySpirit
{
    [AutoloadBossHead]
    public class PuritySpirit : ModNPC
    {
        private const int size = 120;
        private const int particleSize = 12;
        public static readonly int arenaWidth = (int)(1.3f * NPC.sWidth);
        public static readonly int arenaHeight = (int)(1.3f * NPC.sHeight);

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Spirit of Purity");
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
            NPCID.Sets.NeedsExpertScaling[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 400000;
            NPC.damage = 0;
            NPC.defense = 100;
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = true;
            NPC.width = size;
            NPC.height = size;
            NPC.value = Item.buyPrice(0, 50, 0, 0);
            NPC.npcSlots = 50f;
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
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/The sound of anxiety");
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.6f * balance);
            NPC.defense = 102;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 2f;
            return null;
        }

        private int difficulty
        {
            get
            {
                double strength = (double)NPC.life / (double)NPC.lifeMax;
                int difficulty = (int)(4.0 * (1.0 - strength));
                if (Main.expertMode)
                {
                    difficulty++;
                }
                return difficulty;
            }
        }

        private float difficultyGradient
        {
            get
            {
                double strength = (double)NPC.life / (double)NPC.lifeMax;
                double difficulty = 4.0 * (1.0 - strength);
                return (float)(difficulty % 1.0);
            }
        }

        private float timeMultiplier
        {
            get
            {
                return 1f - (difficulty + difficultyGradient) * 0.2f;
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

        private float attackTimer
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

        internal int attack
        {
            get
            {
                return (int)NPC.ai[2];
            }
            private set
            {
                NPC.ai[2] = value;
            }
        }

        internal int attackProgress
        {
            get
            {
                return (int)NPC.ai[3];
            }
            private set
            {
                NPC.ai[3] = value;
            }
        }

        private int portalFrame
        {
            get
            {
                return (int)NPC.localAI[0];
            }
            set
            {
                NPC.localAI[0] = value;
            }
        }

        private int shieldTimer
        {
            get
            {
                return (int)NPC.localAI[1];
            }
            set
            {
                NPC.localAI[1] = value;
            }
        }

        internal static int dpsCap
        {
            get
            {
                return BluemagicWorld.downedPuritySpirit ? 20000 : 10000;
            }
        }

        private IList<Particle> particles = new List<Particle>();
        private float[,] aura = new float[size, size];
        private int damageTotal = 0;
        private bool saidRushMessage = false;
        public readonly IList<int> targets = new List<int>();
        public int[] attackWeights = new int[]{ 2000, 2000, 2000, 2000, 3000 };
        public const int minAttackWeight = 1000;
        public const int maxAttackWeight = 4000;

        public override void AI()
        {
            if (!Main.dedServ)
            {
                UpdateParticles();
                portalFrame++;
                portalFrame %= 6 * Main.projFrames[ProjectileID.PortalGunGate];
            }
            FindPlayers();
            NPC.timeLeft = NPC.activeTime;
            if (stage > 0 && targets.Count == 0)
            {
                attackProgress = 0;
                stage = -1;
            }
            damageTotal -= dpsCap;
            if (damageTotal < 0)
            {
                damageTotal = 0;
            }
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }
            if (stage == 2 && difficulty > 0)
            {
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X - arenaWidth / 2, NPC.Center.Y, NegativeWall.speed, 0f, Mod.Find<ModProjectile>("NegativeWall").Type, 0, 0f, Main.myPlayer, NPC.whoAmI, arenaHeight);
                stage++;
            }
            if (stage == 3 && difficulty > 1)
            {
                SetupCrystals(arenaWidth / 3, false);
                stage++;
            }
            if (stage == 4 && difficulty > 2)
            {
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y - arenaHeight / 2, 0f, NegativeWall.speed, Mod.Find<ModProjectile>("NegativeWall").Type, 0, 0f, Main.myPlayer, NPC.whoAmI, -arenaWidth);
                stage++;
            }
            if (stage == 5 && difficulty > 3)
            {
                stage++;
            }
            switch (stage)
            {
                case -1:
                    RunAway();
                    break;
                case 0:
                    Initialize();
                    break;
                case 1:
                case 12:
                    attack = 4;
                    UltimateAttack();
                    if (attackProgress == 0)
                    {
                        stage++;
                        attackTimer = 160f * timeMultiplier;
                        attack = -1;
                    }
                    break;
                case 2:
                case 3:
                case 4:
                    DoAttack(4);
                    break;
                case 5:
                    DoAttack(4);
                    DoShield(1);
                    break;
                case 6:
                    DoAttack(5);
                    DoShield(2);
                    break;
                case 10:
                    if (attackProgress == 0)
                    {
                        stage++;
                    }
                    else
                    {
                        DoAttack(5);
                    }
                    break;
                case 11:
                    FinishFight1();
                    break;
                case 13:
                    FinishFight2();
                    break;
            }
        }

        private void UpdateParticles()
        {
            foreach (Particle particle in particles)
            {
                particle.Update();
            }
            Vector2 newPos = new Vector2(Main.rand.Next(3 * size / 8, 5 * size / 8), Main.rand.Next(3 * size / 8, 5 * size / 8));
            double newAngle = 2 * Math.PI * Main.rand.NextDouble();
            Vector2 newVel = new Vector2((float)Math.Cos(newAngle), (float)Math.Sin(newAngle));
            newVel *= 0.5f * (1f + (float)Main.rand.NextDouble());
            particles.Add(new Particle(newPos, newVel));
            if (particles[0].strength <= 0f)
            {
                particles.RemoveAt(0);
            }
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    aura[x, y] *= 0.97f;
                }
            }
            foreach (Particle particle in particles)
            {
                int minX = (int)particle.position.X - particleSize / 2;
                int minY = (int)particle.position.Y - particleSize / 2;
                int maxX = minX + particleSize;
                int maxY = minY + particleSize;
                for (int x = minX; x <= maxX; x++)
                {
                    for (int y = minY; y <= maxY; y++)
                    {
                        if (x >= 0 && x < size && y >= 0 && y < size)
                        {
                            float strength = particle.strength;
                            float offX = particle.position.X - x;
                            float offY = particle.position.Y - y;
                            strength *= 1f - (float)Math.Sqrt(offX * offX + offY * offY) / particleSize * 2;
                            if (strength < 0f)
                            {
                                strength = 0f;
                            }
                            aura[x, y] = 1f - (1f - aura[x, y]) * (1f - strength);
                        }
                    }
                }
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
                    ModPacket netMessage = GetPacket(PuritySpiritMessageType.TargetList);
                    netMessage.Write(targets.Count);
                    foreach (int target in targets)
                    {
                        netMessage.Write(target);
                    }
                    netMessage.Send();
                }
            }
        }

        public void RunAway()
        {
            attackProgress++;
            if (attackProgress == 180)
            {
                Talk("Hmph. Was that the extent of your power?");
            }
            if (attackProgress >= 360)
            {
                NPC.active = false;
            }
        }

        public void Initialize()
        {
            if (attackProgress == 1)
            {
                Vector2 center = NPC.Center;
                for (int k = 0; k < 255; k++)
                {
                    Player player = Main.player[k];
                    if (player.active && player.position.X > center.X - arenaWidth / 2 && player.position.X + player.width < center.X + arenaWidth / 2 && player.position.Y > center.Y - arenaHeight / 2 && player.position.Y + player.height < center.Y + arenaHeight / 2)
                    {
                        player.GetModPlayer<BluemagicPlayer>().heroLives = 3;
                        if (Main.netMode == NetmodeID.Server)
                        {
                            ModPacket netMessage = GetPacket(PuritySpiritMessageType.HeroPlayer);
                            netMessage.Send(k);
                        }
                    }
                }
                shieldTimer = 1000;
            }
            attackProgress++;
            if (attackProgress == 90)
            {
                if (BluemagicWorld.downedPuritySpirit)
                {
                    Talk("What, you again? Oh well...");
                }
                else
                {
                    Talk("You, who have challenged me...");
                }
            }
            if (attackProgress == 180)
            {
                SetupCrystals(arenaWidth / 6, true);
            }
            if (attackProgress >= 420)
            {
                Talk("Show me the power that has saved Terraria!");
                attackProgress = 0;
                stage++;
                NPC.dontTakeDamage = false;
                if (Main.netMode == NetmodeID.Server)
                {
                    ModPacket netMessage = GetPacket(PuritySpiritMessageType.DontTakeDamage);
                    netMessage.Write(false);
                    netMessage.Send();
                }
            }
        }

        private void SetupCrystals(int radius, bool clockwise)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }
            Vector2 center = NPC.Center;
            for (int k = 0; k < 10; k++)
            {
                float angle = 2f * (float)Math.PI / 10f * k;
                Vector2 pos = center + radius * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                int damage = 120;
                if (Main.expertMode)
                {
                    damage = (int)(150 / Main.GameModeInfo.EnemyDamageMultiplier);
                }
                Projectile.NewProjectile(NPC.GetSource_FromThis(), pos.X, pos.Y, radius, clockwise ? 1 : -1, Mod.Find<ModProjectile>("PureCrystal").Type, damage, 0f, Main.myPlayer, NPC.whoAmI, angle);
            }
        }

        private void UltimateAttack()
        {
            if (attackProgress == 0)
            {
                PlaySound(SoundID.Roar);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int damage = Main.expertMode ? 720 : 600;
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, 0f, 0f, Mod.Find<ModProjectile>("VoidWorld").Type, damage, 0f, Main.myPlayer, NPC.whoAmI, Main.rand.Next());
                }
            }
            attackProgress++;
            if (attackProgress >= 500)
            {
                attackProgress = 0;
            }
        }

        private void DoAttack(int numAttacks)
        {
            if (attackTimer > 0f)
            {
                attackTimer -= 1f;
                return;
            }
            if (attack < 0)
            {
                int totalWeight = 0;
                for (int k = 0; k < numAttacks; k++)
                {
                    if (attackWeights[k] < minAttackWeight)
                    {
                        attackWeights[k] = minAttackWeight;
                    }
                    totalWeight += attackWeights[k];
                }
                int choice = Main.rand.Next(totalWeight);
                for (attack = 0; attack < numAttacks; attack++)
                {
                    if (choice < attackWeights[attack])
                    {
                        break;
                    }
                    choice -= attackWeights[attack];
                }
                attackWeights[attack] -= 80;
                NPC.netUpdate = true;
            }
            switch (attack)
            {
                case 0:
                    BeamAttack();
                    break;
                case 1:
                    SnakeAttack();
                    break;
                case 2:
                    LaserAttack();
                    break;
                case 3:
                    SphereAttack();
                    break;
                case 4:
                    UltimateAttack();
                    break;
            }
            if (attackProgress == 0)
            {
                attackTimer += 160f * timeMultiplier;
                attack = -1;
            }
        }

        private void BeamAttack()
        {
            if (attackProgress == 0)
            {
                float y = NPC.Center.Y;
                int damage = Main.expertMode ? 360 : 300;
                for (int k = 0; k < targets.Count; k++)
                {
                    float x = Main.player[targets[k]].Center.X;
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), x, y, 0f, 0f, Mod.Find<ModProjectile>("PurityBeam").Type, damage, 0f, Main.myPlayer, arenaHeight);
                    for (int j = -1; j <= 1; j += 2)
                    {
                        float spawnX = x + j * Main.rand.Next(200, 401);
                        if (spawnX > NPC.Center.X + arenaWidth / 2)
                        {
                            spawnX -= arenaWidth;
                        }
                        else if (spawnX < NPC.Center.X - arenaWidth / 2)
                        {
                            spawnX += arenaWidth;
                        }
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), spawnX, y, 0f, 0f, Mod.Find<ModProjectile>("PurityBeam").Type, damage, 0f, Main.myPlayer, arenaHeight);
                    }
                }
                int numExtra = 2 * (difficulty + 1) - 2 * (targets.Count - 1);
                if (difficulty >= 2)
                {
                    numExtra--;
                }
                if (difficulty >= 4)
                {
                    numExtra--;
                }
                for (int k = 0; k < numExtra; k++)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X + Main.rand.Next(-arenaWidth / 2 + 50, arenaWidth / 2 - 50 + 1), y, 0f, 0f, Mod.Find<ModProjectile>("PurityBeam").Type, damage, 0f, Main.myPlayer, arenaHeight);
                }
                attackProgress = (int)(PurityBeam.charge + 60f);
            }
            attackProgress--;
            if (attackProgress < 0)
            {
                attackProgress = 0;
            }
        }

        private void SnakeAttack()
        {
            if (attackProgress == 0)
            {
                int damage = Main.expertMode ? 60 : 80;
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, 0f, 0f, Mod.Find<ModProjectile>("PuritySnake").Type, damage, 0f, Main.myPlayer, NPC.whoAmI, timeMultiplier);
                attackProgress = 240;
            }
            attackProgress--;
            if (attackProgress < 0)
            {
                attackProgress = 0;
            }
        }

        private void LaserAttack()
        {
            if (attackProgress == 0)
            {
                int numAttacks = 3 + difficulty / 2;
                float timer = 30f + 20f * timeMultiplier;
                float totalTime = numAttacks * timer + 120f;
                int damage = Main.expertMode ? 70 : 100;
                for (int k = 0; k < numAttacks; k++)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, (int)totalTime, timer, Mod.Find<ModProjectile>("NullLaser").Type, damage, 0f, Main.myPlayer, NPC.whoAmI, (int)(60f + k * timer));
                }
                attackProgress = (int)totalTime;
            }
            if (attackProgress % 20 == 0)
            {
                PlaySound(SoundID.Roar);
            }
            Dust.NewDust(NPC.position, NPC.width, NPC.height, Mod.Find<ModDust>("Sparkle").Type, 0f, 0f, 0, new Color(0, 180, 0), 1.5f);
            attackProgress--;
            if (attackProgress < 0)
            {
                attackProgress = 0;
            }
        }

        private void SphereAttack()
        {
            if (attackProgress == 0)
            {
                int damage = Main.expertMode ? 75 : 120;
                float time = 60f + 60f * timeMultiplier;
                int rotationSpeed = Main.rand.Next(2) * 2 - 1;
                int numSpheres = 3 + difficulty / 2;
                int numGroups = 4 + difficulty / 3;
                float radius = PuritySphere.radius;
                for (int j = 0; j < numGroups || j < targets.Count; j++)
                {
                    int target;
                    Vector2 center;
                    if (j < targets.Count)
                    {
                        target = targets[j];
                        center = Main.player[target].Center;
                    }
                    else
                    {
                        target = 255;
                        center = NPC.Center + new Vector2(Main.rand.Next(-arenaWidth / 2 + (int)radius, arenaWidth / 2 - (int)radius + 1), Main.rand.Next(-arenaWidth / 2 + (int)radius, arenaWidth / 2 - (int)radius + 1));
                    }
                    float angle = (float)(Main.rand.NextDouble() * 2 * Math.PI / numSpheres);
                    for (int k = 0; k < numSpheres; k++)
                    {
                        Vector2 pos = center + radius * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                        angle += 2f * (float)Math.PI / numSpheres;
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), pos.X, pos.Y, target == 0f ? -1f : target, rotationSpeed, Mod.Find<ModProjectile>("PuritySphere").Type, damage, (int)time, Main.myPlayer, center.X, center.Y);
                    }
                }
                attackProgress = 60 + (int)time + PuritySphere.strikeTime;
            }
            attackProgress--;
            if (attackProgress < 0)
            {
                attackProgress = 0;
            }
        }

        private void DoShield(int numShields)
        {
            int count = 0;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && Main.npc[k].type == Mod.Find<ModNPC>("PurityShield").Type && Main.npc[k].ai[0] == NPC.whoAmI)
                {
                    count++;
                }
            }
            if (count >= numShields)
            {
                shieldTimer = 0;
                return;
            }
            float timeMult = timeMultiplier * 5f;
            shieldTimer++;
            if (shieldTimer >= 300 + 300 * timeMult)
            {
                float targetX = NPC.Center.X + (Main.rand.Next(2) * 2 - 1) * arenaWidth / 4;
                float targetY = NPC.Center.Y + (Main.rand.Next(2) * 2 - 1) * arenaHeight / 4;
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y + 40, Mod.Find<ModNPC>("PurityShield").Type, 0, NPC.whoAmI, targetX, targetY);
                shieldTimer = 0;
            }
        }

        public override bool CheckDead()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient && stage < 13)
            {
                NPC.active = true;
                NPC.life = 1;
                NPC.dontTakeDamage = true;
                return false;
            }
            if (stage < 10)
            {
                NPC.active = true;
                NPC.life = 1;
                NPC.dontTakeDamage = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    ModPacket netMessage = GetPacket(PuritySpiritMessageType.DontTakeDamage);
                    netMessage.Write(true);
                    netMessage.Send();
                }
                stage = 10;
                return false;
            }
            return true;
        }

        public void FinishFight1()
        {
            attackProgress++;
            if (attackProgress == 60)
            {
                Talk("This is... I thank you for demonstrating your power.");
            }
            if (attackProgress >= 240)
            {
                Talk("Please take this as a farewell gift.");
                stage++;
                attackProgress = 0;
            }
        }

        public void FinishFight2()
        {
            attackProgress++;
            if (attackProgress == 120)
            {
                if (BluemagicWorld.downedPuritySpirit)
                {
                    Talk("I wish you luck in your future farming.");
                }
                else
                {
                    Talk("I wish you luck in your future endeavors.");
                }
            }
            if (attackProgress >= 180)
            {
                NPC.dontTakeDamage = false;
                NPC.HitSound = null;
                NPC.SimpleStrikeNPC(9999, 0, false, 0);
            }
        }

        public override void OnKill()
        {
            BluemagicWorld.downedPuritySpirit = true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<PuritySpiritBag>()));
            npcLoot.Add(ItemDropRule.OneFromOptions(10, ModContent.ItemType<PuritySpiritTrophy>(), ModContent.ItemType<TreeTrophy>(), ModContent.ItemType<BunnyTrophy>()));
            npcLoot.Add(ItemDropRule.OneFromOptions(7, ModContent.ItemType<PuritySpiritMask>(), ModContent.ItemType<BunnyMask>(), ItemID.Bunny, ItemID.Bunny, ItemID.Bunny, ItemID.Bunny, ItemID.Bunny));
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<InfinityCrystal>()));
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "The " + name;
            potionType = ItemID.SuperHealingPotion;
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
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && Main.npc[k].type == Mod.Find<ModNPC>("PurityShield").Type && Main.npc[k].ai[0] == NPC.whoAmI)
                {
                    return false;
                }
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
                ModPacket netMessage = GetPacket(PuritySpiritMessageType.Damage);
                netMessage.Write(damage * 60);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    netMessage.Write(Main.myPlayer);
                }
                netMessage.Send();
            }
        }

        /*public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if (damageTotal >= dpsCap * 60)
            {
                if (!saidRushMessage && Main.netMode != 1)
                {
                    Talk("Oh, in a rush now, are we?");
                    saidRushMessage = true;
                }
                damage = 0;
                return false;
            }
            return true;
        }*/

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Vector2 drawPos = NPC.position - Main.screenPosition;
                    drawPos.X += x * 2 - size / 2;
                    drawPos.Y += y * 2 - size / 2;
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Bluemagic/PuritySpirit/PurityParticle").Value, drawPos, null, Color.White * aura[x, y], 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
            }
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Bluemagic/PuritySpirit/PurityEyes").Value, NPC.position - Main.screenPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            /*if (damageTotal >= dpsCap * 60)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Bluemagic/Mounts/PurityShield"), npc.Center - Main.screenPosition, null, Color.White * 0.5f, 0f, new Vector2(32, 32), 2.5f, SpriteEffects.None, 0f);
            }*/
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            int portalWidth = 48;
            int portalDepth = 18;
            Color color = new Color(64, 255, 64);
            int centerX = (int)NPC.Center.X;
            int centerY = (int)NPC.Center.Y;
            Main.instance.LoadProjectile(ProjectileID.PortalGunGate);
            for (int x = centerX - arenaWidth / 2; x < centerX + arenaWidth / 2; x += portalWidth)
            {
                int frameNum = (portalFrame / 6 + x / portalWidth) % Main.projFrames[ProjectileID.PortalGunGate];
                Rectangle frame = new Rectangle(0, frameNum * (portalWidth + 2), portalDepth, portalWidth);
                Vector2 drawPos = new Vector2(x + portalWidth / 2, centerY - arenaHeight / 2) - Main.screenPosition;
                Main.spriteBatch.Draw(TextureAssets.Projectile[ProjectileID.PortalGunGate].Value, drawPos, frame, color, (float)-Math.PI / 2f, new Vector2(portalDepth / 2, portalWidth / 2), 1f, SpriteEffects.None, 0f);
                drawPos.Y += arenaHeight;
                Main.spriteBatch.Draw(TextureAssets.Projectile[ProjectileID.PortalGunGate].Value, drawPos, frame, color, (float)Math.PI / 2f, new Vector2(portalDepth / 2, portalWidth / 2), 1f, SpriteEffects.None, 0f);
            }
            for (int y = centerY - arenaHeight / 2; y < centerY + arenaHeight / 2; y += portalWidth)
            {
                int frameNum = (portalFrame / 6 + y / portalWidth) % Main.projFrames[ProjectileID.PortalGunGate];
                Rectangle frame = new Rectangle(0, frameNum * (portalWidth + 2), portalDepth, portalWidth);
                Vector2 drawPos = new Vector2(centerX - arenaWidth / 2, y + portalWidth / 2) - Main.screenPosition;
                Main.spriteBatch.Draw(TextureAssets.Projectile[ProjectileID.PortalGunGate].Value, drawPos, frame, color, (float)Math.PI, new Vector2(portalDepth / 2, portalWidth / 2), 1f, SpriteEffects.None, 0f);
                drawPos.X += arenaWidth;
                Main.spriteBatch.Draw(TextureAssets.Projectile[ProjectileID.PortalGunGate].Value, drawPos, frame, color, 0f, new Vector2(portalDepth / 2, portalWidth / 2), 1f, SpriteEffects.None, 0f);
            }
        }

        private void Talk(string message)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                string text = Language.GetTextValue("Mods.Bluemagic.NPCTalk", Lang.GetNPCNameValue(NPC.type), message);
                Main.NewText(text, 150, 250, 150);
            }
            else
            {
                NetworkText text = NetworkText.FromKey("Mods.Bluemagic.NPCTalk", Lang.GetNPCNameValue(NPC.type), message);
                ChatHelper.BroadcastChatMessage(text, new Color(150, 250, 150));
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
            else
            {
                /*ModPacket netMessage = GetPacket(PuritySpiritMessageType.PlaySound);
                netMessage.Write(type);
                netMessage.Write(style);
                netMessage.Send();*/
            }
        }

        private ModPacket GetPacket(PuritySpiritMessageType type)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)MessageType.PuritySpirit);
            packet.Write(NPC.whoAmI);
            packet.Write((byte)type);
            return packet;
        }

        public void HandlePacket(BinaryReader reader)
        {
            PuritySpiritMessageType type = (PuritySpiritMessageType)reader.ReadByte();
            if (type == PuritySpiritMessageType.HeroPlayer)
            {
                Player player = Main.player[Main.myPlayer];
                player.GetModPlayer<BluemagicPlayer>().heroLives = 3;
            }
            else if (type == PuritySpiritMessageType.TargetList)
            {
                int numTargets = reader.ReadInt32();
                targets.Clear();
                for (int k = 0; k < numTargets; k++)
                {
                    targets.Add(reader.ReadInt32());
                }
            }
            else if (type == PuritySpiritMessageType.DontTakeDamage)
            {
                NPC.dontTakeDamage = reader.ReadBoolean();
            }
            else if (type == PuritySpiritMessageType.PlaySound)
            {
               /* int soundType = reader.ReadInt32();
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
            else if (type == PuritySpiritMessageType.Damage)
            {
                int damage = reader.ReadInt32();
                damageTotal += damage;
                if (Main.netMode == NetmodeID.Server)
                {
                    ModPacket netMessage = GetPacket(PuritySpiritMessageType.Damage);
                    int ignore = reader.ReadInt32();
                    netMessage.Write(damage);
                    netMessage.Send(-1, ignore);
                }
            }
        }
    }

    class Particle
    {
        internal Vector2 position;
        internal Vector2 velocity;
        internal float strength;

        internal Particle(Vector2 pos, Vector2 vel)
        {
            this.position = pos;
            this.velocity = vel;
            this.strength = 0.75f;
        }

        internal void Update()
        {
            this.position += this.velocity * this.strength;
            this.strength -= 0.01f;
        }
    }

    enum PuritySpiritMessageType : byte
    {
        HeroPlayer,
        TargetList,
        DontTakeDamage,
        PlaySound,
        Damage
    }
}
