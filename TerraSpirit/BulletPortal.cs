using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.TerraSpirit
{
	public class BulletPortal : BulletSingle
	{
		private const float speed = 6f;

		protected Vector2 endPos;
		private int timer = 0;

		public BulletPortal(Vector2 position, Vector2 endPos) : base(position, 48)
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
					var bullet = new BulletRingExpand(position, 6f);
					if (timer == 30)
					{
						bullet.Rotation(MathHelper.Pi / 16f);
					}
					spirit.bullets.Add(bullet);
				}
				if (timer >= 60)
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