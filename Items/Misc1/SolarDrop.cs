using System;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
	public class SolarDrop : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Solar Drop";
			item.width = 8;
			item.height = 8;
			item.toolTip = "Burning with sunlight";
			item.maxStack = 99;
			item.rare = 3;
			item.value = 1000;
		}

		public override void PostUpdate()
		{
			Lighting.AddLight((int)((item.position.X + item.width * 0.5f) / 16f), (int)((item.position.Y + item.height * 0.5f) / 16f), 0.5f, 0.5f, 0f);
		}
	}
}