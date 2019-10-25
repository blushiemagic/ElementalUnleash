using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.TerraSpirit
{
    public class TerraProbe3 : TerraProbe
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.lifeMax *= 3;
        }

        public override void Behavior()
        {
            Timer++;
            if (Timer % 60 == 0)
            {
                TerraSpirit spirit = (TerraSpirit)Spirit.modNPC;
                Vector2 center = npc.Center;
                spirit.bullets.Add(new BulletPortal2(center, center + new Vector2(-320f, -320f)));
                spirit.bullets.Add(new BulletPortal2(center, center + new Vector2(320f, -320f)));
                spirit.bullets.Add(new BulletPortal2(center, center + new Vector2(-320f, 320f)));
                spirit.bullets.Add(new BulletPortal2(center, center + new Vector2(320f, 320f)));
                Timer = 0;
            }
        }

        public override bool PreNPCLoot()
        {
            BluemagicWorld.terraCheckpoint2 = FindHighestLife(BluemagicWorld.terraCheckpoint2);
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Checkpoint2"));
            if (Main.netMode == 2)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
            return base.PreNPCLoot();
        }
    }
}
