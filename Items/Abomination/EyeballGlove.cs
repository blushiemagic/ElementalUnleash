using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination
{
	public class EyeballGlove : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Does not require ammo"
				+ "\nYo I heard you like debuffs, so I...");
		}

		public override void SetDefaults()
		{
			item.autoReuse = true;
			item.rare = 10;
			item.UseSound = SoundID.Item1;
			item.noMelee = true;
			item.useStyle = 1;
			item.damage = 289;
			item.useAnimation = 20;
			item.useTime = 20;
			item.width = 26;
			item.height = 26;
			item.shoot = mod.ProjectileType("EyeballGlove");
			item.shootSpeed = 8f;
			item.knockBack = 6.5f;
			item.thrown = true;
			item.value = Item.sellPrice(0, 15, 0, 0);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, Main.myPlayer, Main.rand.Next(6));
			return false;
		}
	}
}
