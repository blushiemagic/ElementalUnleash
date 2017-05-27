using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Buffs
{
	public class Lunarwalk : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffName[Type] = "Lunarwalk";
			Main.buffTip[Type] = "30% increased movement speed - Press UP to reverse gravity";
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffImmune[BuffID.Swiftness] = true;
			player.moveSpeed += 0.3f;
			player.gravControl = true;
		}
	}
}