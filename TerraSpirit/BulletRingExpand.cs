using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.TerraSpirit
{
    public class BulletRingExpand : BulletRing
    {
        protected float speed;

        public BulletRingExpand(Vector2 center, float speed, float radius = 0f) : base(center, 16, radius)
        {
            this.speed = speed;
        }

        public override bool Update(TerraSpirit spirit, Rectangle bounds)
        {
            radius += speed;
            return radius >= 0f && radius < Math.Sqrt(bounds.Width * bounds.Width + bounds.Height * bounds.Height);
        }
    }
}