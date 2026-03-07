using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom
{
    public class PhantomBag : ModItem
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
            Item.rare = ItemRarityID.Yellow;
            Item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PhantomMask>(), 7));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PhantomPlate>(), 1, 8, 12));
            itemLoot.Add(ItemDropRule.OneFromOptions(1, [ModContent.ItemType<PhantomBlade>(), ModContent.ItemType<SpectreGun>(), ModContent.ItemType<PhantomSphere>(), ModContent.ItemType<PaladinStaff>()]));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PhantomShield>()));
        }
    }
}