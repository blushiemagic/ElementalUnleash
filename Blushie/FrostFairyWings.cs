using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Blushie
{
	public class FrostFairyWings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wings of the Frost Fairy");
			Tooltip.SetDefault("Summons Wings of the Frost Fairy to follow behind you"
				+ "\n'Great for impersonating tModLoader devs!'");
		}

		public override void SetDefaults()
		{
			item.damage = 900;
			item.summon = true;
			item.mana = 20;
			item.width = 28;
			item.height = 28;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 100;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.knockBack = 4f;
			item.value = Item.sellPrice(1, 0, 0, 0);
			item.rare = 13;
			item.UseSound = SoundID.Item44;
			item.shoot = mod.ProjectileType("FrostFairyWingsProj");
			item.shootSpeed = 0f;
			item.buffType = mod.BuffType("FrostFairyWings");
			item.buffTime = 3600;
		}
	}
}
