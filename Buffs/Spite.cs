using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Buffs
{
	public class Spite : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Spite");
			Description.SetDefault("12% increased damage and critical strike chance");
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffImmune[BuffID.Wrath] = true;
			player.buffImmune[BuffID.Rage] = true;
			player.meleeDamage += 0.12f;
			player.rangedDamage += 0.12f;
			player.magicDamage += 0.12f;
			player.minionDamage += 0.12f;
			player.thrownDamage += 0.12f;
			player.meleeCrit += 12;
			player.rangedCrit += 12;
			player.magicCrit += 12;
			player.thrownCrit += 12;
		}
	}
}