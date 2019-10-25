using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.TerraSpirit
{
    public class BulletRingSpinOut : BulletRing
    {
        protected float speed;
        protected float rotSpeed;

        public BulletRingSpinOut(Vector2 center, float speed, float rotSpeed) : base(center, 16)
        {
            this.speed = speed;
            this.rotSpeed = rotSpeed;
        }

        public override bool Update(TerraSpirit spirit, Rectangle bounds)
        {
            radius += speed;
            rotation += rotSpeed;
            return radius >= 0f && radius < Math.Sqrt(bounds.Width * bounds.Width + bounds.Height * bounds.Height);
        }
    }
}