using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.BlushieBoss
{
    public class BulletBounce : Bullet
    {
        public Vector2 Velocity;
        public int NumBounces;

        public BulletBounce(Vector2 position, Vector2 velocity, int numBounces, float size, Texture2D texture)
            : base(position, size, texture)
        {
            this.Velocity = velocity;
            this.NumBounces = numBounces;
        }

        public override void Update()
        {
            this.Position += this.Velocity;
            Vector2 origin = BlushieBoss.Origin;
            float arenaSize = BlushieBoss.ArenaSize;
            if (Position.X - Size <= origin.X - arenaSize && Velocity.X < 0f)
            {
                Position.X = 2 * (origin.X - arenaSize) - (Position.X - Size) + Size;
                Velocity.X *= -1f;
                NumBounces--;
            }
            if (Position.X + Size >= origin.X + arenaSize && Velocity.X > 0f)
            {
                Position.X = 2 * (origin.X + arenaSize) - (Position.X + Size) - Size;
                Velocity.X *= -1f;
                NumBounces--;
            }
            if (Position.Y - Size <= origin.Y - arenaSize && Velocity.Y < 0f)
            {
                Position.Y = 2 * (origin.Y - arenaSize) - (Position.Y - Size) + Size;
                Velocity.Y *= -1f;
                NumBounces--;
            }
            if (Position.Y + Size >= origin.Y + arenaSize && Velocity.Y > 0f)
            {
                Position.Y = 2 * (origin.Y + arenaSize) - (Position.Y + Size) - Size;
                Velocity.Y *= -1f;
                NumBounces--;
            }
        }

        public override bool ShouldRemove()
        {
            return NumBounces < 0;
        }

        public static BulletBounce NewStar(Vector2 position, Vector2 velocity, int numBounces)
        {
            BulletBounce bullet = new BulletBounce(position, velocity, numBounces, 16f, BlushieBoss.BulletStarTexture);
            bullet.Damage = 0.075f;
            return bullet;
        }

        public static BulletBounce NewBone(Vector2 position, Vector2 velocity, int numBounces)
        {
            return new BulletBounce(position, velocity, numBounces, 16f, BlushieBoss.BulletBoneTexture);
        }
    }
}