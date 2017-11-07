using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.BlushieBoss
{
	public class BulletRotateKylie : Bullet
	{
		public float Radius;
		public float Angle;
		public float RotSpeed;
		public int Timer;

		public BulletRotateKylie(float angle, float rotSpeed)
			: base(BlushieBoss.PosK, 16f, BlushieBoss.BulletBlueTexture)
		{
			this.Radius = 0f;
			this.Angle = angle;
			this.RotSpeed = rotSpeed;
		}

		public override void Update()
		{
			Timer++;
			Radius = 240f * (1f - (float)Math.Cos(Timer * MathHelper.TwoPi / 600f));
			Angle += RotSpeed;
			Position = BlushieBoss.PosK + Radius * Angle.ToRotationVector2();
		}

		public override bool ShouldRemove()
		{
			return Timer > 600;
		}
	}
}