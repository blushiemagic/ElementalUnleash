using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.TerraSpirit
{
    public class BulletCross : Bullet
    {
        protected const int size = 32;
        protected const int length = 6000;

        protected Vector2 position;
        protected float rotation;
        protected int delay;
        protected readonly int maxDelay;

        public BulletCross(Vector2 position, float rotation = 0f, int delay = 90)
        {
            this.position = position;
            this.rotation = rotation;
            this.delay = delay;
            this.maxDelay = delay;
        }

        public override bool Update(TerraSpirit spirit, Rectangle bounds)
        {
            delay--;
            return delay > -30;
        }

        public override bool Collides(Rectangle box)
        {
            if (delay > 0)
            {
                Rectangle myBox = new Rectangle((int)position.X - size / 2, (int)position.Y - size / 2, size, size);
                return myBox.Intersects(box);
            }
            else
            {
                Vector2 offset = length / 2 * rotation.ToRotationVector2();
                Vector2 start = position + offset;
                Vector2 end = position - offset;
                float num = 0f;
                if (Collision.CheckAABBvLineCollision(box.TopLeft(), box.Size(), start, end, size, ref num))
                {
                    return true;
                }
                offset = length / 2 * (rotation + MathHelper.PiOver2).ToRotationVector2();
                start = position + offset;
                end = position - offset;
                num = 0f;
                if (Collision.CheckAABBvLineCollision(box.TopLeft(), box.Size(), start, end, size, ref num))
                {
                    return true;
                }
                return false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (delay > 0)
            {
                float glowAlpha = 1f - (float)delay / (float)maxDelay;
                spriteBatch.Draw(Bluemagic.Instance.GetTexture("TerraSpirit/BulletCross"), position - Main.screenPosition, null, Color.White, rotation, new Vector2(size / 2, size / 2), 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(Bluemagic.Instance.GetTexture("TerraSpirit/BulletCrossGlow"), position - Main.screenPosition, null, Color.White * glowAlpha, rotation, new Vector2(size / 2, size / 2), 1f, SpriteEffects.None, 0f);
            }
            else
            {
                Texture2D texture = Bluemagic.Instance.GetTexture("TerraSpirit/BulletBeam");
                Vector2 scale = new Vector2((float)length / (float)size, 1f);
                spriteBatch.Draw(texture, position - Main.screenPosition, null, Color.White, rotation, new Vector2(size / 2, size / 2), scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(texture, position - Main.screenPosition, null, Color.White, rotation + MathHelper.PiOver2, new Vector2(size / 2, size / 2), scale, SpriteEffects.None, 0f);
            }
        }
    }
}