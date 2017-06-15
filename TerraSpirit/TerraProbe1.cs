using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.TerraSpirit
{
	public class TerraProbe1 : TerraProbe
	{
		public override void Behavior()
		{
			Timer++;
			if (Timer >= 90)
			{
				Timer = 0;
				TerraSpirit spirit = (TerraSpirit)Spirit.modNPC;
				Player target = spirit.GetTarget();
				spirit.bullets.Add(new BulletExplode(npc.Center, target.Center));
			}
		}
	}

	public class BulletExplode : BulletSingle
	{
		private Vector2 endPos;
		private const float speed = 8f;
		private Vector2[] oldPos = new Vector2[8];

		public BulletExplode(Vector2 pos, Vector2 endPos) : base(pos)
		{
			this.endPos = endPos;
		}

		public override bool Update(TerraSpirit spirit, Rectangle bounds)
		{
			for (int k = oldPos.Length - 1; k > 0; k--)
			{
				oldPos[k] = oldPos[k - 1];
			}
			oldPos[0] = position;
			Vector2 offset = endPos - position;
			if (offset.Length() < speed)
			{
				spirit.bullets.Add(new BulletRingExpand(endPos, 6f));
				return false;
			}
			offset.Normalize();
			position += speed * offset;
			return true;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Texture2D texture = Bluemagic.Instance.GetTexture("TerraSpirit/BulletSingle");
			for (int k = oldPos.Length - 1; k >= 0; k -= 2)
			{
				float alpha = 1f - (float)(k + 1) / (float)(oldPos.Length + 2);
				spriteBatch.Draw(texture, oldPos[k] - Main.screenPosition - new Vector2(size / 2, size / 2), Color.White * alpha);
			}
			base.Draw(spriteBatch);
		}
	}
}
