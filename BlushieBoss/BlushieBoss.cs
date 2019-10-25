using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;

namespace Bluemagic.BlushieBoss
{
    public static class BlushieBoss
    {
        public const int ArenaSize = 800;
        internal static int Phase = 0;
        internal static int Timer = 0;
        internal static List<Bullet> bullets = new List<Bullet>();
        internal static List<Vector2> crystalStars = new List<Vector2>();
        internal static Vector2 Origin;
        internal static bool[] Players = new bool[256];
        internal static bool CameraFocus = false;
        internal static int[] index = new int[] { -1, -1, -1, -1, -1, -1 };
        internal static bool BlushieC;
        internal static Vector2 PosK;
        internal static Vector2 PosA;
        internal static Vector2 PosL;
        internal static Vector2 DataK;
        internal static float DataA;
        internal static Vector2 DataL;
        internal static float DataL2;
        internal static int Immune = 0;
        internal static int HealthK;
        internal static int HealthA;
        internal static int HealthL;
        internal static int ShieldK = 0;
        internal static int ShieldA = 0;
        internal static int ShieldL = 0;
        internal static Vector2 DragonPos;
        internal static Vector2 ArmLeftPos;
        internal static Vector2 ArmRightPos;
        internal static Vector2 SkullPos;
        internal static Vector2 BoneLTPos;
        internal static Vector2 BoneLBPos;
        internal static Vector2 BoneRTPos;
        internal static Vector2 BoneRBPos;
        internal static float BoneLTRot;
        internal static float BoneLBRot;
        internal static float BoneRTRot;
        internal static float BoneRBRot;
        internal static float flash = 0f;
        internal static int Phase3Attack;
        internal static bool SpawnedStars = false;
        internal static Vector2 Phase3Data1;
        internal static Vector2 Phase3Data2;
        internal static Vector2 Phase3Data3;
        internal static Vector2 Phase3Data4;

        private static int[] types = new int[6];
        internal static Texture2D BulletWhiteTexture;
        internal static Texture2D BulletGoldTexture;
        internal static Texture2D BulletGoldLargeTexture;
        internal static Texture2D BulletStarTexture;
        internal static Texture2D BulletPurpleTexture;
        internal static Texture2D BulletBlackTexture;
        internal static Texture2D BulletBlueTexture;
        internal static Texture2D BulletBlueLargeTexture;
        internal static Texture2D BulletBlueSmallTexture;
        internal static Texture2D BulletBoxBlueTexture;
        internal static Texture2D BulletFireLargeTexture;
        internal static Texture2D BulletFireTexture;
        internal static Texture2D[] BulletColorTextures;
        internal static Texture2D BulletLightTexture;
        internal static Texture2D BulletDragonTexture;
        internal static Texture2D BulletDragonBreathTexture;
        internal static Texture2D BulletSkullTexture;
        internal static Texture2D BulletBoneTexture;
        internal static Texture2D CrystalStarTexture;
        internal static Texture2D BulletDragonLargeTexture;
        internal static Texture2D BulletDragonDiamondTexture;

        public static bool Active
        {
            get
            {
                return Phase > 0;
            }
        }

        internal static Func<int> difficultyOverride;
        public static int Difficulty
        {
            get
            {
                if (difficultyOverride != null)
                {
                    int temp = difficultyOverride();
                    if (temp < 1)
                    {
                        temp = 1;
                    }
                    if (temp > 4)
                    {
                        temp = 4;
                    }
                    return temp;
                }
                return Main.expertMode ? 2 : 1;
            }
        }

        internal static void Load()
        {
            types[0] = Bluemagic.Instance.NPCType("Blushiemagic");
            types[1] = Bluemagic.Instance.NPCType("BlushiemagicK");
            types[2] = Bluemagic.Instance.NPCType("BlushiemagicA");
            types[3] = Bluemagic.Instance.NPCType("BlushiemagicL");
            types[4] = Bluemagic.Instance.NPCType("BlushiemagicM");
            types[5] = Bluemagic.Instance.NPCType("BlushiemagicJ");
            BulletWhiteTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletWhite");
            BulletGoldTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletGold");
            BulletGoldLargeTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletGoldLarge");
            BulletStarTexture = Bluemagic.Instance.GetTexture("BlushieBoss/Star");
            BulletPurpleTexture = Bluemagic.Instance.GetTexture("BlushieBoss/LightningOrb");
            BulletBlackTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletBlack");
            BulletBlueTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletBlue");
            BulletBlueLargeTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletBlueLarge");
            BulletBlueSmallTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletBlueSmall");
            BulletBoxBlueTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletBoxBlue");
            BulletFireLargeTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletFireLarge");
            BulletFireTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletFire");
            BulletColorTextures = new Texture2D[6];
            BulletColorTextures[0] = Bluemagic.Instance.GetTexture("BlushieBoss/BulletRed");
            BulletColorTextures[1] = Bluemagic.Instance.GetTexture("BlushieBoss/BulletOrange");
            BulletColorTextures[2] = BulletGoldTexture;
            BulletColorTextures[3] = Bluemagic.Instance.GetTexture("BlushieBoss/BulletGreenLight");
            BulletColorTextures[4] = Bluemagic.Instance.GetTexture("BlushieBoss/BulletBlueLight");
            BulletColorTextures[5] = Bluemagic.Instance.GetTexture("BlushieBoss/BulletPurple");
            BulletLightTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletLight");
            BulletDragonTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletDragon");
            BulletDragonBreathTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletDragonBreath");
            BulletSkullTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletSkull");
            BulletBoneTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletBone");
            BulletDragonLargeTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletDragonLarge");
            BulletDragonDiamondTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BulletDragonDiamond");
            CrystalStarTexture = Bluemagic.Instance.GetTexture("BlushieBoss/CrystalStar");
        }

        internal static void Unload()
        {
            BulletWhiteTexture = null;
            BulletGoldTexture = null;
            BulletGoldLargeTexture = null;
            BulletStarTexture = null;
            BulletPurpleTexture = null;
            BulletBlackTexture = null;
            BulletBlueTexture = null;
            BulletBlueLargeTexture = null;
            BulletBlueSmallTexture = null;
            BulletBoxBlueTexture = null;
            BulletFireLargeTexture = null;
            BulletFireTexture = null;
            for (var k = 0; k < BulletColorTextures.Length; k++)
            {
                BulletColorTextures[k] = null;
            }
            BulletLightTexture = null;
            BulletDragonTexture = null;
            BulletDragonBreathTexture = null;
            BulletSkullTexture = null;
            BulletBoneTexture = null;
            BulletDragonLargeTexture = null;
            BulletDragonDiamondTexture = null;
            CrystalStarTexture = null;
        }

        internal static void Update()
        {
            bool anyBlushie = false;
            for (int k = 0; k < index.Length; k++)
            {
                index[k] = NPC.FindFirstNPC(types[k]);
                if (index[k] > -1 && Main.npc[index[k]].timeLeft < 600)
                {
                    Main.npc[index[k]].timeLeft = 6000;
                }
                for (int i = 0; i < 200; i++)
                {
                    if (i != index[k] && Main.npc[i].active && Main.npc[i].type == types[k])
                    {
                        Main.npc[i].active = false;
                    }
                }
                if (index[k] > -1)
                {
                    anyBlushie = true;
                }
            }
            if (!Active && index[0] > -1 && Main.netMode != 1)
            {
                Initialize();
            }
            if (Active && !anyBlushie)
            {
                Reset();
            }
            if (Active)
            {
                SkyManager.Instance.Activate("Bluemagic:BlushieBoss");
            }
            else
            {
                SkyManager.Instance.Deactivate("Bluemagic:BlushieBoss");
            }
            for (int k = 0; k < 255; k++)
            {
                if (!Main.player[k].active || Main.player[k].dead)
                {
                    Players[k] = false;
                }
            }
            if (Immune > 0)
            {
                Immune--;
            }
            if (Active)
            {
                Timer++;
                if (Phase == 1)
                {
                    Phase1();
                }
                else if (Phase == 2)
                {
                    Phase2();
                }
                else if (Phase == 3)
                {
                    Phase3();
                }
                Player player = Main.player[GetTarget()];
                if (Players[player.whoAmI])
                {
                    player.GetModPlayer<BluemagicPlayer>().BlushieBarrier();
                }
                for (int k = 0; k < bullets.Count; k++)
                {
                    bullets[k].Update();
                    if (bullets[k].ShouldRemove())
                    {
                        bullets[k].Active = false;
                        bullets.RemoveAt(k);
                        k--;
                    }
                    else if (bullets[k].Damage > 0f && Vector2.Distance(player.Center, bullets[k].Position) < bullets[k].Size)
                    {
                        player.GetModPlayer<BluemagicPlayer>().BlushieDamage(bullets[k].Damage);
                    }
                }
                for (int k = 0; k < crystalStars.Count; k++)
                {
                    if (player.Hitbox.Intersects(new Rectangle((int)crystalStars[k].X - 32, (int)crystalStars[k].Y - 32, 64, 64)))
                    {
                        crystalStars.RemoveAt(k);
                        Main.PlaySound(SoundID.Item25);
                        k--;
                    }
                }
            }
            bool allDead = true;
            for (int k = 0; k < 255; k++)
            {
                if (Players[k])
                {
                    allDead = false;
                    break;
                }
            }
            if (allDead)
            {
                if (Main.netMode != 1 && Phase == 1)
                {
                    BlushieTalk("Done already? Well, I hope we can fight again soon!");
                }
                Reset();
            }
        }

