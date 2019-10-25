using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Buffs
{
    public class Salty : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Salty");
            Description.SetDefault("Greatly increased speed but decreased defense");
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 0.4f;
            player.statDefense -= 6;
        }
    }
}