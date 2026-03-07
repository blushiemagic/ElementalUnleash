using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic;

namespace Bluemagic.Buffs.PuritySpirit
{
    public class Undead : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Undead");
            // Description.SetDefault("Recovering harms you");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<BluemagicPlayer>().badHeal = true;
        }
    }
}