        internal static void Initialize()
        {
            Phase = 1;
            Timer = 0;
            Phase3Attack = 0;
            HealthK = 1000000;
            HealthA = 1000000;
            HealthL = 1000000;
            ShieldK = 0;
            ShieldA = 0;
            ShieldL = 0;
            Origin = Main.npc[index[0]].Center;
            for (int k = 0; k < 255; k++)
            {
                Player player = Main.player[k];
                Players[k] = player.active && !player.dead && player.position.X >= Origin.X - ArenaSize && player.position.X + player.width <= Origin.X + ArenaSize && player.position.Y >= Origin.Y - ArenaSize && player.position.Y + player.height <= Origin.Y + ArenaSize;
                if (Players[k])
                {
                    Main.player[k].GetModPlayer<BluemagicPlayer>().blushieHealth = 1f;
                }
            }
        }

        internal static void InitializeCheckpoint()
        {
            Phase = 3;
            Timer = 0;
            Phase3Attack = 0;
            index[4] = NPC.FindFirstNPC(types[4]);
            Origin = Main.npc[index[4]].Center;
            DragonPos = Origin + new Vector2(0f, -ArenaSize * 0.6f);
            ArmLeftPos = Origin + new Vector2(ArenaSize * 0.6f, 0f);
            ArmRightPos = Origin + new Vector2(-ArenaSize * 0.6f, 0f);
            SkullPos = Origin + new Vector2(0f, ArenaSize * 0.6f);
            BoneLTPos = SkullPos + new Vector2(-ArenaSize * 0.3f - 100f, 100f);
            BoneLBPos = SkullPos + new Vector2(-ArenaSize * 0.3f - 100f, -100f);
            BoneRTPos = SkullPos + new Vector2(ArenaSize * 0.3f + 100f, 100f);
            BoneRBPos = SkullPos + new Vector2(ArenaSize * 0.3f + 100f, -100f);
            BoneLTRot = -MathHelper.Pi / 4f;
            BoneLBRot = MathHelper.Pi / 4f;
            BoneRTRot = MathHelper.Pi / 4f;
            BoneRBRot = -MathHelper.Pi / 4f;
            for (int k = 0; k < 255; k++)
            {
                Player player = Main.player[k];
                Players[k] = player.active && !player.dead && player.position.X >= Origin.X - ArenaSize && player.position.X + player.width <= Origin.X + ArenaSize && player.position.Y >= Origin.Y - ArenaSize && player.position.Y + player.height <= Origin.Y + ArenaSize;
                if (Players[k])
                {
                    Main.player[k].GetModPlayer<BluemagicPlayer>().blushieHealth = BluemagicWorld.blushieCheckpoint;
                }
            }
        }

        internal static void Reset()
        {
            Phase = 0;
            Timer = 0;
            CameraFocus = false;
            HealthK = 1000000;
            HealthA = 1000000;
            HealthL = 1000000;
            ShieldK = 0;
            ShieldA = 0;
            ShieldL = 0;
            for (int k = 0; k < 255; k++)
            {
                Players[k] = false;
            }
            for (int k = 0; k < index.Length; k++)
            {
                if (index[k] > -1)
                {
                    Main.npc[index[k]].active = false;
                }
            }
            bullets.Clear();
            crystalStars.Clear();
        }

        private static void Phase1()
        {
            NPC npc = Main.npc[index[0]];
            if (Main.netMode != 1)
            {
                bool flag = false;
                for (int k = 0; k < 255; k++)
                {
                    if (Players[k] && Main.player[k].GetModPlayer<BluemagicPlayer>().triedGodmode)
                    {
                        flag = true;
                        break;
                    }
                }
                if (Timer == 1)
                {
                    Music ("Music - Shelter - by Phyrnna");
                }
                if (flag && Timer == 120)
                {
                    BlushieTalk("You're using the Rainbow Star?");
                }
                if (flag && Timer == 240)
                {
                    BlushieTalk("I hope you know you can't cheese me with an item I made...");
                }
                if (Timer == 360)
                {
                    BlushieTalk("Hi there! I hope you enjoyed tModLoader!");
                }
                if (Timer == 480)
                {
                    BlushieTalk("Are you ready for your final challenge?");
                }
                if (Timer == 2040)
                {
                    BlushieTalk("\x300cAstral Collision\x300d");
                }
                if (Timer == 3480)
                {
                    BlushieTalk("I'm... so tired...");
                }
                if (Timer == 3600)
                {
                    BlushieTalk("Sorry for such a disappointing fight T-T");
                }
            }
            npc.Center = Origin + new Vector2(0f, 16f * (float)Math.Sin(Timer / 60f));
            npc.velocity = Vector2.Zero;
            if (Main.netMode != 2)
            {
                if (Timer >= 600 && Timer <= 1200 && Timer % 30 == 0)
                {
                    Vector2 target = Main.player[GetTarget()].Center;
                    Vector2 offset = target - Origin;
                    float length = offset.Length();
                    offset.Normalize();
                    float rotate = offset.ToRotation();
                    int num = 16 * Difficulty;
                    for (int k = 0; k < num; k++)
                    {
                        float useRotate = rotate + k * MathHelper.TwoPi / num;
                        AddBullet(BulletSimple.NewWhite(Origin, length / 90f * useRotate.ToRotationVector2()));
                    }
                    num = 4 * Difficulty;
                    for (int k = 0; k < num; k++)
                    {
                        float useRotate = rotate + k * MathHelper.TwoPi / num;
                        Vector2 direction = useRotate.ToRotationVector2();
                        Vector2 center = Origin + length / 2 * direction;
                        AddBullet(BulletRotate.NewGold(center, length / 2f, useRotate + MathHelper.Pi, MathHelper.TwoPi / 180f, 180));
                        AddBullet(BulletRotate.NewGold(center, length / 2f, useRotate + MathHelper.Pi, -MathHelper.TwoPi / 180f, 180));
                    }
                }
                if (Timer >= 1320 && Timer <= 1920 && Timer % (120 / (Difficulty + 1)) == 0)
                {
                    Bullet center = BulletTarget.NewGoldLarge(Origin, Main.player[GetTarget()].Center, 60);
                    AddBullet(center);
                    int num = 8;
                    for (int k = 0; k < num; k++)
                    {
                        float rotate = k / (float)num * MathHelper.TwoPi;
                        AddBullet(BulletRelease.NewWhite(center, 60f, rotate, MathHelper.TwoPi / 480f, 4f));
                        AddBullet(BulletRelease.NewWhite(center, 60f, rotate, -MathHelper.TwoPi / 480f, 4f));
                    }
                }
                if (Timer >= 2040 && Timer <= 2640 && Timer % 90 == 0)
                {
                    Vector2 offset = Main.player[GetTarget()].Center - Origin;
                    offset.Normalize();
                    offset *= 6f;
                    for (int k = 0; k < 5; k++)
                    {
                        float angle = MathHelper.Pi * 1.5f - k * MathHelper.TwoPi / 5f;
                        float nextAngle = MathHelper.Pi * 1.5f - (k + 2) * MathHelper.TwoPi / 5f;
                        Vector2 start = angle.ToRotationVector2();
                        Vector2 end = nextAngle.ToRotationVector2();
                        int num = 6;
                        for (int i = 0; i < num; i++)
                        {
                            if (Difficulty == 1 && (i == num / 2 - 1 || i == num / 2))
                            {
                                continue;
                            }
                            Vector2 adjust = Vector2.Lerp(start, end, i / (float)num);
                            AddBullet(BulletBounce.NewStar(Origin, offset + 0.75f * adjust, 5));
                        }
                    }
                }
            }
            if (Timer >= 3600)
            {
                npc.dontTakeDamage = false;
            }
            else
            {
                npc.life = npc.lifeMax;
            }
        }

        internal static void StartPhase2()
        {
            Phase = 2;
            Timer = 0;
            index[4] = NPC.NewNPC((int)Origin.X, (int)Origin.Y, types[4]);
            Main.npc[index[4]].Center = Origin;
            BlushieC = Main.rand.Next(10) == 0;
        }

