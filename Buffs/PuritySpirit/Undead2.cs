using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic;

namespace Bluemagic.Buffs
{
	public class Undead2 : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffName[this.Type] = "Undead Sickness";
			Main.buffTip[this.Type] = "You are being harmed by recovery";
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			int extra = player.buffTime[buffIndex] / 60;
			player.buffTime[buffIndex] -= extra;
			player.GetModPlayer<BluemagicPlayer>(mod).healHurt = extra + 1;
		}

		public override bool ReApply(Player player, int time, int buffIndex)
		{
			player.buffTime[buffIndex] += time;
			return true;
		}
	}
}
