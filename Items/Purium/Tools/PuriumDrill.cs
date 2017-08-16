using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Tools
{
	public class PuriumDrill : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Can mine Frostbyte");
		}

		public override void SetDefaults()
		{
			item.damage = 90;
			item.melee = true;
			item.width = 20;
			item.height = 12;
			item.useTime = 6;
			item.useAnimation = 11;
			item.channel = true;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.pick = 250;
			item.tileBoost += 4;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = Item.sellPrice(0, 12, 0, 0);
			item.rare = 11;
			item.UseSound = SoundID.Item23;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("PuriumDrill");
			item.shootSpeed = 40f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PuriumBar", 15);
			recipe.AddTile(null, "PuriumAnvil");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}