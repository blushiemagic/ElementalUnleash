using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
	public class BubbleshieldPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases defense by 12 and reduces damage taken by 12%"
				+ "\nIncompatible with Ironskin and Endurance");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 8;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = mod.BuffType("Bubbleshield");
			item.buffTime = 21600;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronskinPotion);
			recipe.AddIngredient(ItemID.EndurancePotion);
			recipe.AddIngredient(null, "Bubble");
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}