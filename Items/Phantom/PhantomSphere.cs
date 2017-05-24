using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom
{
	public class PhantomSphere : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Phantom Sphere";
			item.width = 26;
			item.height = 28;
			item.toolTip = "Summons a phantom sphere";
			item.useStyle = 4;
			item.useAnimation = 28;
			item.useTime = 28;
			item.damage = 53;
			item.knockBack = 2f;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.autoReuse = false;
			item.useTurn = false;
			item.rare = 8;
			item.shootSpeed = 0f;
			item.mana = 18;
			item.magic = true;
			item.noMelee = true;
			item.UseSound = SoundID.Item43;
			item.shoot = mod.ProjectileType("PhantomSphere");
		}
	}
}