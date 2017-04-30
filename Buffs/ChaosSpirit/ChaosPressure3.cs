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
			Main.buffTip[this.Type] = "Slowly losing life, reduces purity shield fill rate";
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			longerExpertDebuff = true;
			canBeCleared = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>(mod);
			modPlayer.chaosPressure = 3;
			modPlayer.puriumShieldChargeRate -= 0.3f;
		}
	}
}
