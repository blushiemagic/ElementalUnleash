using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Buffs.Summons
{
    public class PurityShieldMount : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Purity Shield");
            Description.SetDefault("The Spirit of Purity lends you power");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(mod.MountType("PurityShield"), player);
            player.buffTime[buffIndex] = 10;
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            modPlayer.puriumShieldChargeRate += 0.1f;
            modPlayer.purityShieldMount = true;
        }
    }
}
