using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.BlushieBoss
{
    public class BulletPull : Bullet
    {
        private bool Finished;

        public BulletPull(Vector2 position)
            : base(position, 16f, BlushieBoss.BulletBlackTexture)
        {
            this.Finished = false;
        }

        public override void Update()
        {
            Vector2 offset = BlushieBoss.PosL - Position;
            float length = offset.Length();
            if (offset.Length() <= 6f)
            {
                Position = BlushieBoss.PosL;
                Finished = true;
                return;
            }
            offset.Normalize();
            Position += 6f * offset;
        }

        public override bool ShouldRemove()
        {
            return Finished;
        }
    }
}