        internal static void Phase2()
        {
            NPC megan = Main.npc[index[4]];
            if (Main.netMode != 1)
            {
                if (Timer == 1)
                {
                    BlushieTalk("Woah... what's happening to me?");
                }
                if (Timer == 100)
                {
                    Music("Music - Return of the Snow Queen - by Phyrnna, for Epic Battle Fantasy 5");
                }
                if (Timer == 180)
                {
                    int x = (int)megan.Bottom.X;
                    int y = (int)megan.Bottom.Y;
                    for (int k = 1; k <= 3; k++)
                    {
                        index[k] = NPC.NewNPC(x, y, types[k], index[4]);
                    }
                    PosK = Vector2.Zero;
                    PosA = Vector2.Zero;
                    PosL = Vector2.Zero;
                    KylieTalk("Why am I here? I'm useless...");
                    if (BlushieC)
                    {
                        ChrisTalk("blushiemagic (A) wasn't able to make it this time. But I can fight you in her place!");
                    }
                    else
                    {
                        AnnaTalk("Hi there! Are you ready to have fun?~");
                    }
                    LunaTalk("You think you can stand up to me? We shall see...");
                }
                if (Timer == 1800)
                {
                    if (BlushieC)
                    {
                        ChrisTalk("\x300cRay of Burning Light\x300d");
                    }
                    else
                    {
                        AnnaTalk("\x300cRay of Blinding Light\x300d!~");
                    }
                }
                if (Timer == 2640)
                {
                    KylieTalk("\x300cImprisoning Mirrors\x300d");
                }
                if (Timer == 2040)
                {
                    LunaTalk("\x300cShadow Vortex\x300d");
                }
            }
            if (Timer < 180)
            {
                return;
            }
            NPC kylie = index[1] > -1 ? Main.npc[index[1]] : null;
            NPC anna = index[2] > -1 ? Main.npc[index[2]] : null;
            NPC luna = index[3] > -1 ? Main.npc[index[3]] : null;
            if (Main.netMode != 1)
            {
                if (kylie != null && kylie.localAI[1] == 0f && (ShieldBuff(kylie) || ShieldBuff(anna) || ShieldBuff(luna)))
                {
                    KylieTalk("Oh, um, I can create shields around us to protect us from damage. Yeah...");
                    kylie.localAI[1] = 1f;
                }
                if (luna != null && luna.localAI[1] == 0f && (DamageBuff(kylie) || DamageBuff(anna) || DamageBuff(luna)))
                {
                    LunaTalk("Impressive. You still live. But how about if I buff our damage?");
                    luna.localAI[1] = 1f;
                }
                if (anna != null && anna.localAI[1] == 0f && (HealBuff(kylie) || HealBuff(anna) || HealBuff(luna)))
                {
                    if (BlushieC)
                    {
                        ChrisTalk("Uh oh, looks like we're in need of some healing!");
                    }
                    else
                    {
                        AnnaTalk("You're not gonna defeat any of us on my watch! >:( Magic healing powers, go!");
                    }
                    anna.localAI[1] = 1f;
                }
                if (kylie == null && luna == null && anna == null)
                {
                    StartPhase3();
                }
            }
            if (Timer >= 180 && Timer < 600)
            {
                float distance = (Timer - 180) / 300f;
                if (distance > 1f)
                {
                    distance = 1f;
                }
                distance *= ArenaSize * 0.75f;
                PosK = new Vector2(0f, -distance);
                PosA = distance * (MathHelper.Pi * 5f / 6f).ToRotationVector2();
                PosL = distance * (MathHelper.Pi * 1f / 6f).ToRotationVector2();
            }
            if (Timer >= 480)
            {
                megan.Center = Origin + new Vector2(0f, 16f * (float)Math.Sin((Timer - 480f) / 60f));
            }
            if (Timer >= 600)
            {
                float kVal = ((Timer - 600) / 3600f * 4f + 0.5f) % 4f;
                float distance = ArenaSize * 0.75f;
                if (kVal < 1f)
                {
                    PosK = Vector2.Lerp(new Vector2(-distance, -distance), new Vector2(distance, -distance), kVal);
                }
                else if (kVal < 2f)
                {
                    PosK = Vector2.Lerp(new Vector2(distance, -distance), new Vector2(distance, distance), kVal - 1f);
                }
                else if (kVal < 3f)
                {
                    PosK = Vector2.Lerp(new Vector2(distance, distance), new Vector2(-distance, distance), kVal - 2f);
                }
                else
                {
                    PosK = Vector2.Lerp(new Vector2(-distance, distance), new Vector2(-distance, -distance), kVal - 3f);
                }
                float theta = (Timer - 600) * MathHelper.TwoPi / 1800f;
                float r = (float)Math.Cos(1.8f * theta);
                PosA = ArenaSize * 0.75f * r * (MathHelper.Pi * 5f / 6f - theta).ToRotationVector2();
                float rot = MathHelper.Pi / 6f - ((Timer - 600) % 3600) * MathHelper.TwoPi / 3600f;
                PosL = ArenaSize * 0.75f * rot.ToRotationVector2();
            }
            PosK += Origin;
            PosA += Origin;
            PosL += Origin;
            if (kylie != null)
            {
                kylie.Center = PosK;
            }
            if (anna != null)
            {
                anna.Center = PosA;
            }
            if (luna != null)
            {
                luna.Center = PosL;
            }
            if (Main.netMode != 2 && Timer >= 600)
            {
                if (kylie != null)
                {
                    KylieAttack();
                    kylie.dontTakeDamage = false;
                    if (HealBuff(kylie))
                    {
                        HealthK += 123;
                    }
                    kylie.life = HealthK;
                    if (ShieldK < 300)
                    {
                        ShieldK++;
                    }
                }
                if (anna != null)
                {
                    AnnaAttack();
                    anna.dontTakeDamage = false;
                    if (HealBuff(anna))
                    {
                        HealthA += 123;
                    }
                    anna.life = HealthA;
                    if (ShieldA < 300)
                    {
                        ShieldA++;
                    }
                }
                if (luna != null)
                {
                    LunaAttack();
                    luna.dontTakeDamage = false;
                    if (HealBuff(luna))
                    {
                        HealthL += 123;
                    }
                    luna.life = HealthL;
                    if (ShieldL < 300)
                    {
                        ShieldL++;
                    }
                }
            }
        }

        private static void KylieAttack()
        {
            float damage = DamageBuff(Main.npc[index[1]]) ? 1.5f : 1f;
            int numCycles = Phase2Count() == 1 ? 2 : 1;
            int timerK = (Timer - 600) % 2820;
            for (int i = 0; i < numCycles; i++)
            {
                if (timerK < 840 && timerK % (120 / Difficulty) == 0)
                {
                    for (int k = 0; k < 8; k++)
                    {
                        float angle = k / 8f * MathHelper.TwoPi;
                        AddBullet(new BulletRotateKylie(angle, MathHelper.TwoPi / 300f), damage);
                        AddBullet(new BulletRotateKylie(angle, -MathHelper.TwoPi / 300f), damage);
                    }
                }
                if (timerK >= 1140 && timerK < 1860 && timerK % 90 == 60)
                {
                    var bullet = BulletTargetSmooth.NewBlueLarge(PosK, Main.player[GetTarget()].Center, 450);
                    AddBullet(bullet, damage);
                    int num = (int)(32 * Math.Sqrt(Difficulty));
                    for (int k = 0; k < num; k++)
                    {
                        float rot = MathHelper.TwoPi * k / (float)num;
                        AddBullet(BulletRotateAround.NewBlueSmall(bullet, 80f * (float)Math.Sqrt(Difficulty), rot, MathHelper.TwoPi / 120f), damage);
                    }
                }
                if (timerK >= 2040 && timerK < 2640)
                {
                    if (timerK % 60 == 0)
                    {
                        Vector2 uv = Main.player[GetTarget()].Center - Origin;
                        uv.X -= (float)Math.Floor(uv.X / 400f) * 400f;
                        uv.Y -= (float)Math.Floor(uv.Y / 400f) * 400f;
                        DataK = Origin - new Vector2(ArenaSize) + uv;
                    }
                    int spacing = 8;
                    if (Difficulty > 1)
                    {
                        spacing = 4;
                    }
                    if (timerK % spacing == 0)
                    {
                        for (float x = DataK.X; x <= Origin.X + ArenaSize; x += 400f)
                        {
                            AddBullet(BulletSimple.NewBoxBlue(new Vector2(x, Origin.Y - ArenaSize), new Vector2(0f, 8f)), damage);
                        }
                        for (float y = DataK.Y; y <= Origin.Y + ArenaSize; y += 400f)
                        {
                            AddBullet(BulletSimple.NewBoxBlue(new Vector2(Origin.X - ArenaSize, y), new Vector2(8f, 0f)), damage);
                        }
                    }
                }
                timerK = (timerK + 1410) % 2820;
            }
        }

        private static void AnnaAttack()
        {
            float damage = DamageBuff(Main.npc[index[2]]) ? 1.5f : 1f;
            int numCycles = Phase2Count() == 1 ? 2 : 1;
            int timerA = (Timer - 600) % 1800;
            for (int i = 0; i < numCycles; i++)
            {
                if (timerA < 450 && timerA % 90 == 0)
                {
                    if (Difficulty == 1)
                    {
                        AddBullet(new BulletFireBomb(Main.player[GetTarget()].Center, 120, damage));
                    }
                    else
                    {
                        AddBullet(new BulletFireBombDouble(Main.player[GetTarget()].Center, 120, 45, damage));
                    }
                }
                if (timerA >= 600 && timerA < 1080 && timerA % (12 / Difficulty) == 0)
                {
                    float baseRot = (float)Math.Sin(timerA * MathHelper.TwoPi / 120f);
                    baseRot *= MathHelper.Pi / 6f;
                    for (int k = 0; k < 6; k++)
                    {
                        float rot = baseRot + MathHelper.TwoPi * k / 6f;
                        int color = (k + (timerA / (12 / Difficulty))) % 6;
                        AddBullet(BulletSimple.NewColor(PosA, 6f * rot.ToRotationVector2(), color), damage);
                    }
                }
                float raySize = 80f * Difficulty;
                if (timerA == 1200)
                {
                    DataA = Main.player[GetTarget()].Center.X - raySize;
                }
                if (timerA >= 1200 && timerA < 1680 && timerA % (2 / Difficulty) == 0)
                {
                    float progress = (timerA - 1200) / 480f;
                    int count = 1 + (int)(progress * 5f);
                    for (int k = 0; k < progress; k++)
                    {
                        AddBullet(BulletSimple.NewLight(new Vector2(DataA + 2 * raySize * Main.rand.NextFloat(), Origin.Y + ArenaSize), new Vector2(0f, -4f - progress * 16f)), damage);
                    }
                }
                if (timerA == 1680)
                {
                    Main.PlaySound(29, -1, -1, 104);
                }
                if (timerA >= 1680 && timerA < 1740)
                {
                    for (int k = 0; k < 20 * Difficulty; k++)
                    {
                        var bullet = BulletSimple.NewLight(new Vector2(DataA + 2 * raySize * Main.rand.NextFloat(), Origin.Y - ArenaSize - 32f), new Vector2(0f, 32f));
                        bullet.Damage = 0.2f;
                        AddBullet(bullet, damage);
                        bullet = BulletSimple.NewLight(new Vector2(DataA + 2 * raySize * Main.rand.NextFloat(), Origin.Y - ArenaSize - 16f), new Vector2(0f, 32f));
                        bullet.Damage = 0.2f;
                        AddBullet(bullet, damage);
                    }
                }
                timerA = (timerA + 900) % 1800;
            }
        }

