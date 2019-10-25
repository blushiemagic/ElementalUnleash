using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Buffs.PuritySpirit
{
    public class HeroTwo : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Hero");
            Description.SetDefault("You are a hero of Terraria! (2 Lives)");
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            canBeCleared = false;
        }
    }
}
