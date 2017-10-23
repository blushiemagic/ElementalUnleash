using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class PuriumHeadgear : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("15% increased magic damage, 10% increased magic critical strike chance"
				+ "\n+80 maximum mana and 15% reduced mana usage");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.defense = 10;
			item.rare = 11;
			item.value = Item.sellPrice(0, 6, 0, 0);
		}

		public override void UpdateEquip(Player player)
		{
			player.magicDamage += 0.15f;
			player.magicCrit += 10;
			player.statManaMax2 += 80;
			player.manaCost -= 0.15f;
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