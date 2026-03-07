using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Buffs.Summons
{
    public class VoidEmissary : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Void Emissary");
            // Description.SetDefault("The void emissary will fight alongside you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            if (player.ownedProjectileCounts[Mod.Find<ModProjectile>("VoidEmissary").Type] > 0)
            {
                modPlayer.voidEmissary = true;
            }
            if (!modPlayer.voidEmissary)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
}