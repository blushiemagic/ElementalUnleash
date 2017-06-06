using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons
{
	public class PuriumLightbeam : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 40;
			item.scale = 1.1f;
			item.useStyle = 5;
			item.useAnimation = 25;
			item.useTime = 25;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.damage = 79;
			item.knockBack = 5f;
			item.autoReuse = false;
			item.useTurn = false;
			item.rare = 11;
			item.melee = true;
			item.value = Item.sellPrice(0, 12, 0, 0);
			item.UseSound = SoundID.Item9;
			item.shoot = mod.ProjectileType("PuriumSpear");
			item.shootSpeed = 5f;
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