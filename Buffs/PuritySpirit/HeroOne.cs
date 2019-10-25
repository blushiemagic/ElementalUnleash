using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Buffs.PuritySpirit
{
    public class HeroOne : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Hero");
            Description.SetDefault("You are a hero of Terraria!");
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            canBeCleared = false;
        }
    }
}
