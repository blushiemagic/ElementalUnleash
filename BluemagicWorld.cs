using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Bluemagic
{
	public class BluemagicWorld : ModWorld
	{
		private const int saveVersion = 0;
		public static bool downedAbomination = false;
		public static int downedAbomination2 = 0;
		public static bool downedPuritySpirit = false;
		public static bool downedChaosSpirit = false;

		public override void Initialize()
		{
			downedAbomination = false;
			downedAbomination2 = 0;
			downedPuritySpirit = false;
			downedChaosSpirit = false;
		}

		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			tag["downedAbomination"] = downedAbomination;
			tag["downedAbomination2"] = downedAbomination2;
			tag["downedPuritySpirit"] = downedPuritySpirit;
			tag["downedChaosSpirit"] = downedChaosSpirit;
			return tag;
		}

		public override void Load(TagCompound tag)
		{
			downedAbomination = tag.GetBool("downedAbomination");
			downedAbomination2 = tag.GetInt("downedAbomination2");
			downedPuritySpirit = tag.GetBool("downedPuritySpirit");
			downedChaosSpirit = tag.GetBool("downedChaosSpirit");
		}

		public override void NetSend(BinaryWriter writer)
		{
			byte flags = 0;
			if (downedAbomination)
			{
				flags |= 1;
			}
			if (downedPuritySpirit)
			{
				flags |= 2;
			}
			if (downedChaosSpirit)
			{
				flags |= 4;
			}
			writer.Write(flags);
			writer.Write(downedAbomination2);
		}

		public override void NetReceive(BinaryReader reader)
		{
			byte flags = reader.ReadByte();
			downedAbomination = ((flags & 1) == 1);
			downedPuritySpirit = ((flags & 2) == 2);
			downedChaosSpirit = ((flags & 4) == 4);
			downedAbomination2 = reader.ReadInt32();
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
		}

		public override void PostUpdate()
		{
			Bluemagic.UpdatePureColor();
		}

		public static void GenPurium()
		{
			if (Main.netMode == 1 || WorldGen.noTileActions || WorldGen.gen || !NPC.downedMoonlord)
			{
				return;
			}
			downedAbomination2 += 1;
			for (double k = 0; k < (Main.maxTilesX - 200) * (Main.maxTilesY - 150 - (int)Main.rockLayer) / 10000.0 / (double)downedAbomination2; k += 1.0)
			{
				WorldGen.OreRunner(WorldGen.genRand.Next(100, Main.maxTilesX - 100), WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 150), (double)WorldGen.genRand.Next(4, 8), WorldGen.genRand.Next(4, 8), (ushort)Bluemagic.Instance.TileType("PuriumOre"));
			}
			if (Main.netMode == 0)
			{
				Main.NewText("The elements have been unleashed!", 100, 220, 100);
			}
			else if (Main.netMode == 2)
			{
				NetMessage.SendData(MessageID.ChatText, -1, -1, "The elements have been unleashed!", 255, 100, 220, 100);
				NetMessage.SendData(MessageID.WorldInfo);
			}
		}
	}
}
