using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.TerraSpirit
{
	public class BulletPortal2 : BulletSingle
	{
		private const float speed = 6f;

		protected Vector2 endPos;
		private int timer = 0;

		public BulletPortal2(Vector2 position, Vector2 endPos) : base(position, 48)
		{
			this.endPos = endPos;
		}

		public override Texture2D Texture
		{
			get
			{
				return Bluemagic.Instance.GetTexture("TerraSpirit/BulletPortal");
			}
		}

		public override bool Update(TerraSpirit spirit, Rectangle bounds)
		{
			Vector2 offset = endPos - position;
			if (offset.Length() < speed)
			{
				position = endPos;
				if (timer % 30 == 0)
				{
					Vector2 target = spirit.GetTarget().Center;
					Vector2 shootVel = target - position;
					if (shootVel == Vector2.Zero)
					{
						shootVel = new Vector2(0f, -1f);
					}
					shootVel.Normalize();
					shootVel *= 12f;
					var bullet = new BulletSingleMove(position, shootVel);
					spirit.bullets.Add(bullet);
				}
				if (timer >= 120)
				{
					return false;
				}
				timer++;
				return true;
			}
			offset.Normalize();
			position += speed * offset;
			return true;
		}
	}
}