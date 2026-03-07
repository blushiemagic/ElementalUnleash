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
            NPC.lifeMax *= 5;
        }

        public override void Behavior()
        {
            TerraSpirit spirit = (TerraSpirit)Spirit.ModNPC;
            Vector2 target = spirit.GetTarget().Center;
            Timer++;
            if (Timer % 8 == 4)
            {
                Vector2 offset = target - NPC.Center;
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
                spirit.bullets.Add(new BulletExplode(NPC.Center, target));
            }
        }

        public override bool PreKill()
        {
            BluemagicWorld.terraCheckpointS = FindHighestLife(BluemagicWorld.terraCheckpointS);
            Item.NewItem(NPC.GetSource_Death(), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("CheckpointS").Type);
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
            return base.PreKill();
        }
    }
}
