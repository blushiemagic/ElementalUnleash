using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.BlushieBoss
{
    public class BulletFire : BulletSimple
    {
        public BulletFire(Vector2 position, Vector2 velocity)
            : base(position, velocity, 16f, BlushieBoss.BulletFireTexture)
        {
        }

        public override void Update()
        {
            base.Update();
            for (int k = 0; k < 0; k++)
            {
                int dust = Dust.NewDust(Position - new Vector2(Size), 32, 32, 6, 0f, 0f, 0, default(Color), 4f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 2f;
                Main.dust[dust].velocity += Velocity;
            }
        }
    }
}