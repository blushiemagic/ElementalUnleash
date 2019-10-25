using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.TerraSpirit
{
    public class BulletBeamBigRotate : BulletBeamBig
    {
        protected float rotSpeed;

        public BulletBeamBigRotate(Vector2 position, float rotSpeed, int life, float size = 160f, float rotation = MathHelper.PiOver2, int delay = 90) : base(position, size, rotation, delay, life)
        {
            this.rotSpeed = rotSpeed;
        }

        public override bool Update(TerraSpirit spirit, Rectangle bounds)
        {
            rotation += rotSpeed;
            return base.Update(spirit, bounds);
        }
    }
}