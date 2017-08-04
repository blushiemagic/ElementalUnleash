using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.TerraSpirit
{
	public class TerraProbe5 : TerraProbe
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.lifeMax *= 5;
		}

		public override void Behavior()
		{
			TerraSpirit spirit = (TerraSpirit)Spirit.modNPC;
			Vector2 target = spirit.GetTarget().Center;
			Timer++;
			if (Timer % 8 == 4)
			{
				Vector2 offset = target - npc.Center;
				Vector2 normal = new Vector2(-offset.Y, offset.X);
				spirit.bullets.Insert(0, new BulletVoidWorld(target + offset));
				spirit.bullets.Insert(0, new BulletVoidWorld(target - offset));
				spirit.bullets.Insert(0, new BulletVoidWorld(target + normal));
				spirit.bullets.Insert(0, new BulletVoidWorld(target - normal));
			}
			else if (Timer % 8 == 0)
			{
				spirit.bullets.Insert(0, new BulletVoidWorld(target));
			}
			if (Timer % 90 == 0)
			{
				spirit.bullets.Add(new BulletExplode(npc.Center, target));
			}
		}

		public override bool PreNPCLoot()
		{
			BluemagicWorld.terraCheckpointS = FindHighestLife(BluemagicWorld.terraCheckpointS);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CheckpointS"));
			if (Main.netMode == 2)
			{
				NetMessage.SendData(MessageID.WorldData);
			}
			return base.PreNPCLoot();
		}
	}
}
