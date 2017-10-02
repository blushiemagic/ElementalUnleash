using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items
{
	public class PinkSaltedSalt : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Probably not healthy to eat");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 20;
			item.maxStack = 30;
			item.rare = 8;
			item.value = 300;
			item.UseSound = SoundID.Item2;
			item.useStyle = 2;
			item.useTurn = true;
			item.useAnimation = 17;
			item.useTime = 17;
			item.consumable = true;
			item.buffType = mod.BuffType("PinkSalty");
			item.buffTime = 7200;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod, "PinkSalt", 5);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}