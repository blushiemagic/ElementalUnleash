using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.TerraSpirit
{
	public class BulletSlide : Bullet
	{
		private const int size = 32;
		private const int interval = 240;
		private const int space = 80;

		private Vector2 origin;
		private int progress;

		private float Top
		{
			get
			{
				int top = (progress * 4) % space;
				if (top < 0f)
				{
					top += space;
				}
				return top;
			}
		}

		private float Offset
		{
			get
			{
				return (float)Math.Sin(MathHelper.TwoPi * progress / 120) * interval / 2;
			}
		}

		private bool Reverse
		{
			get
			{
				int temp = (progress * 4) % (2 * space);
				if (temp < 0)
				{
					temp += 2 * space;
				}
				return temp >= space;
			}
		}

		public BulletSlide(Vector2 origin)
		{
			this.origin = origin - new Vector2(TerraSpirit.arenaWidth / 2, TerraSpirit.arenaHeight / 2);
			this.progress = -120;
		}

		public override bool Update(TerraSpirit spirit, Rectangle bounds)
		{
			progress++;
			return progress <= 600;
		}

		public override bool Collides(Rectangle box)
		{
			if (progress < 0)
			{
				return false;
			}
			box.X -= size / 2;
			box.Y -= size / 2;
			box.Width += size / 2;
			box.Height += size / 2;
			box.Y -= (int)Top;
			box.X -= (int)origin.X;
			box.Y -= (int)origin.Y;
			int top = box.Top / space;
			int bottom = box.Bottom / space + 1;
			int left = (box.Left + interval / 2) / interval;
			int right = (box.Right + interval / 2) / interval + 1;
			for (int x = left; x <= right; x++)
			{
				bool opposite = x % 2 == 0;
				int xPos = (int)(x * interval + (opposite ? Offset : -Offset));
				int yStart = top;
				if ((opposite != Reverse) == (top % 2 == 0))
				{
					yStart++;
				}
				for (int y = yStart; y <= bottom; y += 2)
				{
					int yPos = (int)(y * space);
					if (xPos >= box.Left && xPos <= box.Right && yPos >= box.Top && yPos <= box.Bottom)
					{
						return true;
					}
				}
			}
			return false;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Texture2D texture = Bluemagic.Instance.GetTexture("TerraSpirit/BulletSingle");
			float alpha = 1f;
			if (progress < 0)
			{
				alpha = (progress + 120) % 80 / 40f;
				if (alpha > 1f)
				{
					alpha = 2f - alpha;
				}
			}
			Color color = Color.White * alpha;
			Vector2 drawOrigin = origin - Main.screenPosition - new Vector2(size / 2, size / 2);
			for (int x = 0; x <= 2 * TerraSpirit.arenaWidth / interval + 1; x++)
			{
				bool opposite = x % 2 == 0;
				float xPos = x * interval + (opposite ? Offset : -Offset);
				int yStart = 0;
				if (opposite != Reverse)
				{
					yStart++;
				}
				for (int y = yStart; y <= 2 * TerraSpirit.arenaHeight / space + 1; y += 2)
				{
					float yPos = Top + y * space;
					Vector2 drawPos = drawOrigin + new Vector2(xPos, yPos);
					spriteBatch.Draw(texture, drawPos, color);
				}
			}
		}
	}
}