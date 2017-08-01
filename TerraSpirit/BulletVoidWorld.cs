using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.TerraSpirit
{
	public class BulletVoidWorld : Bullet
	{
		private const int size = 80;

		private Vector2 position;
		private int timer;

		public BulletVoidWorld(Vector2 position)
		{
			this.position = position;
			this.timer = 0;
		}

		public override bool Update(TerraSpirit spirit, Rectangle bounds)
		{
			timer++;
			return timer < 80;
		}

		public override bool Collides(Rectangle box)
		{
			if (timer < 70)
			{
				return false;
			}
			Rectangle myBox = new Rectangle((int)position.X - size / 2, (int)position.Y - size / 2, size, size);
			return myBox.Intersects(box);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Texture2D texture = Bluemagic.Instance.GetTexture("TerraSpirit/BulletVoidWorld");
			Rectangle frame = new Rectangle(0, (size + 2) * (timer / 10), size, size);
			Vector2 drawPos = position - Main.screenPosition - new Vector2(size / 2, size / 2);
			spriteBatch.Draw(texture, drawPos, frame, Color.White);
		}
	}
}