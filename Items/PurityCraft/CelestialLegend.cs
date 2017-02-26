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
			player.lifeRegen += 5;
			player.statDefense += 10;
			player.meleeSpeed += 0.2f;
			player.meleeDamage += 0.14f;
			player.meleeCrit += 5;
			player.rangedDamage += 0.14f;
			player.rangedCrit += 5;
			player.magicDamage += 0.14f;
			player.magicCrit += 5;
			player.pickSpeed -= 0.25f;
			player.minionDamage += 0.14f;
			player.minionKB += 0.75f;
			player.thrownDamage += 0.14f;
			player.thrownCrit += 5;
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