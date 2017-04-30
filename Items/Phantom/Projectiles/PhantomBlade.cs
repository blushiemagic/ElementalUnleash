using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom.Projectiles
{
	public class PhantomBlade : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.name = "Phantom Blade";
			projectile.width = 200;
			projectile.height = 12;
			projectile.alpha = 70;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.melee = true;
			projectile.manualDirectionChange = true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if (player.itemAnimation == 0)
			{
				projectile.active = false;
				return;
			}
			if (player.direction + player.gravDir == 0)
			{
				projectile.spriteDirection = -1;
			}
			else
			{
				projectile.spriteDirection = 1;
			}
			projectile.direction = player.direction;
			projectile.position = player.itemLocation;
			projectile.rotation = player.itemRotation;
			projectile.rotation -= (float)Math.PI / 4f;
			if (player.direction == -1)
			{
				projectile.rotation -= (float)Math.PI / 2f;
			}
			if (player.gravDir == -1)
			{
				if (player.direction == 1)
				{
					projectile.rotation += (float)Math.PI / 2f;
				}
				else
				{
					projectile.rotation -= (float)Math.PI / 2f;
				}
			}
			projectile.position.X += 2 * 14f * (float)Math.Cos(projectile.rotation);
			projectile.position.Y += 2 * 14f * (float)Math.Sin(projectile.rotation);
			int y = Main.rand.Next(projectile.height) - projectile.height / 2;
			int x = Main.rand.Next(projectile.width);
			float rotatedX = x * (float)Math.Cos(projectile.rotation) - y * (float)Math.Sin(projectile.rotation);
			float rotatedY = x * (float)Math.Sin(projectile.rotation) + y * (float)Math.Cos(projectile.rotation);
			Dust.NewDust(projectile.position + new Vector2(rotatedX, rotatedY), 0, 0, mod.DustType("SpectreDust"));
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float point = 0f;
			Vector2 endPoint = projectile.position + projectile.width * projectile.rotation.ToRotationVector2();
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.position, endPoint, projectile.height, ref point);
		}

		public override Color? GetAlpha(Color color)
		{
			return Color.White * ((255 - projectile.alpha) / 255f);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawPos = projectile.position - Main.screenPosition;
			Color color = GetAlpha(lightColor).Value;
			float rotation = projectile.rotation;
			Vector2 origin = new Vector2(0f, texture.Height / 2);
			SpriteEffects effects = projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
			spriteBatch.Draw(texture, drawPos, null, color, rotation, origin, 1f, effects, 0f);
			return false;
		}
	}
}