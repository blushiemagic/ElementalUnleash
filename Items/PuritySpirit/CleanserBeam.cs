using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit
{
	public class CleanserBeam : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Cleanser Beam";
			item.damage = 212;
			item.ranged = true;
			item.width = 64;
			item.height = 24;
			item.toolTip = "Cleanse your foes, one line at a time.";
			item.toolTip2 = "Wipes out everything within the beam.";
			item.useTime = 20;
			item.useAnimation = 20;
			item.UseSound = SoundID.Item13;
			item.channel = true;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.useStyle = 5;
			item.knockBack = 1f;
			item.value = Item.sellPrice(0, 50, 0, 0);
			item.rare = 11;
			item.expert = true;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("CleanserBeam");
			item.shootSpeed = 14f;
		}
	}
}