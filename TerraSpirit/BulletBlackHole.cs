using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.TerraSpirit
{
    public class BulletBlackHole : Bullet
    {
        protected const int blackHoleSize = 48;
        protected const int bulletSize = 32;
        protected const float gravity = 400f;

        protected Vector2 origin;
        
        protected int timer;
        protected int rate;

        protected List<Vector2> blackHoles;
        protected List<Vector2> bulletPos;
        protected List<Vector2> bulletVel;

        public BulletBlackHole(Vector2 origin)
        {
            this.origin = origin;
            this.timer = -120;
            this.rate = 0;

            const int width = TerraSpirit.arenaWidth;
            const int height = TerraSpirit.arenaHeight;
            const int halfWidth = width / 2;
            const int halfHeight = height / 2;
            this.blackHoles = new List<Vector2>();
            this.blackHoles.Add(new Vector2(-halfWidth / 2, -halfHeight / 2));
            this.blackHoles.Add(new Vector2(halfWidth / 2, -halfHeight / 2));
            this.blackHoles.Add(new Vector2(-halfWidth / 2, halfHeight / 2));
            this.blackHoles.Add(new Vector2(halfWidth / 2, halfHeight / 2));

            this.bulletPos = new List<Vector2>();
            this.bulletVel = new List<Vector2>();
            int num = 0;
            const float speed = 1f;
            const int interval = 200;
            for (int i = -halfWidth / interval + 1; i < halfWidth / interval; i++)
            {
                for (int j = -halfHeight / interval + 1; j < halfHeight / interval; j++)
                {
                    this.bulletPos.Add(new Vector2(i * interval, j * interval));
                    switch (num)
                    {
                    case 0:
                        this.bulletVel.Add(new Vector2(-speed, -speed));
                        break;
                    case 1:
                        this.bulletVel.Add(new Vector2(speed, -speed));
                        break;
                    case 2:
                        this.bulletVel.Add(new Vector2(-speed, speed));
                        break;
                    case 3:
                        this.bulletVel.Add(new Vector2(speed, speed));
                        break;
                    }
                    num = (num + 1) % 4;
                }
            }
        }

        public override bool Update(TerraSpirit spirit, Rectangle bounds)
        {
            int numUpdates = timer / 20;
            if (numUpdates > 5)
            {
                numUpdates = 5;
            }
            for (int k = 0; k < numUpdates; k++)
            {
                for (int i = 0; i < bulletPos.Count; i++)
                {
                    foreach (Vector2 blackHole in blackHoles)
                    {
                        Vector2 offset = blackHole - bulletPos[i];
                        float distance = offset.Length();
                        if (distance != 0f)
                        {
                            if (distance < 20f)
                            {
                                distance = 20f;
                            }
                            offset.Normalize();
                            bulletVel[i] += gravity * offset / distance / distance;
                        }
                    }
                    if (bulletPos[i].X < -TerraSpirit.arenaWidth / 2 && bulletVel[i].X < 0f)
                    {
                        bulletVel[i] = new Vector2(-bulletVel[i].X, bulletVel[i].Y);
                    }
                    if (bulletPos[i].X > TerraSpirit.arenaWidth / 2 && bulletVel[i].X > 0f)
                    {
                        bulletVel[i] = new Vector2(-bulletVel[i].X, bulletVel[i].Y);
                    }
                    if (bulletPos[i].Y < -TerraSpirit.arenaHeight / 2 && bulletVel[i].Y < 0f)
                    {
                        bulletVel[i] = new Vector2(bulletVel[i].X, -bulletVel[i].Y);
                    }
                    if (bulletPos[i].Y > TerraSpirit.arenaHeight / 2 && bulletVel[i].Y > 0f)
                    {
                        bulletVel[i] = new Vector2(bulletVel[i].X, -bulletVel[i].Y);
                    }
                    bulletPos[i] += bulletVel[i];
                }
            }
            timer++;
            return timer < 720;
        }

        public override bool Collides(Rectangle box)
        {
            if (timer < 0)
            {
                return false;
            }
            foreach (Vector2 blackHole in blackHoles)
            {
                Vector2 pos = origin + blackHole - new Vector2(blackHoleSize / 2, blackHoleSize / 2);
                Rectangle myBox = new Rectangle((int)pos.X, (int)pos.Y, blackHoleSize, blackHoleSize);
                if (myBox.Intersects(box))
                {
                    return true;
                }
            }
            foreach (Vector2 bullet in bulletPos)
            {
                Vector2 pos = origin + bullet - new Vector2(bulletSize / 2, bulletSize / 2);
                Rectangle myBox = new Rectangle((int)pos.X, (int)pos.Y, bulletSize, bulletSize);
                if (myBox.Intersects(box))
                {
                    return true;
                }
            }
            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            if (timer < 0)
            {
                int num = (timer + 120) % 48;
                if (num > 24)
                {
                    num = 48 - num;
                }
                float alpha = num / 24f;
                color *= alpha;
            }
            Texture2D texture = Bluemagic.Instance.GetTexture("TerraSpirit/BulletPortal");
            foreach (Vector2 pos in blackHoles)
            {
                spriteBatch.Draw(texture, origin + pos - Main.screenPosition - new Vector2(blackHoleSize / 2, blackHoleSize / 2), color);
            }
            texture = Bluemagic.Instance.GetTexture("TerraSpirit/BulletSingle");
            foreach (Vector2 pos in bulletPos)
            {
                spriteBatch.Draw(texture, origin + pos - Main.screenPosition - new Vector2(bulletSize / 2, bulletSize / 2), color);
            }
        }
    }
}