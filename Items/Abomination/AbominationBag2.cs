using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination
{
	public class AbominationBag2 : ModItem
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

		public override int BossBagNPC => mod.NPCType("Abomination");

		public override bool CanRightClick()
		{
			return true;
		}

		public override void OpenBossBag(Player player)
		{
			player.TryGettingDevArmor();
			if (Main.rand.Next(7) == 0)
			{
				player.QuickSpawnItem(mod.ItemType("AbominationMask"));
			}
			player.QuickSpawnItem(mod.ItemType("MoltenDrill"));
			player.QuickSpawnItem(mod.ItemType("DimensionalChest"));
			player.QuickSpawnItem(mod.ItemType("MoltenBar"), 5);
			player.QuickSpawnItem(mod.ItemType("SixColorShield"));
			player.QuickSpawnItem(mod.ItemType("ElementalEye"));
			switch (Main.rand.Next(5))
			{
			case 0:
				player.QuickSpawnItem(mod.ItemType("ElementalYoyo"));
				break;
			case 1:
				player.QuickSpawnItem(mod.ItemType("ElementalSprayer"));
				break;
			case 2:
				player.QuickSpawnItem(mod.ItemType("EyeballTome"));
				break;
			case 3:
				player.QuickSpawnItem(mod.ItemType("ElementalStaff"));
				break;
			case 4:
				player.QuickSpawnItem(mod.ItemType("EyeballGlove"));
				break;
			}
		}
	}
}