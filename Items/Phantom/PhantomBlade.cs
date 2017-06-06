using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom
{
	public class PhantomBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Projects a phantom blade when swung");
		}

		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 40;
			item.useStyle = 1;
			item.useAnimation = 32;
			item.useTime = 32;
			item.damage = 102;
			item.knockBack = 6f;
			item.autoReuse = true;
			item.useTurn = false;
			item.rare = 8;
			item.melee = true;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.UseSound = SoundID.Item1;
			item.shoot = mod.ProjectileType("PhantomBlade");
			item.shootSpeed = 0f;
		}
	}
}