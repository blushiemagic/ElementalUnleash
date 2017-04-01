using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit
{
	public class VoidEmblem : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Emblem of the Void";
			item.damage = 612;
			item.summon = true;
			item.mana = 10;
			item.width = 28;
			item.height = 28;
			item.toolTip = "Summons a void emissary to fight alongside you.";
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 4;
			item.noMelee = true;
			item.knockBack = 6f;
			item.value = Item.sellPrice(0, 50, 0, 0);
			item.rare = 11;
			item.UseSound = SoundID.Item44;
			item.shoot = mod.ProjectileType("VoidEmissary");
			item.shootSpeed = 10f;
			item.buffType = mod.BuffType("VoidEmissary");
			item.buffTime = 3600;
		}
	}
}