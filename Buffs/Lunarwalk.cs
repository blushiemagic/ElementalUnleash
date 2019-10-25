using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Buffs
{
    public class Lunarwalk : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Lunarwalk");
            Description.SetDefault("30% increased movement speed - Press UP to reverse gravity");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffImmune[BuffID.Swiftness] = true;
            player.moveSpeed += 0.3f;
            player.gravControl = true;
        }
    }
}