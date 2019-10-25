using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.TerraSpirit
{
    public class BulletRingShrink : BulletRing
    {
        protected float speed;
        protected float rotSpeed;

        public BulletRingShrink(Vector2 center, float speed, float rotSpeed) : base(center, 12, 1600f)
        {
            this.speed = speed;
            this.rotSpeed = rotSpeed;
        }

        public override bool Update(TerraSpirit spirit, Rectangle bounds)
        {
            radius -= speed;
            rotation += rotSpeed;
            return radius >= 0f;
        }
    }
}