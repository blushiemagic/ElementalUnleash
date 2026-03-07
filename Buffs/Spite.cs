using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Buffs
{
    public class Spite : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Spite");
            // Description.SetDefault("12% increased damage and critical strike chance");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffImmune[BuffID.Wrath] = true;
            player.buffImmune[BuffID.Rage] = true;
            player.GetDamage(DamageClass.Melee) += 0.12f;
            player.GetDamage(DamageClass.Ranged) += 0.12f;
            player.GetDamage(DamageClass.Magic) += 0.12f;
            player.GetDamage(DamageClass.Summon) += 0.12f;
            player.GetDamage(DamageClass.Throwing) += 0.12f;
            player.GetCritChance(DamageClass.Melee) += 12;
            player.GetCritChance(DamageClass.Ranged) += 12;
            player.GetCritChance(DamageClass.Magic) += 12;
            player.GetCritChance(DamageClass.Throwing) += 12;
        }
    }
}