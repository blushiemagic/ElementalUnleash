using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom
{
	public class PhantomHammer : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Phantom Hammer";
			item.width = 14;
			item.height = 28;
			item.useStyle = 1;
			item.useAnimation = 15;
			item.useTime = 15;
			item.damage = 78;
			item.knockBack = 7f;
			item.autoReuse = true;
			item.rare = 8;
			item.thrown = true;
			item.value = Item.sellPrice(0, 0, 0, 10);
			item.UseSound = SoundID.Item1;
			item.shoot = mod.ProjectileType("PhantomHammerFriendly");
			item.shootSpeed = 10f;
			item.maxStack = 999;
			item.consumable = true;
			item.noMelee = true;
			item.noUseGraphic = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PhantomPlate");
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 333);
			recipe.AddRecipe();
		}
	}
}