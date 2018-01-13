using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Blushie
{
	public class FrostFairyLaser : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frosty Laser Beam");
			ProjectileID.Sets.NeedsUUID[projectile.type] = true;
			ProjectileID.Sets.MinionShot[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = 84;
			projectile.friendly = true;
			projectile.alpha = 255;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
		}

		public override bool PreAI()
		{
			int uuid = Projectile.GetByUUID(projectile.owner, projectile.ai[1]);
			if (uuid < 0)
			{
				projectile.Kill();
				return false;
			}
			Projectile wings = Main.projectile[uuid];
			if (wings.active && wings.type == mod.ProjectileType("FrostFairyWingsProj"))
			{
				projectile.Center = wings.Center;
				if (projectile.ai[0] == 0f)
				{
					projectile.Center += new Vector2(-18f, -13f);
				}
				else if (projectile.ai[0] == 1f)
				{
					projectile.Center += new Vector2(18f, -13f);
				}
				else if (projectile.ai[0] == 2f)
				{
					projectile.Center += new Vector2(-16f, 15f);
				}
				else
				{
					projectile.Center += new Vector2(16f, 15f);
				}
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
			float length = 1000f;
			projectile.localAI[1] = MathHelper.Lerp(projectile.localAI[1], length, 0.5f);

			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] > 10f)
			{
				projectile.Kill();
			}
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
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, endPoint, 16f, ref point);
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			damage += target.defense / 2;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 direction = projectile.velocity * projectile.localAI[1];
			float rotation = direction.ToRotation();
			float length = projectile.localAI[1];
			Vector2 drawPos = projectile.Center - Main.screenPosition;
			Color color = new Color(100, 190, 255);
			spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, rotation, new Vector2(0f, 2f), new Vector2(length / 4f, 4f), SpriteEffects.None, 0f);
			color = new Color(127, 255, 255);
			spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, rotation, new Vector2(0f, 2f), new Vector2(length / 4f, 3f), SpriteEffects.None, 0f);
			color = new Color(255, 255, 255);
			spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, rotation, new Vector2(0f, 2f), new Vector2(length / 4f, 2f), SpriteEffects.None, 0f);
			
			return false;
		}
	}
}