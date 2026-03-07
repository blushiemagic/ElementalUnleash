using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic;

namespace Bluemagic.Buffs.ChaosSpirit
{
    public class Suppression2 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Suppression");
            // Description.SetDefault("50% reduced damage");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<BluemagicPlayer>().suppression = 0.5f;
            if (player.buffTime[buffIndex] == 1)
            {
                player.buffType[buffIndex] = Mod.Find<ModBuff>("Suppression1").Type;
                player.buffTime[buffIndex] = 300;
            }
        }
    }
}
