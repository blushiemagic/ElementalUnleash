using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.TerraSpirit
{
    public class BulletRingTimed : BulletRing
    {
        protected float speed;
        protected int timer;

        public BulletRingTimed(Vector2 center, int numBullets, float radius, float speed, int life) : base(center, numBullets, radius)
        {
            this.speed = speed;
            this.timer = life;
        }

        public override bool Update(TerraSpirit spirit, Rectangle bounds)
        {
            rotation += speed;
            timer--;
            return timer > 0;
        }
    }
}