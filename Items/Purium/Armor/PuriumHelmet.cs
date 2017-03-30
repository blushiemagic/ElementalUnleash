using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Armor
{
	public class PuriumHelmet : ModItem
	{
		public override bool Autoload(ref string name, ref string texture, IList<EquipType> equips)
		{
			equips.Add(EquipType.Head);
			return base.Autoload(ref name, ref texture, equips);
		}

		public override void SetDefaults()
		{
			item.name = "Purium Helmet";
			item.toolTip = "15% increased melee damage, 10% increased melee critical strike chance";
			item.toolTip2 = "16% increased melee speed, 4% increased movement speed";
			item.width = 18;
			item.height = 18;
			item.defense = 34;
			item.rare = 11;
			item.value = Item.sellPrice(0, 6, 0, 0);
		}

		public override void UpdateEquip(Player player)
		{
			player.meleeDamage += 0.15f;
			player.meleeCrit += 10;
			player.meleeSpeed += 0.16f;
			player.moveSpeed += 0.04f;
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