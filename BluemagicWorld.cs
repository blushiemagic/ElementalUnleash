using System.IO;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Bluemagic.BlushieBoss;

namespace Bluemagic
{
	public class BluemagicWorld : ModWorld
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
		public static bool downedBlushiePhase2 = false;
		public static bool downedBlushie = false;
		public static bool noHitBlushie = false;

		public override void Initialize()
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
			downedBlushiePhase2 = false;
			downedBlushie = false;
			noHitBlushie = false;
		}

		private void FixTerraCheckpoints()
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
		}

		public override TagCompound Save()
		{
			FixTerraCheckpoints();
			TagCompound tag = new TagCompound();
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
			tag["downedBlushiePhase2"] = downedBlushiePhase2;
			tag["downedBlushie"] = downedBlushie;
			tag["noHitBlushie"] = noHitBlushie;
			return tag;
		}

		public override void Load(TagCompound tag)
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
			downedBlushiePhase2 = tag.GetBool("downedBlushiePhase2");
			downedBlushie = tag.GetBool("downedBlushie");
			noHitBlushie = tag.GetBool("noHitBlushie");
			FixTerraCheckpoints();
		}

		public override void NetSend(BinaryWriter writer)
		{
			FixTerraCheckpoints();
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
			if (downedBlushiePhase2)
			{
				flags |= 2;
			}
			if (downedBlushie)
			{
				flags |= 4;
			}
			if (noHitBlushie)
			{
				flags |= 8;
			}
			writer.Write(flags);
			writer.Write(terraDeaths);
			writer.Write((byte)(terraCheckpoint1 + 16 * terraCheckpoint2));
			writer.Write((byte)(terraCheckpoint3 + 16 * terraCheckpointS));
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
			downedBlushiePhase2 = ((flags & 2) == 2);
			downedBlushie = ((flags & 4) == 4);
			noHitBlushie = ((flags & 8) == 8);
			terraDeaths = reader.ReadInt32();
			byte val = reader.ReadByte();
			terraCheckpoint1 = val % 16;
			terraCheckpoint2 = val / 16;
			val = reader.ReadByte();
			terraCheckpoint3 = val % 16;
			terraCheckpointS = val / 16;
			FixTerraCheckpoints();
		}

		public override void LoadLegacy(BinaryReader reader)
		{
			reader.ReadInt32();
			byte flags = reader.ReadByte();
			downedAbomination = ((flags & 1) == 1);
			downedPuritySpirit = ((flags & 2) == 2);
			downedChaosSpirit = ((flags & 4) == 4);
		}

		public override void ResetNearbyTileEffects()
		{
			BluemagicPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<BluemagicPlayer>(mod);
			modPlayer.voidMonolith = false;
			modPlayer.saltLamp = false;
		}

		public override void PostUpdate()
		{
			Bluemagic.UpdatePureColor();
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
			if (Main.netMode == 1 || WorldGen.noTileActions || WorldGen.gen || !NPC.downedMoonlord)
			{
				return;
			}
			numPuriumGens += 1;
			for (double k = 0; k < (Main.maxTilesX - 200) * (Main.maxTilesY - 150 - (int)Main.rockLayer) / 10000.0 / (double)numPuriumGens; k += 1.0)
			{
				WorldGen.OreRunner(WorldGen.genRand.Next(100, Main.maxTilesX - 100), WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 150), (double)WorldGen.genRand.Next(4, 8), WorldGen.genRand.Next(4, 8), (ushort)Bluemagic.Instance.TileType("PuriumOre"));
			}
			Bluemagic.NewText("Mods.Bluemagic.PuriumOreGen", 100, 220, 100);
			if (Main.netMode == 2)
			{
				NetMessage.SendData(MessageID.WorldData);
			}
		}

		public override void PostDrawTiles()
		{
			if (BlushieBoss.BlushieBoss.Active)
			{
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, (RasterizerState)typeof(Main).GetField("Rasterizer", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Main.instance), null, Main.Transform);
				BlushieBoss.BlushieBoss.DrawArena(Main.spriteBatch);
				Main.spriteBatch.End();
			}
		}
	}
}
