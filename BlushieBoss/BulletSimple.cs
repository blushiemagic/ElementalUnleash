using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.BlushieBoss
{
    public class BulletSimple : Bullet
    {
        public Vector2 Velocity;

        public BulletSimple(Vector2 position, Vector2 velocity, float size, Texture2D texture)
            : base(position, size, texture)
        {
            this.Velocity = velocity;
        }

        public override void Update()
        {
            this.Position += this.Velocity;
        }

        public override bool ShouldRemove()
        {
            return (Velocity.X < 0f && Position.X + Size < BlushieBoss.Origin.X - BlushieBoss.ArenaSize)
                || (Velocity.X > 0f && Position.X - Size > BlushieBoss.Origin.X + BlushieBoss.ArenaSize)
                || (Velocity.Y < 0f && Position.Y + Size < BlushieBoss.Origin.Y - BlushieBoss.ArenaSize)
                || (Velocity.Y > 0f && Position.Y - Size > BlushieBoss.Origin.Y + BlushieBoss.ArenaSize);
        }
        public static BulletSimple NewWhite(Vector2 position, Vector2 velocity)
        {
            return new BulletSimple(position, velocity, 16f, BlushieBoss.BulletWhiteTexture.Value);
        }

        public static BulletSimple NewBoxBlue(Vector2 position, Vector2 velocity)
        {
            return new BulletSimple(position, velocity, 16f, BlushieBoss.BulletBoxBlueTexture.Value);
        }

        public static BulletSimple NewColor(Vector2 position, Vector2 velocity, int color)
        {
            var bullet = new BulletSimple(position, velocity, 16f, BlushieBoss.BulletColorTextures[color].Value);
            bullet.Damage = 0.075f;
            return bullet;
        }

        public static BulletSimple NewLight(Vector2 position, Vector2 velocity)
        {
            return new BulletSimple(position, velocity, 16f, BlushieBoss.BulletLightTexture.Value);
        }

        public static BulletSimple NewDragon(Vector2 position, Vector2 velocity)
        {
            return new BulletSimple(position, velocity, 16f, BlushieBoss.BulletDragonTexture.Value);
        }

        public static BulletSimple NewDragonBreath(Vector2 position, Vector2 velocity)
        {
            return new BulletSimple(position, velocity, 16f, BlushieBoss.BulletDragonBreathTexture.Value);
        }

        public static BulletSimple NewSkull(Vector2 position, Vector2 velocity)
        {
            return new BulletSimple(position, velocity, 16f, BlushieBoss.BulletSkullTexture.Value);
        }

        public static BulletSimple NewBone(Vector2 position, Vector2 velocity)
        {
            return new BulletSimple(position, velocity, 16f, BlushieBoss.BulletBoneTexture.Value);
        }

        public static BulletSimple NewDragonLarge(Vector2 position, Vector2 velocity)
        {
            return new BulletSimple(position, velocity, 32f, BlushieBoss.BulletDragonLargeTexture.Value);
        }
    }
}