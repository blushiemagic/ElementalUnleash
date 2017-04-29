using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Buffs
{
	public class PurityShieldMount : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffName[this.Type] = "Purity Shield";
			Main.buffTip[this.Type] = "The Spirit of Purity lends you power";
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(mod.MountType("PurityShield"), player);
			player.buffTime[buffIndex] = 10;
			BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>(mod);
			modPlayer.puriumShieldChargeRate += 0.1f;
			modPlayer.purityShieldMount = true;
		}
	}
}
