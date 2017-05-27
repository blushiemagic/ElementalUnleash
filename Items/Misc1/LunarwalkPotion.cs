using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
	public class LunarwalkPotion : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Lunarwalk Potion";
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 3;
			item.toolTip = "30% increased movement speed and allows the control of gravity";
			item.toolTip2 = "Incompatible with Swiftness";
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = mod.BuffType("Lunarwalk");
			item.buffTime = 21600;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SwiftnessPotion);
			recipe.AddIngredient(ItemID.GravitationPotion);
			recipe.AddIngredient(null, "LunarDrop", 5);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}