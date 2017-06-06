using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
	public class AvengerSeal : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("30% increased damage");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.accessory = true;
			item.rare = 11;
			item.value = Item.sellPrice(0, 30, 0, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeDamage += 0.3f;
			player.rangedDamage += 0.3f;
			player.magicDamage += 0.3f;
			player.minionDamage += 0.3f;
			player.thrownDamage += 0.3f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "WarriorSeal");
			recipe.AddIngredient(null, "RangerSeal");
			recipe.AddIngredient(null, "SorcerorSeal");
			recipe.AddIngredient(null, "SummonerSeal");
			recipe.AddTile(null, "PuriumAnvil");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}