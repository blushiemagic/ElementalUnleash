using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Blushie
{
	public class BlushieCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Use this item to die"
				+ "\nCan be reused infinitely"
				+ "\nWARNING: Use this in the middle of a large open area (eg. the sky)"
				+ "\nIt is highly recommended that you use the Purity Shield [i:" + mod.ItemType("PurityShield") + "] mount"
				+ "\nRight-click to focus the camera on the entire boss arena");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.maxStack = 1;
			item.rare = 12;
			item.value = Item.sellPrice(2, 0, 0, 0);
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = 4;
			item.UseSound = SoundID.Item44;
			item.noUseGraphic = true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			return !BlushieBoss.BlushieBoss.Active;
		}

		public override bool UseItem(Player player)
		{
			if (Main.netMode != 1)
			{
				NPC.NewNPC((int)player.Center.X, (int)player.Center.Y + 24, mod.NPCType("Blushiemagic"));
				BlushieBoss.BlushieBoss.CameraFocus = player.altFunctionUse == 2;
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "InfinityCrystal");
			recipe.AddIngredient(null, "ChaosCrystal");
			recipe.AddIngredient(null, "PureSalt", 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Main.DiscoColor;
		}
	}
}