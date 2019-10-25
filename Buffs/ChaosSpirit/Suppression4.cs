using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic;

namespace Bluemagic.Buffs.ChaosSpirit
{
    public class Suppression4 : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Suppression");
            Description.SetDefault("100% reduced damage");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<BluemagicPlayer>().suppression = 1f;
            if (player.buffTime[buffIndex] == 1)
            {
                player.buffType[buffIndex] = mod.BuffType("Suppression3");
                player.buffTime[buffIndex] = 300;
            }
        }
    }
}
