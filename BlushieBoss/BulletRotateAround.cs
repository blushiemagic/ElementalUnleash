using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.BlushieBoss
{
	public class BulletRotateAround : Bullet
	{
		public Bullet Follow;
		public float Radius;
		public float Angle;
		public float RotSpeed;

		public BulletRotateAround(Bullet follow, float radius, float angle, float rotSpeed, float size, Texture2D texture)
			: base(follow.Position + radius * angle.ToRotationVector2(), size, texture)
		{
			this.Follow = follow;
			this.Radius = radius;
			this.Angle = angle;
			this.RotSpeed = rotSpeed;
		}

		public override void Update()
		{
			Angle += RotSpeed;
			Position = Follow.Position + Radius * Angle.ToRotationVector2();
		}

		public override bool ShouldRemove()
		{
			return !Follow.Active;
		}

		public static BulletRotateAround NewBlueSmall(Bullet follow, float radius, float angle, float rotSpeed)
		{
			return new BulletRotateAround(follow, radius, angle, rotSpeed, 8f, BlushieBoss.BulletBlueSmallTexture);
		}
	}
}