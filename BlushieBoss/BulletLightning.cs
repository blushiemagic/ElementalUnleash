using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.BlushieBoss
{
	public class BulletLightning : BulletBounce
	{
		public BulletLightning(Vector2 position, Vector2 velocity)
			: base(position, velocity, 2, 16f, BlushieBoss.BulletPurpleTexture)
		{
		}

		public override void Update()
		{
			Vector2 oldVel = Velocity;
			base.Update();
			if (NumBounces >= 0 && oldVel != Velocity)
			{
				float rot = Velocity.ToRotation();
				float speed = Velocity.Length();
				float newRot1;
				float newRot2;
				if (rot < -MathHelper.PiOver2)
				{
					newRot1 = (rot - MathHelper.Pi) / 2f;
					newRot2 = (rot - MathHelper.PiOver2) / 2f;
				}
				else if (rot < 0f)
				{
					newRot1 = (rot - MathHelper.PiOver2) / 2f;
					newRot2 = rot / 2f;
				}
				else if (rot < MathHelper.PiOver2)
				{
					newRot1 = rot / 2f;
					newRot2 = (rot + MathHelper.PiOver2) / 2f;
				}
				else
				{
					newRot1 = (rot + MathHelper.PiOver2) / 2f;
					newRot2 = (rot + MathHelper.Pi) / 2f;
				}
				var bullet = new BulletLightning(Position, speed * newRot1.ToRotationVector2());
				bullet.NumBounces = NumBounces;
				BlushieBoss.bullets.Add(bullet);
				bullet = new BulletLightning(Position, speed * newRot2.ToRotationVector2());
				bullet.NumBounces = NumBounces;
				BlushieBoss.bullets.Add(bullet);
			}
			if (Main.rand.Next(2) == 0)
			{
				int dust = Dust.NewDust(Position - new Vector2(Size), 32, 32, Bluemagic.Instance.DustType("PurpleLightning"), 0f, 0f, 100, default(Color), 1f);
				Main.dust[dust].velocity *= 0.5f;
				Main.dust[dust].velocity += Velocity;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}
	}
}