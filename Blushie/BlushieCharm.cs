using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Blushie
{
	public class BlushieCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charm of Blushie");
			Tooltip.SetDefault("Your hearts shall spell the doom of your enemies"
				+ "\nCan be used by any class"
				+ "\nMade in celebration of blushiemagic's love");
		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 54;
			item.useStyle = 4;
			item.useAnimation = 30;
			item.useTime = 30;
			item.channel = true;
			item.noMelee = true;
			item.damage = 1;
			item.knockBack = 1f;
			item.autoReuse = false;
			item.useTurn = false;
			item.rare = 12;
			item.expert = true;
			item.magic = true;
			item.value = 100000000;
			item.UseSound = SoundID.Item1;
			item.shoot = mod.ProjectileType("BlushieCharmProj");
			item.mana = 200;
			item.shootSpeed = 0f;
		}
	}
}