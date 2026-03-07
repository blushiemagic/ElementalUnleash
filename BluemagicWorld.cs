using Bluemagic.Blushie;
using Bluemagic.BlushieBoss;
using Bluemagic.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace Bluemagic
{
    public class BluemagicWorld : ModSystem
    {
        private const int saveVersion = 0;
        public static bool eclipsePassed = false;
        public static bool pumpkinMoonPassed = false;
        public static bool snowMoonPassed = false;
        public static bool downedPhantom = false;
        public static bool downedAbomination = false;
        public static bool elementalUnleash = false;
        public static int numPuriumGens = 0;
        public static bool downedPuritySpirit = false;
        public static bool downedChaosSpirit = false;
        public static int terraDeaths = 0;
        internal static int terraCheckpoint1 = 0;
        internal static int terraCheckpoint2 = 0;
        internal static int terraCheckpoint3 = 0;
        internal static int terraCheckpointS = 0;
        public static bool downedTerraSpirit = false;
        public static float blushieCheckpoint = 0f;
        public static bool downedBlushie = false;

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            InterfaceHelper.ModifyInterfaceLayers(layers);
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            // Better boss bars are dead, dnc _ YuH
            if (BlushieBoss.BlushieBoss.Active && BlushieBoss.BlushieBoss.Phase == 3 && BlushieBoss.BlushieBoss.Phase3Attack > 0 && Bluemagic.HealthBars != null)
            {
               // BlushieBoss.HealthBarDraw.DrawHealthBarDefault(spriteBatch, 1f);
            }
        }

        public override void AddRecipes()
        {
            BluemagicRecipes.AddRecipes(Mod);
        }
        public override void OnWorldLoad()
        {
            eclipsePassed = false;
            pumpkinMoonPassed = false;
            snowMoonPassed = false;
            downedPhantom = false;
            downedAbomination = false;
            elementalUnleash = false;
            numPuriumGens = 0;
            downedPuritySpirit = false;
            downedChaosSpirit = false;
            terraDeaths = 0;
            terraCheckpoint1 = 0;
            terraCheckpoint2 = 0;
            terraCheckpoint3 = 0;
            terraCheckpointS = 0;
            downedTerraSpirit = false;
            blushieCheckpoint = 0f;
            downedBlushie = false;
        }

        private void FixCheckpoints()
        {
            if (terraCheckpoint1 < 0)
            {
                terraCheckpoint1 = 0;
            }
            if (terraCheckpoint1 > 10)
            {
                terraCheckpoint1 = 10;
            }
            if (terraCheckpoint2 < 0)
            {
                terraCheckpoint2 = 0;
            }
            if (terraCheckpoint2 > 10)
            {
                terraCheckpoint2 = 10;
            }
            if (terraCheckpoint3 < 0)
            {
                terraCheckpoint3 = 0;
            }
            if (terraCheckpoint3 > 10)
            {
                terraCheckpoint3 = 10;
            }
            if (terraCheckpointS < 0)
            {
                terraCheckpointS = 0;
            }
            if (terraCheckpointS > 10)
            {
                terraCheckpointS = 10;
            }
            if (blushieCheckpoint < 0f)
            {
                blushieCheckpoint = 0f;
            }
            if (blushieCheckpoint > 1f)
            {
                blushieCheckpoint = 1f;
            }
        }

        public override void SaveWorldData(TagCompound tag)
        {
            FixCheckpoints();
            tag["eclipsePassed"] = eclipsePassed;
            tag["pumpkinMoonPassed"] = pumpkinMoonPassed;
            tag["snowMoonPassed"] = snowMoonPassed;
            tag["downedPhantom"] = downedPhantom;
            tag["downedAbomination"] = downedAbomination;
            tag["elementalUnleash"] = elementalUnleash;
            tag["numPuriumGens"] = numPuriumGens;
            tag["downedPuritySpirit"] = downedPuritySpirit;
            tag["downedChaosSpirit"] = downedChaosSpirit;
            tag["terraDeaths"] = terraDeaths;
            tag["terraCheckpoint1"] = terraCheckpoint1;
            tag["terraCheckpoint2"] = terraCheckpoint2;
            tag["terraCheckpoint3"] = terraCheckpoint3;
            tag["terraCheckpointS"] = terraCheckpointS;
            tag["downedTerraSpirit"] = downedTerraSpirit;
            tag["blushieCheckpoint"] = blushieCheckpoint;
            tag["downedBlushie"] = downedBlushie;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            eclipsePassed = tag.GetBool("eclipsePassed");
            pumpkinMoonPassed = tag.GetBool("pumpkinMoonPassed");
            snowMoonPassed = tag.GetBool("snowMoonPassed");
            downedPhantom = tag.GetBool("downedPhantom");
            downedAbomination = tag.GetBool("downedAbomination");
            if (tag.ContainsKey("numPuriumGens"))
            {
                elementalUnleash = tag.GetBool("elementalUnleash");
                numPuriumGens = tag.GetInt("numPuriumGens");
            }
            else
            {
                numPuriumGens = tag.GetInt("elementalUnleash");
                elementalUnleash = numPuriumGens > 0;
            }
            downedPuritySpirit = tag.GetBool("downedPuritySpirit");
            downedChaosSpirit = tag.GetBool("downedChaosSpirit");
            terraDeaths = tag.GetInt("terraDeaths");
            terraCheckpoint1 = tag.GetInt("terraCheckpoint1");
            terraCheckpoint2 = tag.GetInt("terraCheckpoint2");
            terraCheckpoint3 = tag.GetInt("terraCheckpoint3");
            terraCheckpointS = tag.GetInt("terraCheckpointS");
            downedTerraSpirit = tag.GetBool("downedTerraSpirit");
            blushieCheckpoint = tag.GetFloat("blushieCheckpoint");
            downedBlushie = tag.GetBool("downedBlushie");
            FixCheckpoints();
        }

        public override void NetSend(BinaryWriter writer)
        {
            FixCheckpoints();
            byte flags = 0;
            if (eclipsePassed)
            {
                flags |= 1;
            }
            if (pumpkinMoonPassed)
            {
                flags |= 2;
            }
            if (snowMoonPassed)
            {
                flags |= 4;
            }
            if (downedPhantom)
            {
                flags |= 8;
            }
            if (downedAbomination)
            {
                flags |= 16;
            }
            if (elementalUnleash)
            {
                flags |= 32;
            }
            if (downedPuritySpirit)
            {
                flags |= 64;
            }
            if (downedChaosSpirit)
            {
                flags |= 128;
            }
            writer.Write(flags);
            writer.Write(numPuriumGens);
            flags = 0;
            if (downedTerraSpirit)
            {
                flags |= 1;
            }
            if (downedBlushie)
            {
                flags |= 2;
            }
            writer.Write(flags);
            writer.Write(terraDeaths);
            writer.Write((byte)(terraCheckpoint1 + 16 * terraCheckpoint2));
            writer.Write((byte)(terraCheckpoint3 + 16 * terraCheckpointS));
            writer.Write(blushieCheckpoint);
        }

        public override void NetReceive(BinaryReader reader)
        {
            byte flags = reader.ReadByte();
            eclipsePassed = ((flags & 1) == 1);
            pumpkinMoonPassed = ((flags & 2) == 2);
            snowMoonPassed = ((flags & 4) == 4);
            downedPhantom = ((flags & 8) == 8);
            downedAbomination = ((flags & 16) == 16);
            elementalUnleash = ((flags & 32) == 32);
            downedPuritySpirit = ((flags & 64) == 64);
            downedChaosSpirit = ((flags & 128) == 128);
            numPuriumGens = reader.ReadInt32();
            flags = reader.ReadByte();
            downedTerraSpirit = ((flags & 1) == 1);
            downedBlushie = ((flags & 2) == 2);
            terraDeaths = reader.ReadInt32();
            byte val = reader.ReadByte();
            terraCheckpoint1 = val % 16;
            terraCheckpoint2 = val / 16;
            val = reader.ReadByte();
            terraCheckpoint3 = val % 16;
            terraCheckpointS = val / 16;
            blushieCheckpoint = reader.ReadSingle();
            FixCheckpoints();
        }

        public override void ResetNearbyTileEffects()
        {
            BluemagicPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<BluemagicPlayer>();
            modPlayer.voidMonolith = false;
            modPlayer.saltLamp = false;
        }

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            Main.SceneMetrics.MushroomTileCount += tileCounts[Mod.Find<ModTile>("Shroomstone").Type];
            Main.SceneMetrics.MushroomTileCount += tileCounts[Mod.Find<ModTile>("Shroomsand").Type];
            Main.SceneMetrics.MushroomTileCount += tileCounts[Mod.Find<ModTile>("DarkBlueIce").Type];
        }

        public override void PostUpdateWorld()
        {
            Bluemagic.UpdatePureColor();
            WorldReaver.UpdateGlitchText();
            WorldReaverData.Update();
            if (Main.eclipse)
            {
                eclipsePassed = true;
            }
            if (Main.pumpkinMoon && NPC.waveNumber >= 15)
            {
                pumpkinMoonPassed = true;
            }
            if (Main.snowMoon && NPC.waveNumber >= 15)
            {
                snowMoonPassed = true;
            }
            BlushieBoss.BlushieBoss.Update();
        }

        public static void GenPurium()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient || WorldGen.noTileActions || WorldGen.gen || !NPC.downedMoonlord)
            {
                return;
            }
            numPuriumGens += 1;
            for (double k = 0; k < (Main.maxTilesX - 200) * (Main.maxTilesY - 150 - (int)Main.rockLayer) / 10000.0 / (double)numPuriumGens; k += 1.0)
            {
                WorldGen.OreRunner(WorldGen.genRand.Next(100, Main.maxTilesX - 100), WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 150), (double)WorldGen.genRand.Next(4, 8), WorldGen.genRand.Next(4, 8), (ushort)Bluemagic.Instance.Find<ModTile>("PuriumOre").Type);
            }
            Bluemagic.NewText("Mods.Bluemagic.PuriumOreGen", 100, 220, 100);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }

        public override void PostDrawTiles()
        {
            if (BlushieBoss.BlushieBoss.Active)
            {
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
                BlushieBoss.BlushieBoss.DrawArena(Main.spriteBatch);
                Main.spriteBatch.End();
            }
        }
    }
}
