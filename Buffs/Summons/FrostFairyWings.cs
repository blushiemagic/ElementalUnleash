using System;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Buffs.Summons
{
	public class FrostFairyWings : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Wings of the Frost Fairy");
			Description.SetDefault("The Wings of the Frost Fairy will fight for you.");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>(mod);
			if (player.ownedProjectileCounts[mod.ProjectileType("FrostFairyWingsProj")] > 0)
			{
				modPlayer.frostFairy = true;
			}
			if (!modPlayer.frostFairy)
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