        private static void LunaAttack()
        {
            float damage = DamageBuff(Main.npc[index[3]]) ? 1.5f : 1f;
            int numCycles = Phase2Count() == 1 ? 2 : 1;
            int timerL = (Timer - 600) % 2200;
            for (int i = 0; i < numCycles; i++)
            {
                if (timerL < 600)
                {
                    if (timerL % 32 == 0)
                    {
                        int num = 2 * Difficulty;
                        for (int k = 0; k < num; k++)
                        {
                            AddBullet(new BulletRotateLuna(1f, k * MathHelper.TwoPi / num), damage);
                        }
                    }
                    else if (timerL % 32 == 16)
                    {
                        int num = 2 * Difficulty;
                        for (int k = 0; k < num; k++)
                        {
                            AddBullet(new BulletRotateLuna(-1f, (k + 0.5f) * MathHelper.TwoPi / num), damage);
                        }
                    }
                }
                if (timerL >= 780 && timerL < 1260)
                {
                    if (timerL % 120 == 60)
                    {
                        DataL = PosL;
                        Vector2 dir = Main.player[GetTarget()].Center - PosL;
                        DataL2 = dir.ToRotation();
                    }
                    if (timerL % 120 >= 60 && timerL % 120 < 90)
                    {
                        float normal = DataL2 + MathHelper.PiOver2;
                        Vector2 pos = DataL + (32f * Main.rand.NextFloat() - 16f) * normal.ToRotationVector2();
                        AddBullet(new BulletLightning(pos, 10f * DataL2.ToRotationVector2()), damage);
                    }
                    if (Difficulty > 1 && timerL % 120 >= 60 && timerL % 120 < 75)
                    {
                        float normal = DataL2 + MathHelper.PiOver2;
                        Vector2 pos = DataL + (32f * Main.rand.NextFloat() - 16f) * normal.ToRotationVector2();
                        AddBullet(new BulletLightning(pos, 10f * -DataL2.ToRotationVector2()), damage);
                    }
                }
                if (timerL >= 1440 && timerL < 2080)
                {
                    float rot = timerL;
                    if (Difficulty == 1 || Difficulty >= 4)
                    {
                        if (timerL % 2 == 0 || Difficulty >= 4)
                        {
                            AddBullet(new BulletPull(PosL + 1600f * (float)Math.Sqrt(2) * rot.ToRotationVector2()), damage);
                        }
                    }
                    else
                    {
                        float useRot = rot;
                        float useDistance = 1600f * (float)Math.Sqrt(2);
                        if (timerL % 4 == 0)
                        {
                            AddBullet(new BulletPull(PosL + useDistance * useRot.ToRotationVector2()), damage);
                        }
                        if (timerL % 4 == 1)
                        {
                            useRot += 1f / 3f;
                            useDistance += 2f;
                            AddBullet(new BulletPull(PosL + useDistance * useRot.ToRotationVector2()), damage);
                        }
                        if (timerL % 4 == 3)
                        {
                            useRot -= 1f / 3f;
                            useDistance -= 2f;
                            AddBullet(new BulletPull(PosL + useDistance * useRot.ToRotationVector2()), damage);
                        }
                    }
                }
                timerL = (timerL + 1100) % 2200;
            }
        }

        internal static int Phase2Count()
        {
            int count = 0;
            for (int k = 1; k <= 3; k++)
            {
                if (index[k] > -1)
                {
                    count++;
                }
            }
            return count;
        }

        internal static bool ShieldBuff(NPC npc)
        {
            int count = Phase2Count();
            return index[1] > -1 && npc.life <= 1500000 - 250000 * count;
        }

        internal static bool DamageBuff(NPC npc)
        {
            int count = Phase2Count();
            return index[3] > -1 && npc.life <= 1250000 - 250000 * count;
        }

        internal static bool HealBuff(NPC npc)
        {
            int count = Phase2Count();
            return index[2] > -1 && npc.life <= 1000000 - 250000 * count;
        }

        internal static void StartPhase3()
        {
            Phase = 3;
            Timer = 0;
            DragonPos = Origin + new Vector2(0f, -ArenaSize * 0.6f);
            ArmLeftPos = Origin + new Vector2(ArenaSize * 0.6f, 0f);
            ArmRightPos = Origin + new Vector2(-ArenaSize * 0.6f, 0f);
            SkullPos = Origin + new Vector2(0f, ArenaSize * 0.6f);
            BoneLTPos = SkullPos + new Vector2(-ArenaSize * 0.3f - 100f, 100f);
            BoneLBPos = SkullPos + new Vector2(-ArenaSize * 0.3f - 100f, -100f);
            BoneRTPos = SkullPos + new Vector2(ArenaSize * 0.3f + 100f, 100f);
            BoneRBPos = SkullPos + new Vector2(ArenaSize * 0.3f + 100f, -100f);
            BoneLTRot = -MathHelper.Pi / 4f;
            BoneLBRot = MathHelper.Pi / 4f;
            BoneRTRot = MathHelper.Pi / 4f;
            BoneRBRot = -MathHelper.Pi / 4f;
            Phase3Attack = 0;
            for (int k = 0; k < 255; k++)
            {
                if (Players[k])
                {
                    BluemagicPlayer modPlayer = Main.player[k].GetModPlayer<BluemagicPlayer>();
                    modPlayer.blushieHealth += 0.5f;
                    if (k == Main.myPlayer)
                    {
                        Main.NewText("You gained 50% of your health back!");
                    }
                    if (modPlayer.blushieHealth > BluemagicWorld.blushieCheckpoint)
                    {
                        BluemagicWorld.blushieCheckpoint = modPlayer.blushieHealth;
                    }
                }
            }
            Item.NewItem((int)Origin.X, (int)(Origin.Y + ArenaSize * 0.3f), 0, 0, Bluemagic.Instance.ItemType("BlushieCheckpoint"));
        }

