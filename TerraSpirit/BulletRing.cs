using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.TerraSpirit
{
    public abstract class BulletRing : Bullet
    {
        protected Vector2 center;
        protected int numBullets;
        protected float radius;
        protected float rotation;
        protected int size;

        public virtual Texture2D Texture
        {
            get
            {
                return Bluemagic.Instance.GetTexture("TerraSpirit/BulletSingle");
            }
        }

        public BulletRing(Vector2 center, int numBullets, float radius = 0f, int size = 32)
        {
            this.center = center;
            this.numBullets = numBullets;
            this.radius = radius;
            this.rotation = 0f;
            this.size = size;
        }

        public BulletRing Rotation(float rotation)
        {
            this.rotation = rotation;
            return this;
        }

        public BulletRing NumBullets(int numBullets)
        {
            this.numBullets = numBullets;
            return this;
        }

        public override bool Collides(Rectangle box)
        {
            for (int k = 0; k < numBullets; k++)
            {
                Vector2 pos = GetPos(k);
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
            for (int k = 0; k < numBullets; k++)
            {
                spriteBatch.Draw(Texture, GetPos(k) - Main.screenPosition - new Vector2(size / 2, size / 2), Color.White);
            }
        }

        protected Vector2 GetPos(int bullet)
        {
            Vector2 pos = center;
            float angle = (float)bullet / (float)numBullets * MathHelper.TwoPi + rotation;
            return pos + radius * angle.ToRotationVector2();
        }
    }
}