using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons
{
	public class PuriumStaff : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Purium Staff";
			item.width = 26;
			item.height = 28;
			item.toolTip = "Casts a controllable sphere of purity";
			item.useStyle = 1;
			item.useAnimation = 27;
			item.useTime = 27;
			item.channel = true;
			item.noMelee = true;
			item.damage = 757;
			item.knockBack = 6.5f;
			item.autoReuse = false;
			item.useTurn = false;
			item.rare = 11;
			item.melee = true;
			item.value = Item.sellPrice(0, 12, 0, 0);
			item.UseSound = SoundID.Item28;
			item.shoot = mod.ProjectileType("PuriumStaff");
			item.mana = 18;
			item.shootSpeed = 6f;
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