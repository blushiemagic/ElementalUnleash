using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom
{
	public class PaladinStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Summons a mini paladin to fight for you.");
		}

		public override void SetDefaults()
		{
			item.damage = 62;
			item.summon = true;
			item.mana = 10;
			item.width = 26;
			item.height = 28;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 1;
			item.noMelee = true;
			item.knockBack = 4f;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = 8;
			item.UseSound = SoundID.Item44;
			item.shoot = mod.ProjectileType("MiniPaladin");
			item.shootSpeed = 5f;
			item.buffType = mod.BuffType("MiniPaladin");
			item.buffTime = 3600;
		}
		
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return player.altFunctionUse != 2;
		}
		
		public override bool UseItem(Player player)
		{
			if(player.altFunctionUse == 2)
			{
				player.MinionNPCTargetAim();
			}
			return base.UseItem(player);
		}
	}
}
