using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Buffs
{
    public class NoMount : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Cursed Mount");
            Description.SetDefault("You cannot use your mount");
            Main.buffNoSave[Type] = true;
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            canBeCleared = false;
        }
    }
}
