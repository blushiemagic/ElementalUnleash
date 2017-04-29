using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
	public class PurityShield : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Purity Shield";
			item.width = 28;
			item.height = 28;
			item.alpha = 75;
			item.toolTip = "Mount - Encases you inside a shield of purity";
			item.toolTip2 = "Infinite flight and +10% purity shield fill rate";
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
			recipe.AddIngredient(null, "InfinityCrystal", 2);
			recipe.AddTile(null, "ElementalPurge");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}