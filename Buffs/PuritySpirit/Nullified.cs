using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic;

namespace Bluemagic.Buffs.PuritySpirit
{
	public class Nullified : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffName[this.Type] = "Nullified";
			Main.buffTip[this.Type] = "Your abilities are nullified";
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<BluemagicPlayer>(mod).nullified = true;
		}
	}
}
