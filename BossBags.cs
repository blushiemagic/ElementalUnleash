using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic
{
    public class BossBags : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.FishronBossBag)
            {
                itemLoot.Add(ItemDropRule.Common(Mod.Find<ModItem>("Bubble").Type, 1, 8, 12));
            }
        }
    }
}