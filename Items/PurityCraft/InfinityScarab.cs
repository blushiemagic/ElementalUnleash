using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
	public class InfinityScarab : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Greatly increases your max number of minions"
				+ "\nGreatly increases the damage and knockback of your minions");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 28;
			item.accessory = true;
			item.rare = 11;
			item.value = Item.sellPrice(0, 30, 0, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.minionDamage += 0.3f;
			player.minionKB += 2.5f;
			player.maxMinions += 3;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PapyrusScarab);
			recipe.AddIngredient(null, "SummonerSeal");
			recipe.AddIngredient(null, "InfinityCrystal");
			recipe.AddTile(null, "PuriumAnvil");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}