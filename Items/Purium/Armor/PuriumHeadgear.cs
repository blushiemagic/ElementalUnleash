using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Armor
{
	public class PuriumHeadgear : ModItem
	{
		public override bool Autoload(ref string name, ref string texture, IList<EquipType> equips)
		{
			equips.Add(EquipType.Head);
			return base.Autoload(ref name, ref texture, equips);
		}

		public override void SetDefaults()
		{
			item.name = "Purium Headgear";
			item.toolTip = "15% increased magic damage, 10% increased magic critical strike chance";
			item.toolTip2 = "+80 maximum mana and 15% reduced mana usage";
			item.width = 18;
			item.height = 18;
			item.defense = 14;
			item.rare = 11;
			item.value = Item.sellPrice(0, 6, 0, 0);
		}

		public override void UpdateEquip(Player player)
		{
			player.magicDamage += 0.15f;
			player.magicCrit += 10;
			player.statManaMax2 += 80;
			player.manaCost -= 0.15f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PuriumBar", 12);
			recipe.AddTile(null, "PuriumAnvil");
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}