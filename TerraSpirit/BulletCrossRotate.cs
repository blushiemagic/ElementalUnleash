using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.TerraSpirit
{
    public class BulletCrossRotate : BulletCross
    {
        protected Vector2 origin;
        protected float offset;
        protected float rotSpeed;

        public BulletCrossRotate(Vector2 origin, float offset, float rotSpeed, float rotation, int delay = 90) : base(origin + offset * rotation.ToRotationVector2(), rotation, delay)
        {
            this.origin = origin;
            this.rotSpeed = rotSpeed;
            this.offset = offset;
        }

        public override bool Update(TerraSpirit spirit, Rectangle bounds)
        {
            rotation += rotSpeed;
            position = origin + offset * rotation.ToRotationVector2();
            return base.Update(spirit, bounds);
        }
    }
}