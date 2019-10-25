using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.BlushieBoss
{
    public class BulletTargetSmooth : Bullet
    {
        public Vector2 End;
        public int Time;

        public BulletTargetSmooth(Vector2 position, Vector2 target, int time, float size, Texture2D texture)
            : base(position, size, texture)
        {
            this.End = target;
            this.Time = time;
        }

        public override void Update()
        {
            Time--;
            Vector2 offset = End - Position;
            offset *= 0.1f;
            if (offset.Length() > 12f)
            {
                offset.Normalize();
                offset *= 12f;
            }
            Position += offset;
        }

        public override bool ShouldRemove()
        {
            return Time < 0;
        }

        public static BulletTargetSmooth NewBlueLarge(Vector2 position, Vector2 target, int time)
        {
            return new BulletTargetSmooth(position, target, time, 32f, BlushieBoss.BulletBlueLargeTexture);
        }
    }
}