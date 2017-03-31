using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons
{
	public class PuriumSlicer : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Purium Slicer";
			item.width = 40;
			item.height = 40;
			item.scale = 1.2f;
			item.useStyle = 1;
			item.useAnimation = 20;
			item.useTime = 12;
			item.damage = 502;
			item.knockBack = 4.5f;
			item.autoReuse = true;
			item.useTurn = false;
			item.rare = 11;
			item.melee = true;
			item.value = Item.sellPrice(0, 12, 0, 0);
			item.UseSound = SoundID.Item1;
			item.shoot = mod.ProjectileType("PuriumSlice");
			item.shootSpeed = 0.5f;
			projOnSwing = true;
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