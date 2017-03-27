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
		public static bool downedPuritySpirit = false;
		public static bool downedChaosSpirit = false;

		public override void Initialize()
		{
			downedAbomination = false;
			downedPuritySpirit = false;
			downedChaosSpirit = false;
		}

		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			tag["downedAbomination"] = downedAbomination;
			tag["downedPuritySpirit"] = downedPuritySpirit;
			tag["downedChaosSpirit"] = downedChaosSpirit;
			return tag;
		}

		public override void Load(TagCompound tag)
		{
			downedAbomination = tag.GetBool("downedAbomination");
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
			writer.Write(flags):
		}

		public override void NetReceive(BinaryReader reader)
		{
			byte flags = reader.ReadByte();
			downedAbomination = ((flags & 1) == 1);
			downedPuritySpirit = ((flags & 2) == 2);
			downedChaosSpirit = ((flags & 4) == 4);
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
	}
}
