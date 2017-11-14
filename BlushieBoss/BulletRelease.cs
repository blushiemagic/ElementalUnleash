using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.BlushieBoss
{
	public class BulletRelease : Bullet
	{
		public Bullet Follow;
		public float Radius;
		public float Angle;
		public float RotSpeed;
		public float ReleaseSpeed;

		public BulletRelease(Bullet follow, float radius, float angle, float rotSpeed, float releaseSpeed, float size, Texture2D texture)
			: base(follow.Position + radius * angle.ToRotationVector2(), size, texture)
		{
			this.Follow = follow;
			this.Radius = radius;
			this.Angle = angle;
			this.RotSpeed = rotSpeed;
			this.ReleaseSpeed = releaseSpeed;
		}

		public override void Update()
		{
			Angle += RotSpeed;
			if (!Follow.Active)
			{
				Radius += ReleaseSpeed;
			}
			Position = Follow.Position + Radius * Angle.ToRotationVector2();
		}

		public override bool ShouldRemove()
		{
			return Radius > 1600f * (float)Math.Sqrt(2);
		}

		public static BulletRelease NewWhite(Bullet follow, float radius, float angle, float rotSpeed, float releaseSpeed)
		{
			return new BulletRelease(follow, radius, angle, rotSpeed, releaseSpeed, 16f, BlushieBoss.BulletWhiteTexture);
		}
	}
}