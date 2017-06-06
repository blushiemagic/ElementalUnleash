using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
	public class FlaskOfEtherealFlames : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Melee attacks inflict ethereal flames");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 8;
			item.value = 5000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = mod.BuffType("EtherealFlamesEnchant");
			item.buffTime = 72000;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater);
			recipe.AddIngredient(ItemID.Ectoplasm, 2);
			recipe.AddTile(TileID.ImbuingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}