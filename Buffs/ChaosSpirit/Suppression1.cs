using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic;

namespace Bluemagic.Buffs.ChaosSpirit
{
	public class Suppression1 : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Suppression");
			Description.SetDefault("25% reduced damage");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<BluemagicPlayer>(mod).suppression = 0.25f;
		}
	}
}
