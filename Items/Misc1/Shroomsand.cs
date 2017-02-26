using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
	public class Shroomsand : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Shroomsand Block";
			item.width = 12;
			item.height = 12;
			item.maxStack = 999;
			item.useStyle = 1;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = mod.TileType("Shroomsand");
			item.ammo = AmmoID.Sand;
			item.notAmmo = true;
		}

		public override void PickAmmo(Player player, ref int type, ref float speed, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.SandBallGun)
			{
				type = mod.ProjectileType("ShroomsandGunBall");
			}
		}
	}
}