using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.TerraSpirit
{
    public class BulletFlowerDoom : Bullet
    {
        protected const int interval = 320;
        protected const int xIntervals = TerraSpirit.arenaWidth / interval;
        protected const int yIntervals = TerraSpirit.arenaHeight / interval;
        protected const int size = 32;

        protected Vector2 origin;
        protected float rotation;
        protected float rotSpeed;
        protected int timer;
        protected int maxTime;

        public BulletFlowerDoom(Vector2 origin, int maxTime, float rotSpeed = 0.03f)
        {
            this.origin = origin - new Vector2(TerraSpirit.arenaWidth / 2, TerraSpirit.arenaHeight / 2);
            this.rotation = 0f;
            this.rotSpeed = rotSpeed;
            this.timer = -90;
            this.maxTime = maxTime;
        }

        public override bool Update(TerraSpirit spirit, Rectangle bounds)
        {
            if (timer >= 0 && timer % 120 == 0)
            {
                Vector2 position = spirit.GetTarget().Center - origin;
                int i = (int)position.X / interval;
                int j = (int)position.Y / interval;
                Vector2 bulletPos = origin + interval * new Vector2(i + 0.5f, j + 0.5f);
                if (timer % 360 == 0)
                {
                    spirit.bullets.Add(new BulletBeamBig(bulletPos, interval, MathHelper.PiOver2, 120));
                }
                else if (timer % 360 == 120)
                {
                    spirit.bullets.Add(new BulletBeamBig(bulletPos, interval, 0f, 120));
                }
                else if (timer % 360 == 240)
                {
                    spirit.bullets.Add(new BulletBeamBig(bulletPos, interval, MathHelper.PiOver2, 120));
                    spirit.bullets.Add(new BulletBeamBig(bulletPos, interval, 0f, 120));
                }
            }

            rotation += rotSpeed;
            timer++;
            return timer < maxTime;
        }

        public override bool Collides(Rectangle box)
        {
            if (timer < 0)
            {
                return false;
            }
            int left = box.X - (int)origin.X;
            int right = left + box.Width;
            int top = box.Y - (int)origin.Y;
            int bottom = top + box.Height;
            left = left / interval - 1;
            right = right / interval + 1;
            top = top / interval - 1;
            bottom = bottom / interval + 1;
            if (left < 0)
            {
                left = 0;
            }
            if (right > xIntervals)
            {
                right = xIntervals;
            }
            if (top < 0)
            {
                top = 0;
            }
            if (bottom > yIntervals)
            {
                bottom = yIntervals;
            }
            for (int i = left; i <= right; i++)
            {
                for (int j = top; j <= bottom; j++)
                {
                    Vector2 center = origin + interval * new Vector2(i + 0.5f, j + 0.5f);
                    for (int k = 0; k < 4; k++)
                    {
                        float angle = rotation + MathHelper.PiOver2 * k;
                        Vector2 offset = interval / 2 * angle.ToRotationVector2();
                        Vector2 checkPos = center + offset;
                        Rectangle myBox = new Rectangle((int)checkPos.X - size / 2, (int)checkPos.Y - size / 2, size, size);
                        if (myBox.Intersects(box))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = Bluemagic.Instance.GetTexture("TerraSpirit/BulletSingle");
            Vector2 drawOrigin = origin - Main.screenPosition - new Vector2(size / 2, size / 2);
            Color color = Color.White;
            if (timer < 0)
            {
                int num = (timer + 90) % 60;
                if (num > 30)
                {
                    num = 60 - num;
                }
                float alpha = num / 30f;
                color *= alpha;
            }
            for (int i = 0; i <= xIntervals; i++)
            {
                for (int j = 0; j <= yIntervals; j++)
                {
                    Vector2 drawCenter = drawOrigin + interval * new Vector2(i + 0.5f, j + 0.5f);
                    for (int k = 0; k < 4; k++)
                    {
                        float angle = rotation + MathHelper.PiOver2 * k;
                        Vector2 offset = interval / 2 * angle.ToRotationVector2();
                        Vector2 drawPos = drawCenter + offset;
                        spriteBatch.Draw(texture, drawPos, color);
                    }
                }
            }
        }
    }
}