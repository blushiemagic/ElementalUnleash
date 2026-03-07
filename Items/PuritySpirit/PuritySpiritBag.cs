using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit
{
    public class PuritySpiritBag : ModItem
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
            itemLoot.Add(ItemDropRule.OneFromOptions(1, [ ModContent.ItemType<PuritySpiritMask>(), ModContent.ItemType<BunnyMask>(), ItemID.Bunny, ItemID.Bunny, ItemID.Bunny, ItemID.Bunny, ItemID.Bunny]));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<InfinityCrystal>(), 1, 2, 2));
            itemLoot.Add(ItemDropRule.OneFromOptions(1, [ ModContent.ItemType<DanceOfBlades>(), ModContent.ItemType<CleanserBeam>(), ModContent.ItemType<PrismaticShocker>(), ModContent.ItemType<VoidEmblem>()]));
        }
    }
}