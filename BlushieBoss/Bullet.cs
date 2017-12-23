using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.BlushieBoss
{
	public abstract class Bullet
	{
		public Vector2 Position;
		public float Size;
		public Texture2D Texture;
		public float Damage;
		public bool Active;

		public Bullet(Vector2 position, float size, Texture2D texture)
		{
			this.Position = position;
			this.Size = size;
			this.Texture = texture;
			this.Damage = 0.1f;
			this.Active = true;
		}

		public abstract void Update();

		public virtual bool ShouldRemove()
		{
			return (Position.X + Size < BlushieBoss.Origin.X - BlushieBoss.ArenaSize)
				|| (Position.X - Size > BlushieBoss.Origin.X + BlushieBoss.ArenaSize)
				|| (Position.Y + Size < BlushieBoss.Origin.Y - BlushieBoss.ArenaSize)
				|| (Position.Y - Size > BlushieBoss.Origin.Y + BlushieBoss.ArenaSize);
		}
	}
}