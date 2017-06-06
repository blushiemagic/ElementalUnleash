using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class PuriumVisor : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("15% increased ranged damage, 10% increased ranged critical strike chance"
				+ "\n25% chance to not consume ammo");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.defense = 24;
			item.rare = 11;
			item.value = Item.sellPrice(0, 6, 0, 0);
		}

		public override void UpdateEquip(Player player)
		{
			player.rangedDamage += 0.15f;
			player.rangedCrit += 10;
			player.ammoCost75 = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PuriumBar", 12);
			recipe.AddTile(null, "PuriumAnvil");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}