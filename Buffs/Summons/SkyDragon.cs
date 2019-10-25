using System;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Buffs.Summons
{
    public class SkyDragon : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Sky Dragon");
            Description.SetDefault("The Sky Dragon will absolutely obliterate everything for you.");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            if (player.ownedProjectileCounts[mod.ProjectileType("SkyDragonHead")] > 0)
            {
                modPlayer.skyDragon = true;
            }
            if (!modPlayer.skyDragon)
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