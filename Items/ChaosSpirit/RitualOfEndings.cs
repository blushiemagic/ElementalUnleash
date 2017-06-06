using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.ChaosSpirit
{
	public class RitualOfEndings : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Brings the wrath of the End unto the world"
				+ "\nCan be reused infinitely");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.value = Item.sellPrice(0, 50, 0, 0);
			item.rare = 11;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = 4;
			item.UseSound = SoundID.Item44;
		}

		public override bool CanUseItem(Player player)
		{
			return !NPC.AnyNPCs(mod.NPCType("PuritySpirit")) && !NPC.AnyNPCs(mod.NPCType("ChaosSpirit")) && !NPC.AnyNPCs(mod.NPCType("ChaosSpirit2")) && !NPC.AnyNPCs(mod.NPCType("ChaosSpirit3"));
		}

		public override bool UseItem(Player player)
		{
			if (Main.netMode != 1)
			{
				NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - 240, mod.NPCType("ChaosSpirit"));
			}
			return true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "InfinityCrystal");
			recipe.AddIngredient(null, "FoulOrb", 5);
			recipe.AddIngredient(ItemID.Wood, 500);
			recipe.anyWood = true;
			recipe.AddIngredient(ItemID.FallenStar, 99);
			recipe.AddIngredient(ItemID.FossilOre, 40);
			recipe.AddIngredient(null, "ChaoticSoul", 20);
			recipe.AddIngredient(ItemID.LivingFireBlock, 100);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 10);
			recipe.AddIngredient(ItemID.ShroomiteBar, 10);
			recipe.AddIngredient(ItemID.LunarBar, 20);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}