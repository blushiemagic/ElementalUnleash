using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom
{
	public class SpectreGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Uses wisps as ammo");
		}

		public override void SetDefaults()
		{
			item.damage = 68;
			item.ranged = true;
			item.width = 42;
			item.height = 30;
			item.useTime = 35;
			item.useAnimation = 35;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 5f;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = 8;
			item.UseSound = SoundID.Item43;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("Wisp");
			item.shootSpeed = 8f;
			item.useAmmo = mod.ItemType("Wisp");
		}

		public override void GetWeaponDamage(Player player, ref int damage)
		{
			damage = (int)(damage * player.bulletDamage + 5E-06);
		}

		public override Vector2? HoldoutOffset()
		{
			return Vector2.Zero;
		}

		public override void AddRecipes()
		{
			if (Bluemagic.Sushi != null)
			{
				ModRecipe recipe;

				recipe = new ModRecipe(mod);
				recipe.AddIngredient(null, "PhantomBlade");
				recipe.AddIngredient(Bluemagic.Sushi.ItemType("SwapToken"));
				recipe.AddTile(TileID.TinkerersWorkbench);
				recipe.SetResult(this);
				recipe.AddRecipe();

				recipe = new ModRecipe(mod);
				recipe.AddIngredient(null, "PhantomSphere");
				recipe.AddIngredient(Bluemagic.Sushi.ItemType("SwapToken"));
				recipe.AddTile(TileID.TinkerersWorkbench);
				recipe.SetResult(this);
				recipe.AddRecipe();

				recipe = new ModRecipe(mod);
				recipe.AddIngredient(null, "PaladinStaff");
				recipe.AddIngredient(Bluemagic.Sushi.ItemType("SwapToken"));
				recipe.AddTile(TileID.TinkerersWorkbench);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
		}
	}
}
