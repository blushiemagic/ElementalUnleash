using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.TerraSpirit
{
    public class BulletSingleMove : BulletSingle
    {
        protected Vector2 velocity;

        public BulletSingleMove(Vector2 position, Vector2 velocity) : base(position)
        {
            this.velocity = velocity;
        }

        public override bool Update(TerraSpirit spirit, Rectangle bounds)
        {
            position += velocity;
            return position.X >= bounds.X && position.X <= bounds.Right && position.Y >= bounds.Y && position.Y <= bounds.Bottom;
        }
    }
}