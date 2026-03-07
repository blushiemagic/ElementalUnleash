using Bluemagic.PuritySpirit;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic
{
    public class ChaosScene : ModSceneEffect
    {
        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("Bluemagic:ChaosSpirit", isActive);
        }
        public override bool IsSceneEffectActive(Player player) => BluemagicPlayer.AnyChaosSpirit();

        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
    }
    public class PurityScene : ModSceneEffect
    {
        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("Bluemagic:PuritySpirit", isActive);
        }
        public override bool IsSceneEffectActive(Player player) => NPC.AnyNPCs(ModContent.NPCType<PuritySpirit.PuritySpirit>());

        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
    }

    public class VoidScene : ModSceneEffect
    {
        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("Bluemagic:MonolithVoid", isActive);
        }
        public override bool IsSceneEffectActive(Player player) => player.GetModPlayer<BluemagicPlayer>().voidMonolith && !NPC.AnyNPCs(NPCID.MoonLordCore);

        public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;
    }

	public class TerraScene : ModSceneEffect
	{
        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("Bluemagic:TerraSpirit", isActive);
        }
        public override bool IsSceneEffectActive(Player player) => BluemagicPlayer.AnyTerraSpirit();

        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
	}
}
