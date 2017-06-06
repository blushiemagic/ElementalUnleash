using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.ChaosSpirit
{
	[AutoloadEquip(EquipType.Head)]
	public class ChaosSpiritMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit of Chaos Mask");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.rare = 1;
			item.vanity = true;
		}

		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
		{
			color = drawPlayer.GetImmuneAlphaPure(Color.White, shadow);
		}
	}
}