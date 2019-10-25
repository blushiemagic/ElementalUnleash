using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.TerraSpirit
{
    public abstract class BulletSingle : Bullet
    {
        protected Vector2 position;
        protected int size;

        public virtual Texture2D Texture
        {
            get
            {
                return Bluemagic.Instance.GetTexture("TerraSpirit/BulletSingle");
            }
        }

        public BulletSingle(Vector2 position, int size = 32)
        {
            this.position = position;
            this.size = size;
        }

        public override bool Collides(Rectangle box)
        {
            Rectangle myBox = new Rectangle((int)position.X - size / 2, (int)position.Y - size / 2, size, size);
            return myBox.Intersects(box);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, position - Main.screenPosition - new Vector2(size / 2, size / 2), Color.White);
        }
    }
}