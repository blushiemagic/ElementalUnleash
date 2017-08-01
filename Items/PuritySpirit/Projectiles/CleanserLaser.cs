using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit.Projectiles
{
	public class CleanserLaser : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cleanser Beam");
			ProjectileID.Sets.NeedsUUID[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.aiStyle = 84;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.alpha = 255;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.hide = true;
		}

		public override bool PreAI()
		{
			int uuid = Projectile.GetByUUID(projectile.owner, projectile.ai[1]);
			if (uuid < 0)
			{
				projectile.Kill();
				return false;
			}
			Projectile cannon = Main.projectile[uuid];
			if (cannon.active && cannon.type == mod.ProjectileType("CleanserBeam"))
			{
				Vector2 direction = Vector2.Normalize(cannon.velocity);
				projectile.Center = cannon.Center + 16f * direction + new Vector2(0f, -cannon.gfxOffY);
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
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, endPoint, 4f, ref point);
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			damage += target.defense / 2;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.penetrate++;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 unit = projectile.velocity * projectile.localAI[1];
			float length = unit.Length();
			unit.Normalize();
			byte colorStrength = (byte)(100f + 100f * Math.Sin(projectile.localAI[0] / 40f * MathHelper.TwoPi));
			Color color = new Color(colorStrength, 255, colorStrength) * 0.8f;
			for (float k = 0; k <= length; k += 4f)
			{
				Vector2 drawPos = projectile.Center + unit * k - Main.screenPosition;
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, new Vector2(2, 2), 1f, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}