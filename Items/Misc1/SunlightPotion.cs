using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
	public class SunlightPotion : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Sunlight Potion";
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 3;
			item.toolTip = "Emits a strong aura of light and increases night vision";
			item.toolTip2 = "Incompatible with Shine";
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = mod.BuffType("Sunlight");
			item.buffTime = 21600;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ShinePotion);
			recipe.AddIngredient(ItemID.NightOwlPotion);
			recipe.AddIngredient(null, "SolarDrop", 5);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}