using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Tools
{
	public class PuriumHammer : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Purium Hammer";
			item.damage = 130;
			item.melee = true;
			item.width = 20;
			item.height = 12;
			item.scale = 1.25f;
			item.useTime = 7;
			item.useAnimation = 23;
			item.hammer = 110;
			item.tileBoost += 4;
			item.useStyle = 1;
			item.knockBack = 8;
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