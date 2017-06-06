using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
	public class PurityShield : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Mount - Encases you inside a shield of purity"
				+ "\nInfinite flight and +10% purity shield fill rate");
		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.alpha = 75;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.value = Item.sellPrice(0, 30, 0, 0);
			item.rare = 11;
			item.UseSound = SoundID.Item25;
			item.noMelee = true;
			item.mountType = mod.MountType("PurityShield");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CosmicCarKey);
			recipe.AddIngredient(null, "InfinityCrystal", 3);
			recipe.AddTile(null, "PuriumAnvil");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}