using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom
{
    public class PhantomBag : ModItem
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
            item.rare = 8;
            item.expert = true;
        }

        public override int BossBagNPC => mod.NPCType("Phantom");

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            player.TryGettingDevArmor();
            if (Main.rand.Next(7) == 0)
            {
                player.QuickSpawnItem(mod.ItemType("PhantomMask"));
            }
            player.QuickSpawnItem(mod.ItemType("PhantomPlate"), Main.rand.Next(8, 13));
            int reward = 0;
            switch (Main.rand.Next(4))
            {
            case 0:
                reward = mod.ItemType("PhantomBlade");
                break;
            case 1:
                reward = mod.ItemType("SpectreGun");
                break;
            case 2:
                reward = mod.ItemType("PhantomSphere");
                break;
            case 3:
                reward = mod.ItemType("PaladinStaff");
                break;
            }
            player.QuickSpawnItem(reward);
            player.QuickSpawnItem(mod.ItemType("PhantomShield"));
        }
    }
}