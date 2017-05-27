using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Buffs
{
	public class Bubbleshield : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffName[Type] = "Bubbleshield";
			Main.buffTip[Type] = "Increases defense by 12 and reduces damage taken by 12%";
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffImmune[BuffID.Ironskin] = true;
			player.buffImmune[BuffID.Endurance] = true;
			player.statDefense += 12;
			player.endurance += 0.12f;
		}
	}
}