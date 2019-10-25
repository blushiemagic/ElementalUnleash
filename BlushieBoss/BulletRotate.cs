using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.BlushieBoss
{
    public class BulletRotate : Bullet
    {
        public Vector2 Center;
        public float Radius;
        public float Angle;
        public float RotSpeed;
        public int TimeLeft;

        public BulletRotate(Vector2 center, float radius, float angle, float rotSpeed, int timeLeft, float size, Texture2D texture)
            : base(center + radius * angle.ToRotationVector2(), size, texture)
        {
            this.Center = center;
            this.Radius = radius;
            this.Angle = angle;
            this.RotSpeed = rotSpeed;
            this.TimeLeft = timeLeft;
        }

        public override void Update()
        {
            Angle += RotSpeed;
            Position = Center + Radius * Angle.ToRotationVector2();
            if (TimeLeft > 0)
            {
                TimeLeft--;
            }
        }

        public override bool ShouldRemove()
        {
            return TimeLeft == 0;
        }

        public static BulletRotate NewGold(Vector2 center, float radius, float angle, float rotSpeed, int timeLeft)
        {
            return new BulletRotate(center, radius, angle, rotSpeed, timeLeft, 16f, BlushieBoss.BulletGoldTexture);
        }
    }
}