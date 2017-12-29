using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.BlushieBoss
{
	public class BulletWavy : Bullet
	{
		public Vector2 Velocity;
		public float Amplitude;
		public float Period;
		public int Timer;

		public BulletWavy(Vector2 position, Vector2 velocity, float amplitude, float period, float size, Texture2D texture)
			: base(position, size, texture)
		{
			this.Velocity = velocity;
			this.Amplitude = amplitude;
			this.Period = period;
			this.Timer = 0;
		}

		public override void Update()
		{
			float y = Amplitude * (float)Math.Sin(Timer / this.Period * MathHelper.TwoPi);
			Vector2 dir = this.Velocity;
			dir.Normalize();
			this.Position += (y + this.Velocity.Length()) * dir;
			this.Timer++;
		}

		public override bool ShouldRemove()
		{
			return (Velocity.X < 0f && Position.X + Size + Amplitude / Period < BlushieBoss.Origin.X - BlushieBoss.ArenaSize)
				|| (Velocity.X > 0f && Position.X - Size - Amplitude / Period > BlushieBoss.Origin.X + BlushieBoss.ArenaSize)
				|| (Velocity.Y < 0f && Position.Y + Size + Amplitude / Period < BlushieBoss.Origin.Y - BlushieBoss.ArenaSize)
				|| (Velocity.Y > 0f && Position.Y - Size - Amplitude / Period > BlushieBoss.Origin.Y + BlushieBoss.ArenaSize);
		}

		public static BulletWavy NewDragonBreath(Vector2 position, Vector2 velocity, float amplitude, float period)
		{
			return new BulletWavy(position, velocity, amplitude, period, 16f, BlushieBoss.BulletDragonBreathTexture);
		}

		public static BulletWavy NewSkull(Vector2 position, Vector2 velocity, float amplitude, float period)
		{
			return new BulletWavy(position, velocity, amplitude, period, 16f, BlushieBoss.BulletSkullTexture);
		}
	}
}