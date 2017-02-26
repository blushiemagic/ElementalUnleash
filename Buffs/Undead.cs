using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic;

namespace Bluemagic.Buffs
{
	public class Undead : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffName[this.Type] = "Undead";
			Main.buffTip[this.Type] = "Recovering harms you";
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<BluemagicPlayer>(mod).badHeal = true;
		}
	}
}
