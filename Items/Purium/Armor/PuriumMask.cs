using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class PuriumMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("50% increased minion damage"
				+ "\nIncreases maximum number of minions by 4");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.defense = 9;
			item.rare = 11;
			item.value = Item.sellPrice(0, 6, 0, 0);
		}

		public override void UpdateEquip(Player player)
		{
			player.minionDamage += 0.5f;
			player.maxMinions += 4;
		}

		public override bool DrawHead()
		{
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PuriumBar", 10);
			recipe.AddTile(null, "PuriumAnvil");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}