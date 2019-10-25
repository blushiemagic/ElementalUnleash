using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.TerraSpirit
{
    public class BulletArray : Bullet
    {
        protected const int size = 32;

        protected Vector2 position;
        protected float rotation;
        protected float interval;
        protected Vector2 velocity;
        protected int life;

        public BulletArray(Vector2 position, float rotation, float interval, Vector2 velocity, int life)
        {
            this.position = position;
            this.rotation = rotation;
            this.interval = interval;
            this.velocity = velocity;
            this.life = life;
        }

        public override bool Update(TerraSpirit spirit, Rectangle bounds)
        {
            position += velocity;
            if (position.X < bounds.X)
            {
                position.X += bounds.Width;
            }
            else if (position.X > bounds.Right)
            {
                position.X -= bounds.Width;
            }
            if (position.Y < bounds.Y)
            {
                position.Y += bounds.Height;
            }
            else if (position.Y > bounds.Bottom)
            {
                position.Y -= bounds.Height;
            }
            life--;
            return life >= 0;
        }

        public override bool Collides(Rectangle box)
        {
            Vector2 direction = rotation.ToRotationVector2();
            for (int k = (int)(-3000 / interval); k <= (int)(3000 / (int)interval); k++)
            {
                Vector2 pos = position + k * interval * direction;
                Rectangle myBox = new Rectangle((int)pos.X - size / 2, (int)pos.Y - size / 2, size, size);
                if (myBox.Intersects(box))
                {
                    return true;
                }
            }
            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = Bluemagic.Instance.GetTexture("TerraSpirit/BulletSingle");
            Vector2 direction = rotation.ToRotationVector2();
            for (int k = (int)(-3000 / interval); k <= (int)(3000 / (int)interval); k++)
            {
                Vector2 pos = position + k * interval * direction;
                spriteBatch.Draw(texture, pos - Main.screenPosition - new Vector2(size / 2, size / 2), Color.White);
            }
        }
    }
}