using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.TerraSpirit
{
    public class BulletBeamBig : Bullet
    {
        protected const int length = 6000;

        protected Vector2 position;
        protected float size;
        protected float rotation;
        protected int delay;
        protected int life;
        private float alpha;
        private float alphaDir;

        public BulletBeamBig(Vector2 position, float size = 160f, float rotation = MathHelper.PiOver2, int delay = 90, int life = 30)
        {
            this.position = position;
            this.size = size;
            this.rotation = rotation;
            this.delay = delay;
            this.life = life;
            this.alpha = 0f;
            this.alphaDir = 1f;
        }

        public override bool Update(TerraSpirit spirit, Rectangle bounds)
        {
            delay--;
            alpha += 0.05f * alphaDir;
            if (alpha >= 0.5f)
            {
                alphaDir = -1f;
            }
            if (alpha <= 0f)
            {
                alphaDir = 1f;
            }
            return delay > -life;
        }

        public override bool Collides(Rectangle box)
        {
            if (delay > 0)
            {
                return false;
            }
            Vector2 offset = length / 2 * rotation.ToRotationVector2();
            Vector2 start = position + offset;
            Vector2 end = position - offset;
            float num = 0f;
            return Collision.CheckAABBvLineCollision(box.TopLeft(), box.Size(), start, end, size, ref num);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = Bluemagic.Instance.GetTexture("TerraSpirit/BulletBeamBig");
            float useAlpha = alpha;
            if (delay <= 0)
            {
                useAlpha = 1f;
            }
            Vector2 scale = new Vector2(length / 2f, size / 2f);
            spriteBatch.Draw(texture, position - Main.screenPosition, null, Color.White * useAlpha, rotation, new Vector2(1f, 1f), scale, SpriteEffects.None, 0f);
        }
    }
}