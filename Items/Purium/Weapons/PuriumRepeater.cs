using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons
{
	public class PuriumRepeater : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("50% chance not to consume ammo");
		}

		public override void SetDefaults()
		{
			item.width = 50;
			item.height = 18;
			item.useStyle = 5;
			item.useAnimation = 18;
			item.useTime = 18;
			item.noMelee = true;
			item.damage = 253;
			item.knockBack = 3f;
			item.autoReuse = true;
			item.useTurn = false;
			item.rare = 11;
			item.ranged = true;
			item.value = Item.sellPrice(0, 12, 0, 0);
			item.UseSound = SoundID.Item5;
			item.shoot = ProjectileID.WoodenArrowFriendly;
			item.useAmmo = AmmoID.Arrow;
			item.shootSpeed = 12f;
		}

		public override bool ConsumeAmmo(Player player)
		{
			return Main.rand.Next(2) == 0;
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