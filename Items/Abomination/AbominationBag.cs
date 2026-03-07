using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination
{
    public class AbominationBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Treasure Bag");
            // Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
            ItemID.Sets.BossBag[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Cyan;
            Item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(Mod.Find<ModItem>("AbominationMask").Type, 7));
            itemLoot.Add(ItemDropRule.Common(Mod.Find<ModItem>("MoltenDrill").Type));
            itemLoot.Add(ItemDropRule.Common(Mod.Find<ModItem>("DimensionalChest").Type));
            itemLoot.Add(ItemDropRule.Common(Mod.Find<ModItem>("MoltenBar").Type, 1, 5, 5));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SixColorShield>()));
        }
    }
}