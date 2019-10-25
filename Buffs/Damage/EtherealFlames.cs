using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic;

namespace Bluemagic.Buffs.Damage
{
	public class EtherealFlames : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Ethereal Flames");
			Description.SetDefault("Losing life");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<BluemagicPlayer>().eFlames = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<BluemagicNPC>().eFlames = true;
		}
	}
}
