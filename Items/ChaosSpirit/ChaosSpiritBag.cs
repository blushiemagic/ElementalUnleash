using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.ChaosSpirit
{
    public class ChaosSpiritBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 24;
            item.height = 24;
            item.rare = 11;
            item.expert = true;
        }

        public override int BossBagNPC => mod.NPCType("ChaosSpirit");

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            player.TryGettingDevArmor();
            player.TryGettingDevArmor();
            int choice = Main.rand.Next(7);
            if (choice == 0)
            {
                player.QuickSpawnItem(mod.ItemType("ChaosSpiritMask"));
            }
            else if (choice == 1)
            {
                player.QuickSpawnItem(mod.ItemType("CataclysmMask"));
            }
            player.QuickSpawnItem(mod.ItemType("ChaosCrystal"));
            player.QuickSpawnItem(mod.ItemType("CataclysmCrystal"));
        }
    }
}