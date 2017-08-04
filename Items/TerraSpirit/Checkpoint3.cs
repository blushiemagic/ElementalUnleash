using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.TerraSpirit
{
	public class Checkpoint3 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Third Ritual");
			Tooltip.SetDefault("Enrages the Spirit of Purity at 33% health"
				+ "\nCan be reused infinitely"
				+ "\nEach player starts with {0} lives");
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.rare = 12;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = 4;
			item.UseSound = SoundID.Item44;
		}

		public override bool CanUseItem(Player player)
		{
			return BluemagicWorld.terraCheckpoint3 > 0 && !NPC.AnyNPCs(mod.NPCType("TerraSpirit")) && !NPC.AnyNPCs(mod.NPCType("TerraSpirit2"));
		}

		public override bool UseItem(Player player)
		{
			if (Main.netMode != 1)
			{
				NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, mod.NPCType("TerraSpirit"), 0, 8f);
			}
			return true;
		}

		public override void ModifyTooltips(List<TooltipLine> lines)
		{
			for (int k = 0; k < lines.Count; k++)
			{
				if (lines[k].mod == "Terraria" && lines[k].Name == "Tooltip2")
				{
					lines[k].text = string.Format(lines[k].text, BluemagicWorld.terraCheckpoint3);
				}
			}
		}
	}
}