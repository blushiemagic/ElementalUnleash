using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic;

namespace Bluemagic.Buffs
{
	public class Suppression3 : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffName[this.Type] = "Suppression";
			Main.buffTip[this.Type] = "75% reduced damage";
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<BluemagicPlayer>(mod).suppression = 0.75f;
			if (player.buffTime[buffIndex] == 1)
			{
				player.buffType[buffIndex] = mod.BuffType("Suppression2");
				player.buffTime[buffIndex] = 300;
			}
		}
	}
}
