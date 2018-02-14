using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Blushie
{
	public class FirePulsar : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Sends pulses of flames across the screen"
				+ "\n'Great for impersonating... someone?'");
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.autoReuse = true;
			item.rare = 13;
			item.UseSound = SoundID.Item81;
			item.noMelee = true;
			item.useStyle = 4;
			item.damage = 1200;
			item.useAnimation = 30;
			item.useTime = 30;
			item.width = 30;
			item.height = 30;
			item.shoot = mod.ProjectileType("FirePulse");
			item.shootSpeed = 0f;
			item.knockBack = 5f;
			item.magic = true;
			item.mana = 4;
			item.value = Item.sellPrice(2, 0, 0, 0);
		}
	}
}
