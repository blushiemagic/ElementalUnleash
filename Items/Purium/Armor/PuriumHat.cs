using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class PuriumHat : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("15% increased throwing damage, 10% increased throwing critical strike chance"
				+ "\n20% increased throwing velocity, 33% chance to not consume thrown item");
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
			player.thrownDamage += 0.15f;
			player.thrownCrit += 10;
			player.thrownVelocity += 0.2f;
			player.thrownCost33 = true;
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			drawAltHair = true;
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