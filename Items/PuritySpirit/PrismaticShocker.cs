using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit
{
	public class PrismaticShocker : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Prismatic Shocker";
			item.damage = 409;
			item.magic = true;
			item.width = 48;
			item.height = 48;
			item.toolTip = "Dissolve your foes in a dazzling display.";
			item.toolTip2 = "Right-click to turn off the lights.";
			item.useTime = 30;
			item.useAnimation = 30;
			item.UseSound = SoundID.Item44;
			item.noMelee = true;
			item.useStyle = 1;
			item.knockBack = 3.5f;
			item.value = Item.sellPrice(0, 50, 0, 0);
			item.rare = 11;
			item.expert = true;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("PrismaticShocker");
			item.shootSpeed = 0f;
			item.mana = 26;
		}

		public override bool AltFunctionUse(Player player)
		{
			for (int k = 0; k < 1000; k++)
			{
				Projectile proj = Main.projectile[k];
				if (proj.active && proj.owner == player.whoAmI && proj.type == mod.ProjectileType("PrismaticShocker"))
				{
					proj.Kill();
				}
			}
			return false;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int count = 0;
			for (int k = 0; k < 1000; k++)
			{
				Projectile proj = Main.projectile[k];
				if (proj.active && proj.owner == player.whoAmI && proj.type == mod.ProjectileType("PrismaticShocker"))
				{
					if (count < 4)
					{
						count++;
					}
					else
					{
						proj.Kill();
					}
				}
			}
			position = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			return true;
		}
	}
}