using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination
{
	public class EyeballTome : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Move while using for the best results."
				+ "\nYo I heard you like debuffs, so I...");
		}

		public override void SetDefaults()
		{
			item.autoReuse = true;
			item.rare = 10;
			item.mana = 6;
			item.UseSound = null;
			item.noMelee = true;
			item.useStyle = 4;
			item.damage = 164;
			item.useAnimation = 10;
			item.useTime = 5;
			item.width = 24;
			item.height = 28;
			item.shoot = mod.ProjectileType("EyeballTome");
			item.shootSpeed = 0f;
			item.knockBack = 4f;
			item.magic = true;
			item.value = Item.sellPrice(0, 15, 0, 0);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position, Vector2.Zero, type, damage, knockBack, Main.myPlayer, Main.rand.Next(6));
			return false;
		}

		public override void AddRecipes()
		{
			if (Bluemagic.Sushi != null)
			{
				ModRecipe recipe;

				recipe = new ModRecipe(mod);
				recipe.AddIngredient(null, "ElementalYoyo");
				recipe.AddIngredient(Bluemagic.Sushi.ItemType("SwapToken"));
				recipe.AddTile(TileID.TinkerersWorkbench);
				recipe.SetResult(this);
				recipe.AddRecipe();

				recipe = new ModRecipe(mod);
				recipe.AddIngredient(null, "ElementalSprayer");
				recipe.AddIngredient(Bluemagic.Sushi.ItemType("SwapToken"));
				recipe.AddTile(TileID.TinkerersWorkbench);
				recipe.SetResult(this);
				recipe.AddRecipe();

				recipe = new ModRecipe(mod);
				recipe.AddIngredient(null, "ElementalStaff");
				recipe.AddIngredient(Bluemagic.Sushi.ItemType("SwapToken"));
				recipe.AddTile(TileID.TinkerersWorkbench);
				recipe.SetResult(this);
				recipe.AddRecipe();

				recipe = new ModRecipe(mod);
				recipe.AddIngredient(null, "EyeballGlove");
				recipe.AddIngredient(Bluemagic.Sushi.ItemType("SwapToken"));
				recipe.AddTile(TileID.TinkerersWorkbench);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
		}
	}
}
