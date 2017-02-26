using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic;

namespace Bluemagic.Buffs
{
	public class EtherealFlames : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffName[this.Type] = "Ethereal Flames";
			Main.buffTip[this.Type] = "Losing life";
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<BluemagicPlayer>(mod).eFlames = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetModInfo<BluemagicNPCInfo>(mod).eFlames = true;
		}
	}
}
