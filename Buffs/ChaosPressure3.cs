using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic;

namespace Bluemagic.Buffs
{
	public class ChaosPressure3 : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffName[this.Type] = "Chaos Pressure";
			Main.buffTip[this.Type] = "Slowly losing life";
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<BluemagicPlayer>(mod).chaosPressure = 3;
		}
	}
}
