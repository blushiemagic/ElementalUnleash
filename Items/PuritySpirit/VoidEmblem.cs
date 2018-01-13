using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit
{
	public class VoidEmblem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Emblem of the Void");
			Tooltip.SetDefault("Summons a void emissary to fight alongside you."
				+ "\nDoes not interfere with your piercing projectiles");
		}

		public override void SetDefaults()
		{
			item.damage = 450;
			item.summon = true;
			item.mana = 10;
			item.width = 28;
			item.height = 28;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 4;
			item.noMelee = true;
			item.knockBack = 6f;
			item.value = Item.sellPrice(0, 50, 0, 0);
			item.rare = 11;
			item.UseSound = SoundID.Item44;
			item.shoot = mod.ProjectileType("VoidEmissary");
			item.shootSpeed = 10f;
			item.buffType = mod.BuffType("VoidEmissary");
			item.buffTime = 3600;
		}

		public override void AddRecipes()
		{
			if (Bluemagic.Sushi != null)
			{
				ModRecipe recipe;

				recipe = new ModRecipe(mod);
				recipe.AddIngredient(null, "DanceOfBlades");
				recipe.AddIngredient(Bluemagic.Sushi.ItemType("SwapToken"));
				recipe.AddTile(TileID.TinkerersWorkbench);
				recipe.SetResult(this);
				recipe.AddRecipe();

				recipe = new ModRecipe(mod);
				recipe.AddIngredient(null, "CleanserBeam");
				recipe.AddIngredient(Bluemagic.Sushi.ItemType("SwapToken"));
				recipe.AddTile(TileID.TinkerersWorkbench);
				recipe.SetResult(this);
				recipe.AddRecipe();

				recipe = new ModRecipe(mod);
				recipe.AddIngredient(null, "PrismaticShocker");
				recipe.AddIngredient(Bluemagic.Sushi.ItemType("SwapToken"));
				recipe.AddTile(TileID.TinkerersWorkbench);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}
		}
	}
}