using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Bluemagic.BlushieBoss;
using Bluemagic.ChaosSpirit;
using Bluemagic.Interface;
using Bluemagic.PuritySpirit;
using Bluemagic.TerraSpirit;
using Bluemagic.Tiles;
using ReLogic.Content;
using Bluemagic.Items.Phantom;
using Bluemagic.Items.Abomination;
using Bluemagic.Items.ChaosSpirit;
using Bluemagic.Items.TerraSpirit;

namespace Bluemagic
{
    public class Bluemagic : Mod
    {
        public static Mod Instance;
        public static Mod Calamity;
        public static Mod Thorium;
        public static Mod Sushi;
        public static Mod HealthBars;
        public const bool testing = false;

        private static Color pureColor = new Color(100, 255, 100);
        private static int pureColorStyle = 0;

        public const string captiveElementHead = "Bluemagic/Abomination/CaptiveElement_Head_Boss_";
        public const string captiveElement2Head = "Bluemagic/Abomination/CaptiveElement2_Head_Boss_";
        public static bool freezeHeroLives = false;

        public static Color PureColor
        {
            get
            {
                return pureColor;
            }
        }

        public override void Load()
        {
            Instance = this;
            foreach (Mod checkMod in ModLoader.Mods)
            {
                if (checkMod.Name == "ExampleMod")
                {
                    throw new Exception("ExampleMod and Bluemagic cannot be loaded at the same time");
                }
            }
            InterfaceHelper.Initialize();
            EquipLoader.AddEquipTexture(this, "Bluemagic/Blushie/DarkLightningPack_Back", EquipType.Back, name: "DarkLightningPack_Back");
            for (int k = 1; k <= 4; k++)
            {
                AddBossHeadTexture(captiveElementHead + k);
                AddBossHeadTexture(captiveElement2Head + k);
            }
            Filters.Scene["Bluemagic:PuritySpirit"] = new Filter(new PuritySpiritScreenShaderData("FilterMiniTower").UseColor(0.4f, 0.9f, 0.4f).UseOpacity(0.7f), EffectPriority.VeryHigh);
            SkyManager.Instance["Bluemagic:PuritySpirit"] = new PuritySpiritSky();
            Filters.Scene["Bluemagic:MonolithVoid"] = new Filter(new ScreenShaderData("FilterMoonLord"), EffectPriority.Medium);
            SkyManager.Instance["Bluemagic:MonolithVoid"] = new VoidSky();
            Filters.Scene["Bluemagic:ChaosSpirit"] = new Filter(new ChaosSpiritScreenShaderData("FilterMiniTower").UseColor(0.9f, 0.4f, 0.4f).UseOpacity(0.25f), EffectPriority.VeryHigh);
            SkyManager.Instance["Bluemagic:ChaosSpirit"] = new ChaosSpiritSky();
            Filters.Scene["Bluemagic:TerraSpirit"] = new Filter(new TerraSpiritScreenShaderData("FilterMiniTower").UseColor(0f, 0f, 0f).UseOpacity(0.1f), EffectPriority.VeryHigh);
            SkyManager.Instance["Bluemagic:TerraSpirit"] = new TerraSpiritSky();
            SkyManager.Instance["Bluemagic:BlushieBoss"] = new BlushieSky();
            Overlays.Scene["Bluemagic:WorldReaver"] = new WorldReaverOverlay();
            if (!Main.dedServ)
            {
                Filters.Scene["Bluemagic:WorldReaver"] = new Filter(new ScreenShaderData(ModContent.Request<Effect>("Bluemagic/Effects/WorldReaver"), "WorldReaver"), EffectPriority.VeryHigh);
            }
        }

