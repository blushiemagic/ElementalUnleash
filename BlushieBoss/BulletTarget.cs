using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.BlushieBoss
{
	public class BulletTarget : Bullet
	{
		public Vector2 Start;
		public Vector2 End;
		public int Time;
		public int MaxTime;

		public BulletTarget(Vector2 position, Vector2 target, int time, float size, Texture2D texture)
			: base(position, size, texture)
		{
			this.Start = position;
			this.End = target;
			this.Time = 0;
			this.MaxTime = time;
		}

		public override void Update()
		{
			Time++;
			Position = Vector2.Lerp(Start, End, (float)Time / (float)MaxTime);
		}

		public override bool ShouldRemove()
		{
			return Time > MaxTime;
		}

		public static BulletTarget NewGoldLarge(Vector2 position, Vector2 target, int time)
		{
			return new BulletTarget(position, target, time, 32f, BlushieBoss.BulletGoldLargeTexture);
		}
	}
}