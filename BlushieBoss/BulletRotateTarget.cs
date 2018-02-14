using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.BlushieBoss
{
	public class BulletRotateTarget : BulletRotate
	{
		public float RadiusTarget;

		public BulletRotateTarget(Vector2 center, float radiusTarget, float angle, float rotSpeed, int timeLeft, float size, Texture2D texture)
			: base(center, 0f, angle, rotSpeed, timeLeft, size, texture)
		{
			this.RadiusTarget = radiusTarget;
		}

		public override void Update()
		{
			float distance = (this.RadiusTarget - this.Radius) * 0.2f;
			if (distance > RadiusTarget / 200f)
			{
				distance = RadiusTarget / 200f;
			}
			Radius += distance;
			base.Update();
		}

		public static BulletRotate NewBone(Vector2 center, float radiusTarget, float angle, float rotSpeed, int timeLeft)
		{
			return new BulletRotateTarget(center, radiusTarget, angle, rotSpeed, timeLeft, 16f, BlushieBoss.BulletBoneTexture);
		}
	}
}