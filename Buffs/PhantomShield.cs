using System;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Buffs
{
	public class PhantomShield : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffName[Type] = "Phantom Shield";
			Main.buffTip[Type] = "7% reduced damage taken";
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.endurance += 0.07f;
		}
	}
}