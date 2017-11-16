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

		public static BulletSimple NewWhite(Vector2 position, Vector2 velocity)
		{
			return new BulletSimple(position, velocity, 16f, BlushieBoss.BulletWhiteTexture);
		}

		public static BulletSimple NewBoxBlue(Vector2 position, Vector2 velocity)
		{
			return new BulletSimple(position, velocity, 16f, BlushieBoss.BulletBoxBlueTexture);
		}

		public static BulletSimple NewColor(Vector2 position, Vector2 velocity, int color)
		{
			var bullet = new BulletSimple(position, velocity, 16f, BlushieBoss.BulletColorTextures[color]);
			bullet.Damage = 0.075f;
			return bullet;
		}

		public static BulletSimple NewLight(Vector2 position, Vector2 velocity)
		{
			return new BulletSimple(position, velocity, 16f, BlushieBoss.BulletLightTexture);
		}
	}
}