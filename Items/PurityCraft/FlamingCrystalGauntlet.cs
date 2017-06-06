using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
	[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff)]
	public class FlamingCrystalGauntlet : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases melee knockback and inflicts fire damage on attack"
				+ "\n25% increased melee damage and speed");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 24;
			item.accessory = true;
			item.rare = 11;
			item.value = Item.sellPrice(0, 30, 0, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.kbGlove = true;
			player.meleeSpeed += 0.25f;
			player.meleeDamage += 0.25f;
			player.magmaStone = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FireGauntlet);
			recipe.AddIngredient(null, "WarriorSeal");
			recipe.AddIngredient(null, "InfinityCrystal");
			recipe.AddTile(null, "PuriumAnvil");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}