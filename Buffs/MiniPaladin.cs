using System;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Buffs
{
	public class MiniPaladin : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffName[Type] = "Mini Paladin";
			Main.buffTip[Type] = "A mini paladin will fight for you.";
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>(mod);
			if (player.ownedProjectileCounts[mod.ProjectileType("MiniPaladin")] > 0)
			{
				modPlayer.paladinMinion = true;
			}
			if (!modPlayer.paladinMinion)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
			else
			{
				player.buffTime[buffIndex] = 18000;
			}
		}
	}
}