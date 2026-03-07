using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic;

namespace Bluemagic.Buffs
{
    public class SaltLamp : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Salt Lamp");
            // Description.SetDefault("Increase defense by 4");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
    }
}
