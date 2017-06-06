using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom
{
	public class Wisp : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Chases enemies through walls");
		}

		public override void SetDefaults()
		{
			item.damage = 1;
			item.ranged = true;
			item.width = 14;
			item.height = 14;
			item.maxStack = 999;
			item.consumable = true;
			item.knockBack = 1f;
			item.value = Item.sellPrice(0, 0, 1, 0);
			item.rare = 8;
			item.shoot = mod.ProjectileType("Wisp");
			item.ammo = item.type;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Ectoplasm);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}
	}
}