        public override void PostSetupContent()
        {
            ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist);
            if (bossChecklist != null)
            {
                bossChecklist.Call(
                    "LogBoss",
                    Instance,
                    nameof(Phantom.Phantom),
                    13.05f,
                    () => BluemagicWorld.downedPhantom,
                    ModContent.NPCType<Phantom.Phantom>(),
                    new Dictionary<string, object>()
                    {
                        ["spawnItems"] = ModContent.ItemType<PaladinEmblem>(),
                        ["spawnInfo"] = GetLocalization("NPCs.Phantom.SpawnInfo")
                    }
                );
                bossChecklist.Call(
                    "LogBoss",
                    Instance,
                    "Abomination",
                    13.8f,
                    () => BluemagicWorld.downedAbomination,
                    ModContent.NPCType<Abomination.Abomination>(),
                    new Dictionary<string, object>()
                    {
                        ["spawnItems"] = ModContent.ItemType<FoulOrb>(),
                        ["spawnInfo"] = GetLocalization("NPCs.Abomination.SpawnInfo")
                    }
                );
                bossChecklist.Call(
                    "LogBoss",
                    Instance,
                    "AbominationRematch",
                    18.5f,
                    () => BluemagicWorld.elementalUnleash,
                    ModContent.NPCType<Abomination.Abomination>(),
                    new Dictionary<string, object>()
                    {
                        ["spawnItems"] = ModContent.ItemType<FoulOrb>(),
                        ["spawnInfo"] = GetLocalization("NPCs.Abomination.SpawnInfoRematch"),
                        ["displayName"] = GetLocalization("NPCs.Abomination.Rematch"),
                    }
                );
                bossChecklist.Call(
                    "LogBoss",
                    Instance,
                    "TheSpiritofPurity",
                    20,
                    () => BluemagicWorld.downedPuritySpirit,
                    ModContent.NPCType<PuritySpirit.PuritySpirit>(),
                    new Dictionary<string, object>()
                    {
                        ["spawnItems"] = ModContent.ItemType<Items.PuritySpirit.ElementalPurge>(),
                        ["spawnInfo"] = GetLocalization("NPCs.PuritySpirit.SpawnInfo")
                    }
                );
                bossChecklist.Call(
                    "LogBoss",
                    Instance,
                    "TheSpiritofChaos",
                    22,
                    () => BluemagicWorld.downedChaosSpirit,
                    ModContent.NPCType<ChaosSpirit.ChaosSpirit>(),
                    new Dictionary<string, object>()
                    {
                        ["spawnItems"] = ModContent.ItemType<RitualOfEndings>(),
                        ["spawnInfo"] = GetLocalization("NPCs.ChaosSpirit.SpawnInfo")
                    }
                );
                bossChecklist.Call(
                    "LogBoss",
                    Instance,
                    "QMarxOne",
                    42,
                    () => BluemagicWorld.terraCheckpoint1 > 0,
                    ModContent.NPCType<TerraSpirit.TerraSpirit>(),
                    new Dictionary<string, object>()
                    {
                        ["spawnItems"] = ModContent.ItemType<RitualOfBunnies>(),
                        ["spawnInfo"] = GetLocalization("NPCs.PuritySpirit.SpawnInfo2"),
                        ["displayName"] = GetLocalization("NPCs.PuritySpirit.Terra1"),
                    }
                );
                bossChecklist.Call(
                    "LogBoss",
                    Instance,
                    "QMarxTwo",
                    256,
                    () => BluemagicWorld.terraCheckpoint2 > 0,
                    ModContent.NPCType<TerraSpirit.TerraSpirit>(),
                    new Dictionary<string, object>()
                    {
                        ["spawnItems"] = ModContent.ItemType<Checkpoint1>(),
                        ["spawnInfo"] = GetLocalization("NPCs.PuritySpirit.SpawnInfo3"),
                        ["displayName"] = GetLocalization("NPCs.PuritySpirit.Terra2"),
                    }
                );
                bossChecklist.Call(
                    "LogBoss",
                    Instance,
                    "QMarxThree",
                    666,
                    () => BluemagicWorld.terraCheckpoint3 > 0,
                    ModContent.NPCType<TerraSpirit.TerraSpirit>(),
                    new Dictionary<string, object>()
                    {
                        ["spawnItems"] = ModContent.ItemType<Checkpoint2>(),
                        ["spawnInfo"] = GetLocalization("NPCs.PuritySpirit.SpawnInfo4"),
                        ["displayName"] = GetLocalization("NPCs.PuritySpirit.Terra3"),
                    }
                );
                bossChecklist.Call(
                    "LogBoss",
                    Instance,
                    "QMarxFour",
                    1337,
                    () => BluemagicWorld.terraCheckpointS > 0,
                    ModContent.NPCType<TerraSpirit.TerraSpirit>(),
                    new Dictionary<string, object>()
                    {
                        ["spawnItems"] = ModContent.ItemType<Checkpoint3>(),
                        ["spawnInfo"] = GetLocalization("NPCs.PuritySpirit.SpawnInfo5"),
                        ["displayName"] = GetLocalization("NPCs.PuritySpirit.Terra4"),
                    }
                );
                bossChecklist.Call(
                    "LogBoss",
                    Instance,
                    "QMarx",
                    9001,
                    () => BluemagicWorld.downedTerraSpirit,
                    ModContent.NPCType<TerraSpirit.TerraSpirit>(),
                    new Dictionary<string, object>()
                    {
                        ["spawnInfo"] = GetLocalization("NPCs.PuritySpirit.SpawnInfo6"),
                        ["displayName"] = GetLocalization("NPCs.PuritySpirit.Terra5"),
                    }
                );



                /*bossList.Call("AddBossWithInfo", "The Phantom", 12.05f, (Func<bool>)(() => BluemagicWorld.downedPhantom), string.Format("Use a [i:{0}] in the Dungeon (Plantera must be defeated)", Find<ModItem>("PaladinEmblem").Type));
                bossList.Call("AddBossWithInfo", "The Abomination", 12.8f, (Func<bool>)(() => BluemagicWorld.downedAbomination), string.Format("Use a [i:{0}] in the Underworld (Plantera must be defeated)", Find<ModItem>("FoulOrb").Type));
                bossList.Call("AddBossWithInfo", "The Abomination (Rematch)", 14.5f, (Func<bool>)(() => BluemagicWorld.elementalUnleash), string.Format("Use a [i:{0}] in the Underworld (Moon Lord must be defeated). [c/FF0000:Starts the Elemental Unleash!]", Find<ModItem>("FoulOrb").Type));
                bossList.Call("AddBossWithInfo", "The Spirit of Purity", 16f, (Func<bool>)(() => BluemagicWorld.downedPuritySpirit), string.Format("Kill a Bunny while the Bunny is standing in front of a placed [i:{0}]", Find<ModItem>("ElementalPurge").Type));
                bossList.Call("AddBossWithInfo", "The Spirit of Chaos", 18f, (Func<bool>)(() => BluemagicWorld.downedChaosSpirit), string.Format("Use a [i:{0}] anytime, anywhere (has infinite reuses)", Find<ModItem>("RitualOfEndings").Type));
                bossList.Call("AddBossWithInfo", "????? (Phase 1)", 42f, (Func<bool>)(() => BluemagicWorld.terraCheckpoint1 > 0), string.Format("Use a [i:{0}] anytime, anywhere, after all previous bosses have been defeated (has infinite reuses)", Find<ModItem>("RitualOfBunnies").Type));
                bossList.Call("AddBossWithInfo", "????? (Phase 2)", 256f, (Func<bool>)(() => BluemagicWorld.terraCheckpoint2 > 0), string.Format("Defeat the previous phase or use a [i:{0}]", Find<ModItem>("Checkpoint1").Type));
                bossList.Call("AddBossWithInfo", "????? (Phase 3)", 666f, (Func<bool>)(() => BluemagicWorld.terraCheckpoint3 > 0), string.Format("Defeat the previous phase or use a [i:{0}]", Find<ModItem>("Checkpoint2").Type));
                bossList.Call("AddBossWithInfo", "????? (Phase 4)", 1337f, (Func<bool>)(() => BluemagicWorld.terraCheckpointS > 0), string.Format("Defeat the previous phase or use a [i:{0}]", Find<ModItem>("Checkpoint3").Type));
                bossList.Call("AddBossWithInfo", "?????", 9001f, (Func<bool>)(() => BluemagicWorld.downedTerraSpirit), "Overcome all phases and defeat the boss once and for all!");*/
            }
            ModLoader.TryGetMod("CalamityMod", out Calamity);
            ModLoader.TryGetMod("ThoriumMod", out Thorium);
            ModLoader.TryGetMod("imkSushisMod", out Sushi);
            if (!Main.dedServ)
            {
                BlushieBoss.BlushieBoss.Load();
            }

