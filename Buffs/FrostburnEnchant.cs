using System;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Buffs
{
	public class FrostburnEnchant : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Weapon Imbue: Frostburn");
			Description.SetDefault("Melee attacks inflict frostburn");
			Main.meleeBuff[Type] = true;
			Main.persistentBuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<BluemagicPlayer>().customMeleeEnchant = 2;
		}
	}
}