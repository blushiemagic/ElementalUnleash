using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Phantom
{
	public class PhantomBladeHostile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phantom Blade");
		}

		public override void SetDefaults()
		{
			projectile.width = 400;
			projectile.height = 24;
			projectile.alpha = 70;
			projectile.hostile = true;
			projectile.melee = true;
			projectile.maxPenetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}

		public override void AI()
		{
			if (projectile.ai[1] == 0f)
			{
				projectile.rotation = -MathHelper.Pi * 0.75f;
			}
			NPC center = Main.npc[(int)projectile.ai[0]];
			projectile.position = center.Center;
			projectile.rotation += (float)Math.PI * 1.5f / 60f;
			projectile.position.X += (float)Math.Cos(projectile.rotation);
			projectile.position.Y += (float)Math.Sin(projectile.rotation);

			projectile.ai[1] += 1f;
			if (projectile.ai[1] > 60f)
			{
				projectile.Kill();
			}

			int y = Main.rand.Next(projectile.height) - projectile.height / 2;
			int x = Main.rand.Next(projectile.width);
			float rotatedX = x * (float)Math.Cos(projectile.rotation) - y * (float)Math.Sin(projectile.rotation);
			float rotatedY = x * (float)Math.Sin(projectile.rotation) + y * (float)Math.Cos(projectile.rotation);
			Dust.NewDust(projectile.position + new Vector2(rotatedX, rotatedY), 0, 0, mod.DustType("Phantom"));
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float point = 0f;
			Vector2 endPoint = projectile.position + projectile.width * projectile.rotation.ToRotationVector2();
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.position, endPoint, projectile.height, ref point);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(2) == 0)
			{
				target.AddBuff(mod.BuffType("EtherealFlames"), 300, true);
			}
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
			SpriteEffects effects = SpriteEffects.None;
			spriteBatch.Draw(texture, drawPos, null, color, rotation, origin, 1f, effects, 0f);
			return false;
		}
	}
}