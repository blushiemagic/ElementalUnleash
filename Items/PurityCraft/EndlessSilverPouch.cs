using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
	public class EndlessSilverPouch : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Endless Silver Pouch";
			item.shootSpeed = 4f;
			item.shoot = ProjectileID.Bullet;
			item.damage = 9;
			item.width = 26;
			item.height = 26;
			item.ranged = true;
			item.ammo = ProjectileID.Bullet;
			item.knockBack = 3f;
			item.rare = 11;
			item.value = Item.sellPrice(0, 20, 0, 0);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.EndlessMusketPouch);
			recipe.AddIngredient(null, "InfinityCrystal");
			recipe.AddIngredient(ItemID.SilverBullet, 3996);
			recipe.AddTile(null, "ElementalPurge");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}