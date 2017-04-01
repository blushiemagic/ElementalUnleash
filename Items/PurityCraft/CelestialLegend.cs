using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
	public class CelestialLegend : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Celestial Legend";
			item.toolTip = "Turns the holder into a werewolf at night and a merfolk when entering water";
			item.toolTip2 = "Increases to all stats";
			item.width = 16;
			item.height = 24;
			item.accessory = true;
			item.rare = 11;
			item.value = Item.sellPrice(0, 30, 0, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.accMerman = true;
			player.wolfAcc = true;
			if (hideVisual)
			{
				player.hideMerman = true;
				player.hideWolf = true;
			}
			player.lifeRegen += 6;
			player.statDefense += 12;
			player.meleeSpeed += 0.2f;
			player.meleeDamage += 0.15f;
			player.meleeCrit += 10;
			player.rangedDamage += 0.15f;
			player.rangedCrit += 10;
			player.magicDamage += 0.15f;
			player.magicCrit += 10;
			player.pickSpeed -= 0.25f;
			player.minionDamage += 0.15f;
			player.minionKB += 0.75f;
			player.thrownDamage += 0.15f;
			player.thrownCrit += 10;
		}

		public override DrawAnimation GetAnimation()
		{
			return new DrawAnimationVertical(30, 2);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CelestialShell);
			recipe.AddIngredient(null, "InfinityCrystal", 4);
			recipe.AddTile(null, "ElementalPurge");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}