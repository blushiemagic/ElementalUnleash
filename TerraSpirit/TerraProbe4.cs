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
            NPC.lifeMax *= 4;
        }

        public override void Behavior()
        {
            Timer++;
            if (Timer >= 90)
            {
                Timer = 0;
                TerraSpirit spirit = (TerraSpirit)Spirit.ModNPC;
                Vector2 center = NPC.Center;
                spirit.bullets.Add(new BulletExplode(center, center + new Vector2(-320f, -320f)));
                spirit.bullets.Add(new BulletExplode(center, center + new Vector2(320f, -320f)));
                spirit.bullets.Add(new BulletExplode(center, center + new Vector2(-320f, 320f)));
                spirit.bullets.Add(new BulletExplode(center, center + new Vector2(320f, 320f)));
            }
        }

        public override bool PreKill()
        {
            BluemagicWorld.terraCheckpoint3 = FindHighestLife(BluemagicWorld.terraCheckpoint3);
            Item.NewItem(NPC.GetSource_Death(), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("Checkpoint3").Type);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
            return base.PreKill();
        }
    }
}
