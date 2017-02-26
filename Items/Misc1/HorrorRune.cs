using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
	public class HorrorRune : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Horror Rune";
			item.width = 12;
			item.height = 20;
			item.maxStack = 20;
			item.rare = 4;
			item.toolTip = "Toggles the horrors from above";
			item.useStyle = 4;
			item.useAnimation = 45;
			item.useTime = 45;
			item.UseSound = SoundID.Item44;
			item.consumable = true;
		}

		public override bool UseItem(Player player)
		{
			if (Main.netMode != 1)
			{
				if (Main.dayTime)
				{
					Main.eclipse = !Main.eclipse;
				}
				else
				{
					Main.bloodMoon = !Main.bloodMoon;
				}
				if (Main.netMode == 2)
				{
					NetMessage.SendData(MessageID.WorldInfo);
				}
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "HorrorDrop", 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}