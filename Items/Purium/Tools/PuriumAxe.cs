using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Tools
{
	public class PuriumAxe : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Purium Axe";
			item.damage = 110;
			item.melee = true;
			item.width = 20;
			item.height = 12;
			item.scale = 1.15f;
			item.useTime = 7;
			item.useAnimation = 15;
			item.axe = 170 / 5;
			item.tileBoost += 4;
			item.useStyle = 1;
			item.knockBack = 7;
			item.value = Item.sellPrice(0, 8, 0, 0);
			item.rare = 11;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
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