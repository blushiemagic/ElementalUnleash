using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
	public class MoonlightCharm : ModItem
	{
		public override bool Autoload(ref string name, ref string texture, IList<EquipType> equips)
		{
			equips.Add(EquipType.HandsOn);
			return base.Autoload(ref name, ref texture, equips);
		}

		public override void SetDefaults()
		{
			item.name = "Moonlight Charm";
			item.toolTip = "Provides huge life regeneration and reduces the cooldown of healing potions";
			item.toolTip2 = "Increases pickup range and effectiveness of hearts";
			item.width = 16;
			item.height = 24;
			item.accessory = true;
			item.lifeRegen = 10;
			item.rare = 11;
			item.value = Item.sellPrice(0, 25, 0, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>(mod);
			modPlayer.lifeMagnet2 = true;
			player.pStone = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CharmofMyths);
			recipe.AddIngredient(null, "InfinityCrystal");
			recipe.AddTile(null, "ElementalPurge");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}