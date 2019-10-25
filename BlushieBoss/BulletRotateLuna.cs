using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.BlushieBoss
{
    public class BulletRotateLuna : Bullet
    {
        public float Radius;
        public float Direction;
        public float AngleOffset;

        public BulletRotateLuna(float direction, float angleOffset)
            : base(BlushieBoss.PosL, 32f, BlushieBoss.BulletGoldLargeTexture)
        {
            this.Radius = 0f;
            this.Direction = direction;
            this.AngleOffset = angleOffset;
        }

        public override void Update()
        {
            float angle = Direction * BlushieBoss.Timer * MathHelper.TwoPi / 600f + AngleOffset;
            Radius += 4f;
            Position = BlushieBoss.PosL + Radius * angle.ToRotationVector2();
        }

        public override bool ShouldRemove()
        {
            return Radius > 1600f * (float)Math.Sqrt(2);
        }
    }
}