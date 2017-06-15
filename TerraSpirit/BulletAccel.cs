using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.TerraSpirit
{
	public class BulletAccel : BulletSingle
	{
		protected Vector2 direction;
		protected float speed;
		protected float acceleration;

		public BulletAccel(Vector2 position, Vector2 direction, float acceleration = 0.2f) : base(position)
		{
			this.direction = direction;
			this.direction.Normalize();
			this.speed = 0f;
			this.acceleration = acceleration;
		}

		public BulletAccel(Vector2 position, float direction, float acceleration = 0.2f) : base(position)
		{
			this.direction = direction.ToRotationVector2();
			this.speed = 0f;
			this.acceleration = acceleration;
		}

		public override bool Update(TerraSpirit spirit, Rectangle bounds)
		{
			speed += acceleration;
			position += speed * direction;
			return position.X >= bounds.X && position.X <= bounds.Right && position.Y >= bounds.Y && position.Y <= bounds.Bottom;
		}
	}
}