            ModLoader.TryGetMod("FKBossHealthBar", out HealthBars);
            if (HealthBars != null)
            {
                HealthBars.Call("RegisterHealthBarMini", Find<ModNPC>("BlushiemagicK").Type);
                HealthBars.Call("RegisterHealthBarMini", Find<ModNPC>("BlushiemagicA").Type);
                HealthBars.Call("RegisterHealthBarMini", Find<ModNPC>("BlushiemagicL").Type);
            }
        }

        public override void Unload()
        {
            Instance = null;
            Calamity = null;
            Thorium = null;
            Sushi = null;
            HealthBars = null;
            BlushieBoss.BlushieBoss.Unload();
        }

        public override object Call(object[] args)
        {
            if (args.Length == 0)
            {
                return null;
            }
            if (args[0] == "Set")
            {
                if (args.Length < 4)
                {
                    return null;
                }
                if (args[1] is Player && args[2] is string)
                {
                    return CallPlayerSet((Player)args[1], (string)args[2], args[3]);
                }
                if (args[1] == "Global" && args[2] is string)
                {
                    return CallSet((string)args[2], args[3]);
                }
                return null;
            }
            if (args[0] == "Get")
            {
                if (args.Length < 2)
                {
                    return null;
                }
                if (args[1] is string)
                {
                    return CallGet((string)args[1]);
                }
                return null;
            }
            return null;
        }

