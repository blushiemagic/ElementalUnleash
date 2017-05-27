using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Buffs
{
	public class Sunlight : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffName[Type] = "Sunlight";
			Main.buffTip[Type] = "Emitting light and increased night vision";
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffImmune[BuffID.Shine] = true;
			Lighting.AddLight(player.Center, 1.1f, 1.15f, 1.2f);
			player.nightVision = true;
		}
	}
}