using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Projectiles.PuritySpiritWeap.VoidEmissary
{
	public class VoidLaser : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.name = "Void Laser";
			projectile.width = 20;
			projectile.height = 20;
			projectile.aiStyle = 84;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
			ProjectileID.Sets.MinionShot[projectile.type] = true;
		}

		public override bool PreAI()
		{
			Projectile minion = Main.projectile[(int)projectile.ai[1]];
			if (minion.active && minion.type == mod.ProjectileType("VoidEmissary") && minion.ai[0] == 2f)
			{
				Vector2 direction = minion.ai[1].ToRotationVector2();
				projectile.Center = minion.Center + 30f * direction + new Vector2(0f, -minion.gfxOffY);
				projectile.velocity = direction;
			}
			else
			{
				projectile.Kill();
				return false;
			}
			if (projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
			{
				projectile.velocity = -Vector2.UnitY;
			}
			float rotation = projectile.velocity.ToRotation();
			projectile.rotation = rotation - MathHelper.PiOver2;
			projectile.velocity = rotation.ToRotationVector2();

			int startTileX = (int)projectile.Center.X / 16;
			int startTileY = (int)projectile.Center.Y / 16;
			Vector2 endPoint = projectile.Center + projectile.velocity * 16f * 150f;
			int endTileX = (int)endPoint.X / 16;
			int endTileY = (int)endPoint.Y / 16;
			Tuple<int, int> collideTile;
			float length;
			if (!Collision.TupleHitLine(startTileX, startTileY, endTileX, endTileY, 0, 0, new List<Tuple<int, int>>(), out collideTile))
			{
				length = new Vector2(collideTile.Item1 - startTileX, collideTile.Item2 - startTileY).Length() * 16f;
			}
			else if (collideTile.Item1 == endTileX && collideTile.Item2 == endTileY)
			{
				length = 2400f;
			}
			else
			{
				length = new Vector2(collideTile.Item1 - startTileX, collideTile.Item2 - startTileY).Length() * 16f;
			}
			projectile.localAI[1] = MathHelper.Lerp(projectile.localAI[1], length, 0.5f);

			projectile.localAI[0] += 1f;
			projectile.localAI[0] %= 40f;

			return false;
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float point = 0f;
			Vector2 endPoint = projectile.Center + projectile.velocity * projectile.localAI[1];
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, endPoint, 20f, ref point);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 position = projectile.Center - Main.screenPosition;
			Vector2 origin = new Vector2(0, texture.Height / 2);
			Vector2 scale = new Vector2(projectile.localAI[1] / 2f, 1f);
			spriteBatch.Draw(texture, position, null, Color.White, projectile.velocity.ToRotation(), origin, scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}