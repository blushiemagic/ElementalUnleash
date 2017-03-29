using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
	public class EndlessFlamingQuiver : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Endless Flaming Quiver";
			item.shootSpeed = 3.5f;
			item.shoot = ProjectileID.FireArrow;
			item.damage = 7;
			item.width = 26;
			item.height = 26;
			item.ranged = true;
			item.ammo = AmmoID.Arrow;
			item.knockBack = 2f;
			item.rare = 11;
			item.value = Item.sellPrice(0, 20, 0, 0);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.EndlessQuiver);
			recipe.AddIngredient(null, "InfinityCrystal");
			recipe.AddIngredient(ItemID.FlamingArrow, 3996);
			recipe.AddTile(null, "ElementalPurge");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}