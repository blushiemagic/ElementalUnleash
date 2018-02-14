using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Blushie
{
	public class RadiantRainbowRondure : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Fires a ray of fabulous rainbow!"
				+ "\n'Great for impersonating tModLoader devs...?'");
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.useStyle = 4;
			item.useAnimation = 30;
			item.useTime = 30;
			item.channel = true;
			item.noMelee = true;
			item.damage = 1200;
			item.knockBack = 4f;
			item.autoReuse = false;
			item.useTurn = false;
			item.rare = 13;
			item.magic = true;
			item.value = Item.sellPrice(2, 0, 0, 0);
			item.UseSound = SoundID.Item1;
			item.shoot = mod.ProjectileType("RadiantRainbowRay");
			item.mana = 10;
			item.shootSpeed = 0f;
		}
	}
}