        private object CallPlayerSet(Player player, string command, object arg)
        {
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            if (command == "puriumShieldChargeMax" && arg is float)
            {
                modPlayer.puriumShieldChargeMax = (float)arg;
                return arg;
            }
            if (command == "puriumShieldChargeRate" && arg is float)
            {
                modPlayer.puriumShieldChargeRate = (float)arg;
                return arg;
            }
            if (command == "puriumShieldEnduranceMult" && arg is float)
            {
                modPlayer.puriumShieldEnduranceMult = (float)arg;
                return arg;
            }
            if (command == "manaMagnet2" && arg is bool)
            {
                modPlayer.manaMagnet2 = (bool)arg;
                return arg;
            }
            if (command == "crystalCloak" && arg is bool)
            {
                modPlayer.crystalCloak = (bool)arg;
                return arg;
            }
            if (command == "lifeMagnet2" && arg is bool)
            {
                modPlayer.lifeMagnet2 = (bool)arg;
                return arg;
            }
            if (command == "noGodmode" && arg is bool)
            {
                modPlayer.noGodmode = (bool)arg;
                return arg;
            }
            return null;
        }

        private object CallSet(string command, object arg)
        {
            if (command == "blushieDifficulty")
            {
                BlushieBoss.BlushieBoss.difficultyOverride = (Func<int>)arg;
                return arg;
            }
            return null;
        }

