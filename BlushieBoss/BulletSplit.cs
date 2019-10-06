using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.BlushieBoss
{
	public class BulletSplit : Bullet
	{
		public float Speed;
		public float Threshold;
		public int Timer;

		public BulletSplit(Vector2 position, float speed, float threshold, float size, Texture2D texture)
			: base(position, size, texture)
		{
			this.Speed = speed;
			this.Threshold = threshold;
			this.Timer = 60;
		}

		public override void Update()
		{
			if (this.Position.Y >= this.Threshold)
			{
				this.Timer--;
				if (this.ShouldRemove())
				{
					for (int k = 0; k < 4; k++)
					{
						float rot = MathHelper.TwoPi * (k + 0.5f) / 4f;
						Bullet bullet = BulletSimple.NewDragon(this.Position, this.Speed * rot.ToRotationVector2());
						BlushieBoss.AddBullet(bullet, this.Damage);
					}
				}
			}
			else
			{
				this.Position.Y += this.Speed;
			}
		}

		public override bool ShouldRemove()
		{
			return this.Timer <= 0;
		}
		
		public static BulletSplit NewDragonDiamond(Vector2 position, float speed, float threshold)
		{
			return new BulletSplit(position, speed, threshold, 16f, BlushieBoss.BulletDragonDiamondTexture);
		}
	}
}