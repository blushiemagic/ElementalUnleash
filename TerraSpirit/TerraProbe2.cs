using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.TerraSpirit
{
	public class TerraProbe2 : TerraProbe
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.lifeMax *= 2;
		}

		public override void Behavior()
		{
			Timer++;
			if (Timer % 30 == 0)
			{
				TerraSpirit spirit = (TerraSpirit)Spirit.modNPC;
				var bullet = new BulletRingExpand(npc.Center, 8f);
				if (Timer == 30)
				{
					bullet.Rotation(MathHelper.Pi / 16f);
				}
				spirit.bullets.Add(bullet);
			}
			if (Timer >= 60)
			{
				Timer = 0;
			}
		}
	}
}
