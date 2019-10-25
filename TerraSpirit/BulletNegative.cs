using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.TerraSpirit
{
    public class BulletNegative : BulletSingle
    {
        protected Vector2 velocity;

        public BulletNegative(Vector2 position, Vector2 velocity) : base(position)
        {
            this.velocity = velocity;
        }

        public override Texture2D Texture
        {
            get
            {
                return Bluemagic.Instance.GetTexture("TerraSpirit/BulletNegative");
            }
        }

        public override bool Update(TerraSpirit spirit, Rectangle bounds)
        {
            if (position.X - size / 2 <= bounds.Left && velocity.X < 0f)
            {
                velocity.X *= -1f;
            }
            if (position.X + size / 2 >= bounds.Right && velocity.X > 0f)
            {
                velocity.X *= -1f;
            }
            if (position.Y - size / 2 <= bounds.Top && velocity.Y < 0f)
            {
                velocity.Y *= -1f;
            }
            if (position.Y + size / 2 >= bounds.Bottom && velocity.Y > 0f)
            {
                velocity.Y *= -1f;
            }
            position += velocity;
            return true;
        }
    }
}