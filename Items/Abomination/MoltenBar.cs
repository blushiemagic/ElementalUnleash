using System;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination
{
	public class MoltenBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Pulsing with heat energy'"
				+ "\nThe first item bluemagic123 ever made");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 99;
			item.rare = 10;
			item.value = 30000;
			item.useStyle = 1;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = mod.TileType("MoltenBar");
			item.holdStyle = 4;
		}

		public override void PostUpdate()
		{
			Lighting.AddLight((int)((item.position.X + (float)item.width*0.5f) / 16f), (int)((item.position.Y + (float)item.height*0.5f) / 16f), 0.7f, 0.4f, 0f);
		}

		public override void HoldStyle(Player player)
		{
			player.itemLocation.X = player.Center.X + 6f * player.direction;
			player.itemLocation.Y = player.Center.Y + 10f;
			if(player.gravDir == -1)
			{
				player.itemLocation.Y = player.position.Y + player.height + (player.position.Y - player.itemLocation.Y);
			}
			player.itemRotation = 0;
		}

		public override bool HoldItemFrame(Player player)
		{
			player.bodyFrame.Y = player.bodyFrame.Height * 3;
			return true;
		}
	}
}