using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination
{
    public class AbominationBag2 : ModItem
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
            Item.rare = ItemRarityID.Purple;
            Item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<AbominationMask>(), 7));
            itemLoot.Add(ItemDropRule.Common(Mod.Find<ModItem>("MoltenDrill").Type));
            itemLoot.Add(ItemDropRule.Common(Mod.Find<ModItem>("DimensionalChest").Type));
            itemLoot.Add(ItemDropRule.Common(Mod.Find<ModItem>("MoltenBar").Type, 1, 5, 5));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SixColorShield>()));
            itemLoot.Add(ItemDropRule.Common(Mod.Find<ModItem>("ElementalEye").Type));
            itemLoot.Add(ItemDropRule.OneFromOptions(1, new int[] { Mod.Find<ModItem>("ElementalYoyo").Type, Mod.Find<ModItem>("ElementalSprayer").Type, Mod.Find<ModItem>("EyeballTome").Type, Mod.Find<ModItem>("ElementalStaff").Type, Mod.Find<ModItem>("EyeballGlove").Type }));
        }
    }
}