        internal static void Phase3()
        {
            NPC megan = Main.npc[index[4]];
            megan.Center = Origin + new Vector2(0f, 16f * (float)Math.Sin((Timer - 480f) / 60f));
            if (Main.netMode != 2)
            {
                if (Timer == 840)
                {
                    Main.PlaySound(29, -1, -1, 92, 1f, 0f);
                }
                else if (Timer == 960)
                {
                    Main.PlaySound(29, -1, -1, 104);
                }
                if (Timer >= 600 && Timer < 780)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        int dust = Dust.NewDust(SkullPos - new Vector2(80f, 80f), 160, 160, Bluemagic.Instance.DustType("Smoke"), 0f, 0f, 0, Color.Black);
                        Main.dust[dust].scale = 2.5f;
                        Main.dust[dust].noLight = true;
                    }
                }
                if ((Phase3Attack < 8 && Timer >= 780) || (Phase3Attack == 8 && Timer < 2420))
                {
                    for (int k = 0; k < 1; k++)
                    {
                        int dust = Dust.NewDust(SkullPos - new Vector2(80f, 80f) + new Vector2(26f, 58f), 36, 16, Bluemagic.Instance.DustType("Smoke"), 0f, 0f, 0, Color.Black);
                        Main.dust[dust].scale = 2.5f;
                        Main.dust[dust].noLight = true;
                        dust = Dust.NewDust(SkullPos - new Vector2(80f, 80f) + new Vector2(98f, 58f), 36, 16, Bluemagic.Instance.DustType("Smoke"), 0f, 0f, 0, Color.Black);
                        Main.dust[dust].scale = 2.5f;
                        Main.dust[dust].noLight = true;
                    }
                }
                if (Phase3Attack == 8 && Timer == 2420)
                {
                    for (int k = 0; k < 300; k++)
                    {
                        int dust = Dust.NewDust(SkullPos - new Vector2(80f, 80f), 160, 160, Bluemagic.Instance.DustType("Smoke"), 0f, 0f, 0, Color.Black);
                        Main.dust[dust].scale = 2.5f;
                        Main.dust[dust].noLight = true;
                    }
                }
            }
            if (Main.netMode != 1)
            {
                if (Timer == 100)
                {
                    Music("Music - Fallen Blood - by Phyrnna, for Epic Battle Fantasy 4");
                }
                if (Timer == 300)
                {
                    MeganTalk("I'm... all alone again...");
                }
                if (Timer == 600)
                {
                    JoyceTalk("At long last... freedom!");
                }
                if (Timer == 660)
                {
                    MeganTalk("Who... was that?");
                }
                if (Timer == 780)
                {
                    index[5] = NPC.NewNPC((int)SkullPos.X, (int)SkullPos.Y + 80, types[5], index[4]);
                    JoyceTalk("I would thank you, player, but you shall be the first thing I destroy with my newfound power.");
                }
            }
            if (Timer >= 780 && index[5] > -1)
            {
                Main.npc[index[5]].Bottom = SkullPos + new Vector2(0f, 80f);
                Main.npc[index[5]].realLife = index[4];
            }
            if (Timer >= 900 && Timer < 960)
            {
                BoneLTRot -= MathHelper.TwoPi / 30f;
                BoneLBRot += MathHelper.TwoPi / 30f;
                BoneRTRot += MathHelper.TwoPi / 30f;
                BoneRBRot -= MathHelper.TwoPi / 30f;
            }
            if (Timer == 960)
            {
                BoneLTRot = -MathHelper.Pi / 4f;
                BoneLBRot = MathHelper.Pi / 4f;
                BoneRTRot = MathHelper.Pi / 4f;
                BoneRBRot = -MathHelper.Pi / 4f;
            }
            if (Timer >= 960 && Timer <= 990)
            {
                BoneLTPos += new Vector2(100f / 30f, -100f / 30f);
                BoneLBPos += new Vector2(100f / 30f, 100f / 30f);
                BoneRTPos += new Vector2(-100f / 30f, -100f / 30f);
                BoneRBPos += new Vector2(-100f / 30f, 100f / 30f);
            }
            if (Timer == 990)
            {
                BoneLTPos = SkullPos + new Vector2(-ArenaSize * 0.3f, 0f);
                BoneLBPos = SkullPos + new Vector2(-ArenaSize * 0.3f, 0f);
                BoneRTPos = SkullPos + new Vector2(ArenaSize * 0.3f, 0f);
                BoneRBPos = SkullPos + new Vector2(ArenaSize * 0.3f, 0f);
            }
            if (Timer == 1320)
            {
                Timer = 2000;
                Phase3Attack = 1;
                SpawnedStars = false;
            }
            if (Timer >= 2000)
            {
                bool dontTakeDamage = !SpawnedStars || crystalStars.Count > 0;
                Main.npc[index[4]].dontTakeDamage = dontTakeDamage;
                Main.npc[index[5]].dontTakeDamage = dontTakeDamage;
                if (dontTakeDamage)
                {
                    Main.npc[index[4]].life = Main.npc[index[4]].lifeMax;
                }
                switch (Phase3Attack)
                {
                case 1:
                    Phase3_1(Timer - 2000);
                    break;
                case 2:
                    Phase3_2(Timer - 2000);
                    break;
                case 3:
                    Phase3_3(Timer - 2000);
                    break;
                case 4:
                    Phase3_4(Timer - 2000);
                    break;
                case 5:
                    Phase3_5(Timer - 2000);
                    break;
                case 6:
                    Phase3_6(Timer - 2000);
                    break;
                case 7:
                    Phase3_7(Timer - 2000);
                    break;
                case 8:
                    Phase3_8(Timer - 2000);
                    break;
                case 9:
                    Phase3_9(Timer - 2000);
                    break;
                }
            }
        }

        private static void Phase3_1(int timer)
        {
            if (timer == 150)
            {
                AddCrystalStar(new Vector2(-0.8f, -0.8f));
                AddCrystalStar(new Vector2(0.8f, -0.8f));
                AddCrystalStar(new Vector2(-0.8f, 0.8f));
                AddCrystalStar(new Vector2(0.8f, 0.8f));
                if (Main.netMode != 2)
                {
                    Main.NewText("Collect the stars in order to damage the two blushiemagics!");
                }
                SpawnedStars = true;
            }
            if (timer < 120)
            {
                Vector2 target1 = Origin + new Vector2(0f, -ArenaSize * 0.9f);
                Vector2 target2 = Origin + new Vector2(0f, ArenaSize * 0.9f);
                Vector2 target3 = Origin + new Vector2(ArenaSize * 0.9f, 0f);
                Vector2 target4 = Origin + new Vector2(-ArenaSize * 0.9f, 0f);
                if (timer == 0)
                {
                    Phase3Data1 = BoneLTPos;
                    Phase3Data2 = BoneRTPos;
                    Phase3Data3 = ArmLeftPos;
                    Phase3Data4 = ArmRightPos;
                }
                BoneLTPos = Vector2.Lerp(Phase3Data1, target1, timer / 120f);
                BoneRTPos = Vector2.Lerp(Phase3Data2, target2, timer / 120f);
                BoneLBPos = BoneLTPos;
                BoneRBPos = BoneRTPos;
                ArmLeftPos = Vector2.Lerp(Phase3Data3, target3, timer / 120f);
                ArmRightPos = Vector2.Lerp(Phase3Data4, target4, timer / 120f);
                BoneLTRot += MathHelper.TwoPi / 30f;
                BoneLBRot += MathHelper.TwoPi / 30f;
                BoneRTRot += MathHelper.TwoPi / 30f;
                BoneRBRot += MathHelper.TwoPi / 30f;
                return;
            }
            if (timer == 120)
            {
                BoneLTPos = Origin + new Vector2(0f, -ArenaSize * 0.9f);
                BoneRTPos = Origin + new Vector2(0f, ArenaSize * 0.9f);
                BoneLBPos = BoneLTPos;
                BoneRBPos = BoneRTPos;
                ArmLeftPos = Origin + new Vector2(ArenaSize * 0.9f, 0f);
                ArmRightPos = Origin + new Vector2(-ArenaSize * 0.9f, 0f);
                BoneLTRot = -MathHelper.PiOver4;
                BoneLBRot = MathHelper.PiOver4;
                BoneRTRot = MathHelper.PiOver4;
                BoneRBRot = -MathHelper.PiOver4;
                JoyceTalk("\x300cAround the Flames\x300d");
            }
            timer -= 120;
            float theta = timer / 600f * MathHelper.TwoPi;
            float r = ArenaSize * 0.6f;
            DragonPos = Origin + r * (theta - MathHelper.PiOver2).ToRotationVector2();
            SkullPos = Origin + r * (theta + MathHelper.PiOver2).ToRotationVector2();
            for (int k = 0; k < 16 * Difficulty; k++)
            {
                float offset = -k * 4f / Difficulty;
                Vector2 dir = (MathHelper.PiOver2 - theta).ToRotationVector2();
                Vector2 norm = new Vector2(-dir.Y, dir.X);
                Vector2 pos = DragonPos + (Main.rand.NextFloat() * 32f - 16f) * Difficulty * norm + offset * dir;
                dir = dir.RotatedBy(Main.rand.NextFloat() * 0.2f - 0.1f);
                AddBullet(BulletSimple.NewDragonBreath(pos, 64f * dir), 1.5f);
                dir = (-MathHelper.PiOver2 - theta).ToRotationVector2();
                norm = new Vector2(-dir.Y, dir.X);
                pos = SkullPos + (Main.rand.NextFloat() * 32f - 16f) * norm + offset * dir;
                dir = dir.RotatedBy(Main.rand.NextFloat() * 0.2f - 0.1f);
                AddBullet(BulletSimple.NewSkull(pos, 64f * dir), 1.5f);
            }
            if (timer % (240 / (1 + Difficulty)) == 0)
            {
                Phase3Data1 = Main.player[GetTarget()].Center;
                if (Difficulty >= 2)
                {
                    Phase3Data2 = 5f * Main.player[GetTarget()].velocity;
                }
                else
                {
                    Phase3Data2 = Vector2.Zero;
                }
            }
            if (timer % (240 / (1 + Difficulty)) <= 10)
            {
                Vector2 dir = Phase3Data1 + Phase3Data2 - ArmLeftPos;
                dir.Normalize();
                AddBullet(BulletSimple.NewDragon(ArmLeftPos, 16f * dir), 0.75f);
                dir = dir.RotatedBy(MathHelper.Pi / 6f);
                AddBullet(BulletSimple.NewDragon(ArmLeftPos, 16f * dir), 0.75f);
                dir = dir.RotatedBy(-MathHelper.Pi / 3f);
                AddBullet(BulletSimple.NewDragon(ArmLeftPos, 16f * dir), 0.75f);
                dir = Phase3Data1 - ArmRightPos;
                dir.Normalize();
                AddBullet(BulletSimple.NewDragon(ArmRightPos, 16f * dir), 0.75f);
                dir = dir.RotatedBy(MathHelper.Pi / 6f);
                AddBullet(BulletSimple.NewDragon(ArmRightPos, 16f * dir), 0.75f);
                dir = dir.RotatedBy(-MathHelper.Pi / 3f);
                AddBullet(BulletSimple.NewDragon(ArmRightPos, 16f * dir), 0.75f);

                dir = Phase3Data1 + Phase3Data2 - BoneLTPos;
                dir.Normalize();
                AddBullet(BulletSimple.NewBone(BoneLTPos, 16f * dir), 0.75f);
                dir = dir.RotatedBy(MathHelper.Pi / 6f);
                AddBullet(BulletSimple.NewBone(BoneLTPos, 16f * dir), 0.75f);
                dir = dir.RotatedBy(-MathHelper.Pi / 3f);
                AddBullet(BulletSimple.NewBone(BoneLTPos, 16f * dir), 0.75f);
                dir = Phase3Data1 - BoneRTPos;
                dir.Normalize();
                AddBullet(BulletSimple.NewBone(BoneRTPos, 16f * dir), 0.75f);
                dir = dir.RotatedBy(MathHelper.Pi / 6f);
                AddBullet(BulletSimple.NewBone(BoneRTPos, 16f * dir), 0.75f);
                dir = dir.RotatedBy(-MathHelper.Pi / 3f);
                AddBullet(BulletSimple.NewBone(BoneRTPos, 16f * dir), 0.75f);
            } 
        }

        private static void Phase3_2(int timer)
        {
            if (timer <= 60)
            {
                Phase3_Reset(timer);
            }
            if (timer < 90)
            {
                return;
            }
            timer -= 90;

            if (timer == 0)
            {
                MeganTalk("\x300cBreaking Box\x300d");
            }
            if (timer == 60)
            {
                AddCrystalStar(new Vector2(-0.5f, -0.5f));
                AddCrystalStar(new Vector2(0.5f, -0.5f));
                AddCrystalStar(new Vector2(-0.5f, 0.5f));
                AddCrystalStar(new Vector2(0.5f, 0.5f));
                AddCrystalStar(Vector2.Zero);
                SpawnedStars = true;
            }
            timer += 30;
            timer %= 240;
            float progress = (timer % 60) / 60f;
            int side = timer / 60;
            Vector2 start;
            Vector2 direction;
            float distance = progress * ArenaSize * 1.2f;
            switch (side)
            {
            case 0:
                start = new Vector2(ArenaSize * 0.6f, ArenaSize * 0.6f);
                direction = new Vector2(0f, -1f);
                break;
            case 1:
                start = new Vector2(ArenaSize * 0.6f, -ArenaSize * 0.6f);
                direction = new Vector2(-1f, 0f);
                break;
            case 2:
                start = new Vector2(-ArenaSize * 0.6f, -ArenaSize * 0.6f);
                direction = new Vector2(0f, 1f);
                break;
            case 3:
                start = new Vector2(-ArenaSize * 0.6f, ArenaSize * 0.6f);
                direction = new Vector2(1f, 0f);
                break;
            default:
                throw new InvalidOperationException();
            }
            Vector2 goToPos = start + distance * direction;
            ArmLeftPos = Origin + goToPos;
            ArmRightPos = Origin - goToPos;

            int interval = 8;
            if (Difficulty == 2)
            {
                interval = 6;
            }
            if (Difficulty > 2)
            {
                interval = 4;
            }
            if (timer % interval == 0)
            {
                Vector2 target = Main.player[GetTarget()].Center;
                Vector2 dir = target - ArmLeftPos;
                dir.Normalize();
                AddBullet(BulletSimple.NewDragon(ArmLeftPos, 6f * dir));
                dir = target - ArmRightPos;
                dir.Normalize();
                AddBullet(BulletSimple.NewDragon(ArmRightPos, 6f * dir));
            }
        }

        private static void Phase3_3(int timer)
        {
            if (timer <= 60)
            {
                Phase3_Reset(timer);
            }
            if (timer < 90)
            {
                return;
            }
            timer -= 90;

            if (timer == 0)
            {
                JoyceTalk("\x300cBouncing Bones\x300d");
            }
            Vector2 target = Main.player[GetTarget()].Center;
            float radius = 480f;
            if (timer < 120f)
            {
                float progress = timer / 120f;
                Vector2 leftStart = Origin + new Vector2(-ArenaSize * 0.3f, ArenaSize * 0.6f);
                Vector2 rightStart = Origin + new Vector2(ArenaSize * 0.3f, ArenaSize * 0.6f);
                BoneLTPos = Vector2.Lerp(leftStart, target + radius * (-3f * MathHelper.PiOver4).ToRotationVector2(), progress);
                BoneLBPos = Vector2.Lerp(leftStart, target + radius * (3f * MathHelper.PiOver4).ToRotationVector2(), progress);
                BoneRTPos = Vector2.Lerp(rightStart, target + radius * (-MathHelper.PiOver4).ToRotationVector2(), progress);
                BoneRBPos = Vector2.Lerp(rightStart, target + radius * MathHelper.PiOver4.ToRotationVector2(), progress);
                return;
            }
            timer -= 120;
            if (timer == 120)
            {
                AddCrystalStar(new Vector2(-0.8f, -0.8f));
                AddCrystalStar(new Vector2(0.8f, -0.8f));
                AddCrystalStar(new Vector2(-0.8f, 0.8f));
                AddCrystalStar(new Vector2(0.8f, 0.8f));
                SpawnedStars = true;
            }
            float rotation = timer * MathHelper.TwoPi / 250f;
            BoneLTPos = target + radius * (rotation - 3f * MathHelper.PiOver4).ToRotationVector2();
            BoneLTRot = rotation - MathHelper.PiOver4;
            BoneLBPos = target + radius * (rotation + 3f * MathHelper.PiOver4).ToRotationVector2();
            BoneLBRot = rotation + MathHelper.PiOver4;
            BoneRTPos = target + radius * (rotation - MathHelper.PiOver4).ToRotationVector2();
            BoneRTRot = rotation + MathHelper.PiOver4;
            BoneRBPos = target + radius * (rotation + MathHelper.PiOver4).ToRotationVector2();
            BoneRBRot = rotation - MathHelper.PiOver4;
            int difficulty = Difficulty;
            if (difficulty > 3)
            {
                difficulty = 3;
            }
            if (timer % (120 / (Difficulty + 1)) == 0)
            {
                Vector2 dir = target - BoneLTPos;
                dir.Normalize();
                AddBullet(BulletBounce.NewBone(BoneLTPos, 6f * dir, 3));
                dir = target - BoneLBPos;
                dir.Normalize();
                AddBullet(BulletBounce.NewBone(BoneLBPos, 6f * dir, 3));
                dir = target - BoneRTPos;
                dir.Normalize();
                AddBullet(BulletBounce.NewBone(BoneRTPos, 6f * dir, 3));
                dir = target - BoneRBPos;
                dir.Normalize();
                AddBullet(BulletBounce.NewBone(BoneRBPos, 6f * dir, 3));
            }
        }

        private static void Phase3_4(int timer)
        {
            if (timer <= 60)
            {
                Phase3_Reset(timer);
            }
            if (timer < 90)
            {
                return;
            }
            timer -= 90;

            if (timer == 0)
            {
                MeganTalk("\x300cUnstable Vortex\x300d");
                int num = (int)(20 * Math.Sqrt(Difficulty));
                for (int k = 0; k < num; k++)
                {
                    float rot = MathHelper.TwoPi * k / (float)num;
                    AddBullet(BulletRotateAround.NewDragonBreath((Func<Vector2>)(() => DragonPos), 100f * (float)Math.Sqrt(Difficulty), rot, MathHelper.TwoPi / 240f));
                    AddBullet(BulletRotateAround.NewSkull((Func<Vector2>)(() => SkullPos), 100f * (float)Math.Sqrt(Difficulty), rot, MathHelper.TwoPi / 240f));
                }
            }
            if (timer % 180 == 0)
            {
                Phase3Data1 = timer % 360 < 180 ? DragonPos : SkullPos;
                Phase3Data2 = Main.player[GetTarget()].Center;
                Phase3Data3.X = (Phase3Data2 - (timer % 360 < 180 ? SkullPos : DragonPos)).ToRotation();
            }
            if (timer % 180 <= 60)
            {
                if (timer % 360 < 180)
                {
                    DragonPos = Vector2.Lerp(Phase3Data1, Phase3Data2, (timer % 180) / 60f);
                }
                else
                {
                    SkullPos = Vector2.Lerp(Phase3Data1, Phase3Data2, (timer % 180) / 60f);
                }
            }
            if (timer % 180 < 120 && timer % (Math.Max(1, 2 / Difficulty)) == 0)
            {
                float rotation = timer * MathHelper.TwoPi / 45f - MathHelper.TwoPi / 15f;
                Vector2 target = Main.player[GetTarget()].Center;
                if (timer % 360 < 180)
                {
                    rotation += Phase3Data3.X + (target - SkullPos).ToRotation();
                    AddBullet(BulletWavy.NewSkull(SkullPos, 4f * rotation.ToRotationVector2(), 10f, 90f));
                }
                else
                {
                    rotation = Phase3Data3.X - rotation + (target - DragonPos).ToRotation();
                    AddBullet(BulletWavy.NewDragonBreath(DragonPos, 4f * rotation.ToRotationVector2(), 10f, 90f));
                }
            }
            if (timer == 180)
            {
                AddCrystalStar(new Vector2(0.8f, 0.8f));
                AddCrystalStar(new Vector2(-0.8f, 0.8f));
                AddCrystalStar(new Vector2(0.8f, -0.8f));
                AddCrystalStar(new Vector2(-0.8f, -0.8f));
                SpawnedStars = true;
            }
        }

        private static void Phase3_5(int timer)
        {
            if (timer <= 60)
            {
                Phase3_Reset(timer);
            }
            if (timer < 90)
            {
                return;
            }
            timer -= 90;

            if (timer == 0)
            {
                MeganTalk("\x300cBarrier of Light\x300d");
            }
            int num = 2 * ArenaSize / 32;
            if (timer < 4 * num)
            {
                Vector2 start;
                Vector2 end;
                float progress = (timer % num) / (float)num;
                if (timer < num)
                {
                    start = Origin + new Vector2(-ArenaSize + 16f, -ArenaSize + 16f);
                    end = Origin + new Vector2(-ArenaSize + 16f, ArenaSize - 16f);
                }
                else if (timer < 2 * num)
                {
                    start = Origin + new Vector2(-ArenaSize + 16f, ArenaSize - 16f);
                    end = Origin + new Vector2(ArenaSize - 16f, ArenaSize - 16f);
                }
                else if (timer < 3 * num)
                {
                    start = Origin + new Vector2(ArenaSize - 16f, ArenaSize - 16f);
                    end = Origin + new Vector2(ArenaSize - 16f, -ArenaSize + 16f);
                }
                else
                {
                    start = Origin + new Vector2(ArenaSize - 16f, -ArenaSize + 16f);
                    end = Origin + new Vector2(-ArenaSize + 16f, -ArenaSize + 16f);
                }
                AddBullet(BulletSimple.NewDragon(Vector2.Lerp(start, end, progress), Vector2.Zero));
                return;
            }
            timer -= 4 * num;

            const int space = 6;
            if (timer < 3 * (num - space) / 2)
            {
                if (timer % 3 == 0)
                {
                    Vector2 start = Origin + new Vector2(-ArenaSize + 16f, 0f);
                    AddBullet(BulletSimple.NewDragon(start + new Vector2(timer / 3 * 32f, 0f), Vector2.Zero));
                    start = Origin + new Vector2(ArenaSize - 16f, 0f);
                    AddBullet(BulletSimple.NewDragon(start - new Vector2(timer / 3 * 32f, 0f), Vector2.Zero));
                }
                return;
            }
            timer -= 3 * (num - space) / 2;

            if (timer < 3 * (num - space) / 2)
            {
                if (timer % 3 == 0)
                {
                    Vector2 start = Origin + new Vector2(-ArenaSize * 0.5f, ArenaSize);
                    AddBullet(BulletSimple.NewDragon(start + new Vector2(0f, -timer / 3 * 32f), Vector2.Zero));
                    start = Origin + new Vector2(-ArenaSize * 0.5f, -ArenaSize);
                    AddBullet(BulletSimple.NewDragon(start + new Vector2(0f, timer / 3 * 32f), Vector2.Zero));
                    start = Origin + new Vector2(ArenaSize * 0.5f, ArenaSize);
                    AddBullet(BulletSimple.NewDragon(start + new Vector2(0f, -timer / 3 * 32f), Vector2.Zero));
                    start = Origin + new Vector2(ArenaSize * 0.5f, -ArenaSize);
                    AddBullet(BulletSimple.NewDragon(start + new Vector2(0f, timer / 3 * 32f), Vector2.Zero));
                }
                return;
            }
            timer -= 3 * (num - space) / 2;

            if (timer < 3 * (num - space) / 2)
            {
                if (timer % 3 == 0)
                {
                    Vector2 start = Origin;
                    AddBullet(BulletSimple.NewDragon(start + new Vector2(0f, timer / 3 * 32f), Vector2.Zero));
                    AddBullet(BulletSimple.NewDragon(start + new Vector2(0f, -timer / 3 * 32f), Vector2.Zero));
                    start = Origin;
                    AddBullet(BulletSimple.NewDragon(start + new Vector2(0f, timer / 3 * 32f), Vector2.Zero));
                    AddBullet(BulletSimple.NewDragon(start + new Vector2(0f, -timer / 3 * 32f), Vector2.Zero));
                }
                return;
            }
            timer -= 3 * (num - space) / 2;

            if (timer == 180)
            {
                AddCrystalStar(new Vector2(-0.75f, -0.875f));
                AddCrystalStar(new Vector2(-0.75f, 0.875f));
                AddCrystalStar(new Vector2(0.75f, -0.875f));
                AddCrystalStar(new Vector2(0.75f, 0.875f));
                SpawnedStars = true;
            }
            int rows = 4 * Difficulty + 2;
            if (timer % 8 == 0)
            {
                timer /= 8;
                if (timer % 8 < 4)
                {
                    float x = Origin.X - ArenaSize;
                    for (int k = 1; k < rows; k++)
                    {
                        if (k % 2 == 0)
                        {
                            continue;
                        }
                        float y = Origin.Y + ArenaSize * (2f * k / rows - 1f);
                        AddBullet(BulletSimple.NewSkull(new Vector2(x, y), new Vector2(4f, 0f)));
                    }
                }
                else
                {
                    float x = Origin.X + ArenaSize;
                    for (int k = 1; k < rows; k++)
                    {
                        if (k % 2 != 0)
                        {
                            continue;
                        }
                        float y = Origin.Y + ArenaSize * (2f * k / rows - 1f);
                        AddBullet(BulletSimple.NewSkull(new Vector2(x, y), new Vector2(-4f, 0f)));
                    }
                }
            }
        }

        private static void Phase3_6(int timer)
        {
            if (timer <= 60)
            {
                Phase3_Reset(timer);
            }
            if (timer < 90)
            {
                return;
            }
            timer -= 90;

            if (timer == 0)
            {
                JoyceTalk("\x300cSpiraling Gear\x300d");
                float r1 = ArenaSize * 0.8f;
                float r2 = ArenaSize * 1.4f;
                int numCogs = 8;
                float cogInterval = MathHelper.Pi / numCogs;
                int numBullets = (int)(r2 / 5f);
                List<Vector2> cache = new List<Vector2>();
                bool prevOuter = false;
                for (int k = 0; k < numBullets; k++)
                {
                    float theta = MathHelper.TwoPi * k / numBullets;
                    bool outer = theta % (2f * cogInterval) < cogInterval;
                    float r;
                    if (outer != prevOuter)
                    {
                        r = r1;
                        while (r <= r2)
                        {
                            cache.Add(new Vector2(r, theta));
                            r += 32f;
                        }
                    }
                    r = outer ? r2 : r1;
                    cache.Add(new Vector2(r, theta));
                    prevOuter = outer;
                }
                foreach (Vector2 pos in cache)
                {
                    AddBullet(BulletRotateTarget.NewBone(Origin, pos.X, pos.Y, MathHelper.TwoPi / 16f / 60f, -1));
                }
                cache.Clear();
            }
            if (timer < 60)
            {
                float distance = (ArenaSize * 0.6f - 64f) / 60f;
                ArmLeftPos.X -= distance;
                ArmRightPos.X += distance;
            }
            else if (timer % (8 / Difficulty) == 0)
            {
                float theta = MathHelper.TwoPi * timer / 120f;
                int num = 4;
                for (int k = 0; k < num; k++)
                {
                    float rot = theta + MathHelper.TwoPi * k / num;
                    AddBullet(BulletSimple.NewDragon(Origin, 4f * rot.ToRotationVector2()));
                }
            }
            if (timer == 180)
            {
                AddCrystalStar(new Vector2(0.85f, 0.85f));
                AddCrystalStar(new Vector2(0.85f, -0.85f));
                AddCrystalStar(new Vector2(-0.85f, 0.85f));
                AddCrystalStar(new Vector2(-0.85f, -0.85f));
                SpawnedStars = true;
            }
        }

        private static void Phase3_7(int timer)
        {
            if (timer <= 60)
            {
                Phase3_Reset(timer);
            }
            if (timer < 90)
            {
                return;
            }
            timer -= 90;

            if (timer == 0)
            {
                Phase3Data1.X = 0f;
                for (int k = 0; k < 255; k++)
                {
                    if (Players[k] && Main.player[k].mount.Active)
                    {
                        Phase3Data1.X = 1f;
                        break;
                    }
                }
                if (Phase3Data1.X == 1f)
                {
                    JoyceTalk("You know what, that mount of yours is getting REALLY annoying...");
                }
                else
                {
                    JoyceTalk("You're not even using the Purity Shield mount right now?");
                }
            }
            if (timer == 240)
            {
                if (Phase3Data1.X == 1f)
                {
                    JoyceTalk("There we go. Much better.");
                }
                else
                {
                    JoyceTalk("Good. Keep it that way.");
                }
            }
            if (timer < 300)
            {
                return;
            }
            timer -= 300;

            if (timer == 0)
            {
                JoyceTalk("\x300cApocalypse Beam\x300d");
            }
            if (timer % (120 / Difficulty) == 0)
            {
                const float speed = 6f;
                const int interval = 192;
                for (int k = -ArenaSize; k <= ArenaSize; k += 32)
                {
                    if (((k + ArenaSize) / interval) % 2 == 0)
                    {
                        AddBullet(BulletSimple.NewBone(Origin + new Vector2(-ArenaSize, k), new Vector2(speed, 0f)));
                    }
                    else
                    {
                        AddBullet(BulletSimple.NewBone(Origin + new Vector2(ArenaSize, k), new Vector2(-speed, 0f)));
                    }
                }
            }
            if (timer % 180 < 90)
            {
                float distance = Main.player[GetTarget()].Center.X - SkullPos.X;
                SkullPos.X += distance / 4f;
            }
            else
            {
                float speed = 8f + (timer % 180 - 90) / 4f;
                for (int k = 0; k < 3; k++)
                {
                    float x = SkullPos.X + (60f * Main.rand.NextFloat() - 30f) * Difficulty;
                    AddBullet(BulletSimple.NewSkull(new Vector2(x, Origin.Y - ArenaSize - speed), new Vector2(0f, speed)));
                }
            }
            if (timer == 240)
            {
                AddCrystalStar(new Vector2(-0.4f, 0.8f));
                AddCrystalStar(new Vector2(0.4f, 0.8f));
                AddCrystalStar(new Vector2(0f, -0.5f));
                SpawnedStars = true;
            }
        }

        private static void Phase3_8(int timer)
        {
            if (timer <= 60)
            {
                Phase3_Reset(timer);
            }
            if (timer < 90)
            {
                return;
            }
            timer -= 90;

            if (timer == 0)
            {
                Main.PlaySound(4, -1, -1, 61);
            }
            if (timer == 390)
            {
                JoyceTalk("I... I wasn't... strong enough...?");
            }
            if (timer < 520)
            {
                return;
            }
            timer -= 520;

            if (timer == 0)
            {
                MeganTalk("\x300cSky Dragon's Rage\x300d");
            }
            if (timer < 30)
            {
                return;
            }
            timer -= 30;
            if (timer == 0)
            {
                Main.PlaySound(29, -1, -1, 104);
            }
            float speed = 4f + 28f * timer / 120f;
            if (speed > 32f)
            {
                speed = 32f;
            }
            for (int k = 0; k < 3; k++)
            {
                float rotation = Main.rand.NextFloat() * 0.4f - 0.2f - MathHelper.PiOver2;
                Vector2 vel = speed * rotation.ToRotationVector2();
                AddBullet(BulletTimed.NewDragonBreath(DragonPos - vel, vel, 120));
            }
            if (timer >= 120)
            {
                int sections = 8;
                float sectionWidth = 2f * ArenaSize / sections;
                bool spawnBig = timer % 2 == 0;
                int chance = 32;
                for (int k = 0; k < sections; k++)
                {
                    if (Main.rand.Next(chance) == 0)
                    {
                        float x = Origin.X - ArenaSize + k * sectionWidth;
                        x += sectionWidth * Main.rand.NextFloat();
                        float y = Origin.Y - ArenaSize - 8f;
                        AddBullet(BulletSimple.NewDragonBreath(new Vector2(x, y), new Vector2(0f, 8f)));
                    }
                }
                if (spawnBig)
                {
                    for (int k = 0; k < sections; k++)
                    {
                        if (Main.rand.Next(chance) == 0)
                        {
                            float x = Origin.X - ArenaSize + k * sectionWidth;
                            x += sectionWidth * Main.rand.NextFloat();
                            float y = Origin.Y - ArenaSize - 4f;
                            AddBullet(BulletSimple.NewDragonLarge(new Vector2(x, y), new Vector2(0f, 4f)));
                        }
                    }
                }
                if (Difficulty >= 2 && timer % 4 == 0)
                {
                    for (int k = 0; k < sections; k++)
                    {
                        if (Main.rand.Next(2 * chance) == 0)
                        {
                            float x = Origin.X - ArenaSize + k * sectionWidth;
                            x += sectionWidth * Main.rand.NextFloat();
                            float y = Origin.Y - ArenaSize - 4f;
                            float threshold = Origin.Y - ArenaSize / 2f + ArenaSize * Main.rand.NextFloat();
                            AddBullet(BulletSplit.NewDragonDiamond(new Vector2(x, y), 6f, threshold));
                        }
                    }
                }
            }
            if (timer == 240)
            {
                AddCrystalStar(new Vector2(-0.6f, -0.3f));
                AddCrystalStar(new Vector2(0f, -0.3f));
                AddCrystalStar(new Vector2(0.6f, -0.3f));
                AddCrystalStar(new Vector2(-0.6f, 0.3f));
                AddCrystalStar(new Vector2(0f, 0.3f));
                AddCrystalStar(new Vector2(0.6f, 0.3f));
                SpawnedStars = true;
            }
        }

        private static void Phase3_9(int timer)
        {
            if (timer <= 60)
            {
                Phase3_Reset(timer);
            }
            if (timer < 90)
            {
                return;
            }
            timer -= 90;

            if (timer < 120)
            {
                flash = ((timer + 1) % 60) / 60f;
            }
            if (timer == 60 || timer == 120)
            {
                Main.PlaySound(40);
            }
            if (timer == 120)
            {
                flash = 0f;
            }

            if (timer == 240)
            {
                int gore;
                for (int k = 0; k < 10; k++)
                {
                    gore = Gore.NewGore(ArmLeftPos, Vector2.Zero, Main.rand.Next(435, 438), 2f);
                    Main.gore[gore].velocity = 0.25f * k * (Main.rand.NextFloat() * MathHelper.TwoPi).ToRotationVector2();
                    gore = Gore.NewGore(ArmRightPos, Vector2.Zero, Main.rand.Next(435, 438), 2f);
                    Main.gore[gore].velocity = 0.25f * k * (Main.rand.NextFloat() * MathHelper.TwoPi).ToRotationVector2();
                }
                Main.PlaySound(16);
            }
            if (timer == 360)
            {
                Main.PlaySound(SoundID.DD2_WinScene);
            }
            if (timer > 360 && timer <= 960)
            {
                flash = (timer - 360) / 600f;
            }
            if (timer == 961)
            {
                flash = 0f;
            }
            if (timer == 1080)
            {
                MeganTalk("I'm free...");
            }
            if (timer == 1200)
            {
                BluemagicWorld.downedBlushie = true;
                if (Main.netMode == 2)
                {
                    NetMessage.SendData(MessageID.WorldData);
                }
                Item.NewItem((int)Origin.X, (int)Origin.Y, 0, 0, Bluemagic.Instance.ItemType("PuriumCoin"), Main.expertMode ? Main.rand.Next(48, 53) : Main.rand.Next(24, 27));
                Item.NewItem((int)SkullPos.X, (int)SkullPos.Y, 0, 0, Bluemagic.Instance.ItemType("PuriumCoin"), Main.expertMode ? Main.rand.Next(48, 53) : Main.rand.Next(24, 27));
                Item.NewItem((int)Origin.X, (int)Origin.Y, 0, 0, Bluemagic.Instance.ItemType("SkyDragonHeart"));
                Item.NewItem((int)SkullPos.X, (int)SkullPos.Y, 0, 0, Bluemagic.Instance.ItemType("WorldReaver"));
                Main.NewText(Language.GetTextValue("Announcement.HasBeenDefeated_Single", "blushiemagic"));
                Reset();
            }
        }

        private static void Phase3_Reset(int timer)
        {
            MoveTo(ref DragonPos, Origin + new Vector2(0f, -ArenaSize * 0.6f), timer);
            MoveTo(ref ArmLeftPos, Origin + new Vector2(ArenaSize * 0.6f, 0f), timer);
            MoveTo(ref ArmRightPos, Origin + new Vector2(-ArenaSize * 0.6f, 0f), timer);
            MoveTo(ref SkullPos, Origin + new Vector2(0f, ArenaSize * 0.6f), timer);
            MoveTo(ref BoneLTPos, Origin + new Vector2(-ArenaSize * 0.3f, ArenaSize * 0.6f), timer);
            MoveTo(ref BoneLBPos, Origin + new Vector2(-ArenaSize * 0.3f, ArenaSize * 0.6f), timer);
            MoveTo(ref BoneRTPos, Origin + new Vector2(ArenaSize * 0.3f, ArenaSize * 0.6f), timer);
            MoveTo(ref BoneRBPos, Origin + new Vector2(ArenaSize * 0.3f, ArenaSize * 0.6f), timer);
            BoneLTRot = -MathHelper.PiOver4;
            BoneLBRot = MathHelper.PiOver4;
            BoneRTRot = MathHelper.PiOver4;
            BoneRBRot = -MathHelper.PiOver4;
        }

        private static void MoveTo(ref Vector2 position, Vector2 target, int timer)
        {
            float distance = 16f;
            int timeLeft = 60 - timer;
            float length = Vector2.Distance(position, target);
            if (timeLeft <= 0 || length == 0f)
            {
                position = target;
                return;
            }
            if (distance < length / timeLeft)
            {
                distance = length / timeLeft;
            }
            if (distance > length)
            {
                distance = length;
            }
            Vector2 offset = target - position;
            offset.Normalize();
            position += offset * distance;
        }

        internal static int GetTarget()
        {
            if (Players[Main.myPlayer])
            {
                return Main.myPlayer;
            }
            for (int k = 0; k < 255; k++)
            {
                if (Players[k])
                {
                    return k;
                }
            }
            return 255;
        }

        internal static void AddBullet(Bullet bullet, float damageMult = 1f)
        {
            bullet.Damage *= damageMult;
            bullets.Add(bullet);
        }

        private static void AddCrystalStar(Vector2 offset)
        {
            crystalStars.Add(Origin + ArenaSize * offset);
        }

        private static void Music(string message)
        {
            if (Main.netMode != 2)
            {
                Main.NewText(message);
            }
            else
            {
                NetworkText text = NetworkText.FromLiteral(message);
                NetMessage.BroadcastChatMessage(text, Color.White);
            }
        }

        private static void Talk(string name, string message, byte r, byte g, byte b)
        {
            if (Main.netMode != 2)
            {
                string text = Language.GetTextValue("Mods.Bluemagic.NPCTalk", name, message);
                Main.NewText(text, r, g, b);
            }
            else
            {
                NetworkText text = NetworkText.FromKey("Mods.Bluemagic.NPCTalk", name, message);
                NetMessage.BroadcastChatMessage(text, new Color(r, g, b));
            }
        }

        private static void BlushieTalk(string message)
        {
            Talk("blushiemagic", message, 200, 255, 255);
        }

        internal static void KylieTalk(string message)
        {
            Talk("blushiemagic (K)", message, 0, 128, 255);
        }

        internal static void AnnaTalk(string message)
        {
            Talk("blushiemagic (A)", message, 255, 128, 128);
        }

        internal static void LunaTalk(string message)
        {
            Talk("blushiemagic (L)", message, 128, 0, 128);
        }

        internal static void ChrisTalk(string message)
        {
            Talk("blushiemagic (C)", message, 255, 255, 0);
        }

        internal static void MeganTalk(string message)
        {
            Talk("blushiemagic (M)", message, 0, 255, 0);
        }

        internal static void JoyceTalk(string message)
        {
            Talk("blushiemagic (J)", message, 127, 0, 0);
        }

        internal static void DrawArena(SpriteBatch spriteBatch)
        {
            const int blockSize = 16;
            int centerX = (int)Origin.X;
            int centerY = (int)Origin.Y;
            Texture2D outlineTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BlushieBlockOutline");
            Texture2D blockTexture = Bluemagic.Instance.GetTexture("BlushieBoss/BlushieBlock");
            for (int x = centerX - ArenaSize - blockSize / 2; x <= centerX + ArenaSize + blockSize / 2; x += blockSize)
            {
                int y = centerY - ArenaSize - blockSize / 2;
                Vector2 drawPos = new Vector2(x - blockSize / 2, y - blockSize / 2) - Main.screenPosition;
                spriteBatch.Draw(outlineTexture, drawPos, Color.White);
                spriteBatch.Draw(blockTexture, drawPos, Color.White * 0.75f);
                drawPos.Y += 2 * ArenaSize + blockSize;
                spriteBatch.Draw(outlineTexture, drawPos, Color.White);
                spriteBatch.Draw(blockTexture, drawPos, Color.White * 0.75f);
            }
            for (int y = centerY - ArenaSize - blockSize / 2; y <= centerY + ArenaSize + blockSize / 2; y += blockSize)
            {
                int x = centerX - ArenaSize - blockSize / 2;
                Vector2 drawPos = new Vector2(x - blockSize / 2, y - blockSize / 2) - Main.screenPosition;
                spriteBatch.Draw(outlineTexture, drawPos, Color.White);
                spriteBatch.Draw(blockTexture, drawPos, Color.White * 0.75f);
                drawPos.X += 2 * ArenaSize + blockSize;
                spriteBatch.Draw(outlineTexture, drawPos, Color.White);
                spriteBatch.Draw(blockTexture, drawPos, Color.White * 0.75f);
            }
            if (flash > 0)
            {
                Texture2D pixel = Bluemagic.Instance.GetTexture("Pixel");
                int left = centerX - ArenaSize - (int)Main.screenPosition.X;
                int top = centerY - ArenaSize - (int)Main.screenPosition.Y;
                spriteBatch.Draw(pixel, new Rectangle(left, top, 2 * ArenaSize, 2 * ArenaSize), Color.White * flash);
            }
        }

        internal static void DrawBullets(SpriteBatch spriteBatch)
        {
            foreach (Bullet bullet in bullets)
            {
                spriteBatch.Draw(bullet.Texture, bullet.Position - new Vector2(bullet.Size) - Main.screenPosition, Color.White);
            }
            foreach (Vector2 star in crystalStars)
            {
                spriteBatch.Draw(CrystalStarTexture, star - new Vector2(32f) - Main.screenPosition, Color.White);
            }
        }
    }
}