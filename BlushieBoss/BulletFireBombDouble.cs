using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.BlushieBoss
{
    public class BulletFireBombDouble : Bullet
    {
        public int Time;
        public int Time2;
        public float BombDamageMult;

        public BulletFireBombDouble(Vector2 position, int time, int time2, float damageMult = 1f)
            : base(position, 32f, BlushieBoss.BulletFireLargeTexture)
        {
            this.Time = time;
            this.Time2 = time2;
            this.BombDamageMult = damageMult;
            this.Damage = 0f;
        }

        public override void Update()
        {
            Time--;
            if (Time == 0)
            {
                for (int k = 0; k < 16; k++)
                {
                    float rot = MathHelper.TwoPi * k / 16f;
                    Bullet bullet = new BulletFire(Position, 8f * rot.ToRotationVector2());
                    BlushieBoss.AddBullet(bullet, BombDamageMult);
                }
            }
            if (Time < 0)
            {
                Time2--;
            }
            if (Time2 == 0)
            {
                for (int k = 0; k < 16; k++)
                {
                    float rot = MathHelper.TwoPi * (k + 0.5f) / 16f;
                    Bullet bullet = new BulletFire(Position, 8f * rot.ToRotationVector2());
                    BlushieBoss.AddBullet(bullet, BombDamageMult);
                }
            }
            for (int k = 0; k < 1; k++)
            {
                int dust = Dust.NewDust(Position - new Vector2(Size), 64, 64, 6, 0f, 0f, 0, default(Color), 4f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 2f;
            }
        }

        public override bool ShouldRemove()
        {
            return Time < 0 && Time2 < 0;
        }
    }
}