using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.TerraSpirit
{
    public class BulletChase : BulletSingle
    {
        protected int timer;
        protected int interval;
        protected int life;
        protected float speed;
        protected Func<Vector2, TerraSpirit, Bullet> action;

        public override Texture2D Texture
        {
            get
            {
                return Bluemagic.Instance.GetTexture("TerraSpirit/BulletChase");
            }
        }

        public BulletChase(Vector2 position, int interval, int life, Func<Vector2, TerraSpirit, Bullet> action, float speed = 0.05f) : base(position)
        {
            this.position = position;
            this.interval = interval;
            this.life = life;
            this.speed = speed;
            this.action = action;
        }

        public override bool Update(TerraSpirit spirit, Rectangle bounds)
        {
            Vector2 target = spirit.GetTarget().Center;
            position += (target - position) * speed;
            timer++;
            if (timer % interval == 0)
            {
                Bullet newBullet = action(position, spirit);
                if (newBullet != null)
                {
                    spirit.bullets.Add(newBullet);
                }
            }
            return timer < life;
        }
    }
}