using System;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Buffs.Summons
{
    public class MiniPaladin : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Mini Paladin");
            Description.SetDefault("A mini paladin will fight for you.");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
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