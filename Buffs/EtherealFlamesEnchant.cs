using System;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Buffs
{
	public class EtherealFlamesEnchant : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Weapon Imbue: Ethereal Flames");
			Description.SetDefault("Melee attacks inflict ethereal flames");
			Main.meleeBuff[Type] = true;
			Main.persistentBuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<BluemagicPlayer>(mod).customMeleeEnchant = 1;
		}
	}
}