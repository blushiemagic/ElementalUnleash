using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit
{
	public class DanceOfBlades : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Dance of Blades";
			item.damage = 200;
			item.melee = true;
			item.width = 80;
			item.height = 80;
			item.toolTip = "Dance the dance of death.";
			item.toolTip2 = "Purifies the screen of enemies.";
			item.useTime = 10;
			item.useAnimation = 10;
			item.channel = true;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.useStyle = 100;
			item.knockBack = 8f;
			item.value = Item.sellPrice(0, 50, 0, 0);
			item.rare = 11;
			item.expert = true;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("DanceOfBlades");
			item.shootSpeed = 0f;
		}

		public override bool UseItemFrame(Player player)
		{
			player.bodyFrame.Y = 3 * player.bodyFrame.Height;
			return true;
		}
	}
}