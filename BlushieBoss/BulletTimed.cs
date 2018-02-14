using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.BlushieBoss
{
	public class BulletTimed : Bullet
	{
		public Vector2 Velocity;
		public int Time;

		public BulletTimed(Vector2 position, Vector2 velocity, int time, float size, Texture2D texture)
			: base(position, size, texture)
		{
			this.Velocity = velocity;
			this.Time = time;
		}

		public override void Update()
		{
			this.Position += this.Velocity;
			this.Time--;
		}

		public override bool ShouldRemove()
		{
			return Time <= 0;
		}

		public static BulletTimed NewDragonBreath(Vector2 position, Vector2 velocity, int time)
		{
			return new BulletTimed(position, velocity, time, 16f, BlushieBoss.BulletDragonBreathTexture);
		}
	}
}