        private object CallGet(string command)
        {
            if (command == "downedPhantom")
            {
                return BluemagicWorld.downedPhantom;
            }
            if (command == "downedAbomination")
            {
                return BluemagicWorld.downedAbomination;
            }
            if (command == "elementalUnleash")
            {
                return BluemagicWorld.elementalUnleash;
            }
            if (command == "downedPuritySpirit")
            {
                return BluemagicWorld.downedPuritySpirit;
            }
            if (command == "downedChaosSpirit")
            {
                return BluemagicWorld.downedChaosSpirit;
            }
            if (command == "downedTerraSpirit")
            {
                return BluemagicWorld.downedTerraSpirit;
            }
            return null;
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType type = (MessageType)reader.ReadByte();
            if (type == MessageType.PuritySpirit)
            {
                PuritySpirit.PuritySpirit spirit = Main.npc[reader.ReadInt32()].ModNPC as PuritySpirit.PuritySpirit;
                if (spirit != null && spirit.NPC.active)
                {
                    spirit.HandlePacket(reader);
                }
            }
            else if (type == MessageType.HeroLives)
            {
                Player player = Main.player[reader.ReadInt32()];
                int lives = reader.ReadInt32();
                player.GetModPlayer<BluemagicPlayer>().heroLives = lives;
                if (lives > 0)
                {
                    NetworkText text;
                    if (lives == 1)
                    {
                        text = NetworkText.FromKey("Mods.Bluemagic.LifeLeft", player.name);
                    }
                    else
                    {
                        text = NetworkText.FromKey("Mods.Bluemagic.LivesLeft", player.name, lives);
                    }
                    ChatHelper.BroadcastChatMessage(text, new Color(255, 25, 25));
                }
            }
            else if (type == MessageType.ChaosSpirit)
            {
                NPC npc = Main.npc[reader.ReadInt32()];
                if (npc.active)
                {
                    ChaosSpirit.ChaosSpirit spirit = npc.ModNPC as ChaosSpirit.ChaosSpirit;
                    if (spirit != null)
                    {
                        spirit.HandlePacket(reader);
                    }
                    ChaosSpirit2 spirit2 = npc.ModNPC as ChaosSpirit2;
                    if (spirit2 != null)
                    {
                        spirit2.HandlePacket(reader);
                    }
                    ChaosSpirit3 spirit3 = npc.ModNPC as ChaosSpirit3;
                    if (spirit3 != null)
                    {
                        spirit3.HandlePacket(reader);
                    }
                }
            }
            else if (type == MessageType.PushChaosArm)
            {
                NPC npc = Main.npc[reader.ReadInt32()];
                Vector2 push = new Vector2(reader.ReadSingle(), reader.ReadSingle());
                if (npc.active)
                {
                    ChaosSpiritArm arm = npc.ModNPC as ChaosSpiritArm;
                    if (arm != null)
                    {
                        arm.offset += push;
                        if (Main.netMode == NetmodeID.Server)
                        {
                            ModPacket packet = GetPacket();
                            packet.Write((byte)MessageType.PushChaosArm);
                            packet.Write(push.X);
                            packet.Write(push.Y);
                            packet.Send(-1, whoAmI);
                        }
                    }
                }
            }
            else if (type == MessageType.TerraSpirit)
            {
                NPC npc = Main.npc[reader.ReadInt32()];
                if (npc.active)
                {
                    TerraSpirit.TerraSpirit spirit = npc.ModNPC as TerraSpirit.TerraSpirit;
                    if (spirit != null)
                    {
                        spirit.HandlePacket(reader);
                    }
                }
            }
            else if (type == MessageType.TerraLives)
            {
                Player player = Main.player[reader.ReadInt32()];
                int lives = reader.ReadInt32();
                player.GetModPlayer<BluemagicPlayer>().terraLives = lives;
                if (lives > 0)
                {
                    NetworkText text;
                    if (lives == 1)
                    {
                        text = NetworkText.FromKey("Mods.Bluemagic.LifeLeft", player.name);
                    }
                    else
                    {
                        text = NetworkText.FromKey("Mods.Bluemagic.LivesLeft", player.name, lives);
                    }
                    ChatHelper.BroadcastChatMessage(text, new Color(255, 25, 25));
                }
            }
            else if (type == MessageType.GoldBlob)
            {
                NPC npc = Main.npc[reader.ReadByte()];
                float value = reader.ReadByte();
                if (npc.active && npc.type == Find<ModNPC>("GoldBlob").Type)
                {
                    npc.localAI[0] = value;
                }
            }
            else if (type == MessageType.ExtraLives)
            {
                BluemagicPlayer player = Main.player[Main.myPlayer].GetModPlayer<BluemagicPlayer>();
                if (player.terraLives > 0)
                {
                    player.terraLives += 3;
                }
            }
            else if (type == MessageType.BulletNegative)
            {
                NPC npc = Main.npc[reader.ReadByte()];
                if (npc.active && npc.type == Find<ModNPC>("TerraSpirit2").Type && npc.ModNPC is TerraSpirit2)
                {
                    var bullets = ((TerraSpirit2)npc.ModNPC).bullets;
                    int count = reader.ReadByte();
                    for (int k = 0; k < count; k++)
                    {
                        bullets.Add(new BulletNegative(reader.ReadVector2(), reader.ReadVector2()));
                    }
                }
            }
            else if (type == MessageType.CustomStats)
            {
                byte byte1 = reader.ReadByte();
                byte byte2 = reader.ReadByte();
                Player player = Main.player[byte1];
                BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
                CustomStats stats = byte2 == 0 ? modPlayer.chaosStats : modPlayer.cataclysmStats;
                stats.NetReceive(reader);
                if (Main.netMode == NetmodeID.Server)
                {
                    ModPacket packet = GetPacket(512);
                    packet.Write(byte1);
                    packet.Write(byte2);
                    stats.NetSend(packet);
                    packet.Send(-1, whoAmI);
                }
            }
            else if (type == MessageType.WorldReaver)
            {
                WorldReaverData.Begin(reader.ReadInt32());
            }
        }

        public static void UpdatePureColor()
        {
            pureColor.R = (byte)(255 - 155f * Math.Abs(Math.Cos(pureColorStyle * Math.PI / 200.0)));
            pureColor.B = pureColor.R;
            pureColorStyle = (pureColorStyle + 1) % 200;
        }

        public static void NewText(string key, int r, int g, int b)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(Language.GetTextValue(key), (byte)r, (byte)g, (byte)b);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                NetworkText text = NetworkText.FromKey(key);
                ChatHelper.BroadcastChatMessage(text, new Color(r, g, b));
            }
        }

        public static void NewText(string key, Color color)
        {
            NewText(key, color.R, color.G, color.B);
        }
    }

    enum MessageType : byte
    {
        PuritySpirit,
        HeroLives,
        ChaosSpirit,
        PushChaosArm,
        TerraSpirit,
        TerraLives,
        GoldBlob,
        ExtraLives,
        BulletNegative,
        CustomStats,
        WorldReaver
    }
}