using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.TerraSpirit
{
    public class TerraProbe2 : TerraProbe
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.lifeMax *= 2;
        }

        public override void Behavior()
        {
            Timer++;
            if (Timer % 30 == 0)
            {
                TerraSpirit spirit = (TerraSpirit)Spirit.ModNPC;
                var bullet = new BulletRingExpand(NPC.Center, 8f);
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

        public override bool PreKill()
        {
            BluemagicWorld.terraCheckpoint1 = FindHighestLife(BluemagicWorld.terraCheckpoint1);
            Item.NewItem(NPC.GetSource_Death(), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("Checkpoint1").Type);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
            return base.PreKill();
        }
    }
}
