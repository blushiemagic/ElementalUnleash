using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Blushie
{
	public class DarkLightningPack : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Zaps enemies across the entire screen"
				+ "\n'Great for impersonating tModLoader devs!'");
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.autoReuse = true;
			item.rare = 13;
			item.UseSound = SoundID.Item121;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.useStyle = 100;
			item.damage = 500;
			item.useAnimation = 20;
			item.useTime = 20;
			item.width = 24;
			item.height = 28;
			item.shoot = mod.ProjectileType("DarkLightningProj");
			item.shootSpeed = 0f;
			item.knockBack = 4f;
			item.ranged = true;
			item.value = Item.sellPrice(2, 0, 0, 0);
		}
	}
}
