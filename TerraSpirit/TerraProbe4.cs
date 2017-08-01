using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.TerraSpirit
{
	public class TerraProbe4 : TerraProbe
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.lifeMax *= 4;
		}

		public override void Behavior()
		{
			Timer++;
			if (Timer >= 90)
			{
				Timer = 0;
				TerraSpirit spirit = (TerraSpirit)Spirit.modNPC;
				Vector2 center = npc.Center;
				spirit.bullets.Add(new BulletExplode(center, center + new Vector2(-320f, -320f)));
				spirit.bullets.Add(new BulletExplode(center, center + new Vector2(320f, -320f)));
				spirit.bullets.Add(new BulletExplode(center, center + new Vector2(-320f, 320f)));
				spirit.bullets.Add(new BulletExplode(center, center + new Vector2(320f, 320f)));
			}
		}

		public override bool PreNPCLoot()
		{
			BluemagicWorld.terraCheckpoint3 = FindHighestLife(BluemagicWorld.terraCheckpoint3);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Checkpoint3"));
			if (Main.netMode == 2)
			{
				NetMessage.SendData(MessageID.WorldData);
			}
			return base.PreNPCLoot();
		}
	}
}
