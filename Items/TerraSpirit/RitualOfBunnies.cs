using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.TerraSpirit
{
	public class RitualOfBunnies : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Enrages the Spirit of Purity"
				+ "\nCan be reused infinitely"
				+ "\nEach player starts with 10 lives"
				+ "\nSpirit of Chaos must be defeated first"
				+ "\nWARNING: Use this in the middle of a large open area (eg. the sky)"
				+ "\n[c/FF0000:Work in Progress]: This fight has not yet been finished"
				+ "\nThere is currently no reward for beating the part of the fight that exists"
				+ "\nA checkpoint system will be added later");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.value = Item.sellPrice(0, 50, 0, 0);
			item.rare = 12;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = 4;
			item.UseSound = SoundID.Item44;
		}

		public override bool CanUseItem(Player player)
		{
			return BluemagicWorld.downedChaosSpirit && !NPC.AnyNPCs(mod.NPCType("TerraSpirit"));
		}

		public override bool UseItem(Player player)
		{
			if (Main.netMode != 1)
			{
				NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, mod.NPCType("TerraSpirit"));
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "InfinityCrystal");
			recipe.AddIngredient(ItemID.Bunny);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}