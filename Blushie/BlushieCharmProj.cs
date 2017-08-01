using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Blushie
{
	public class BlushieCharmProj : ModProjectile
	{
		private const int xRange = 600;
		private const int yRange = 320;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charm of Blushie");
		}

		public override void SetDefaults()
		{
			projectile.width = 2;
			projectile.height = 2;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.magic = true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if (Main.myPlayer == projectile.owner)
			{
				if (!player.channel || player.noItems || player.CCed)
				{
					projectile.Kill();
				}
			}
			projectile.Center = player.MountedCenter;
			projectile.timeLeft = 2;
			player.itemTime = 2;
			player.itemAnimation = 2;

			projectile.ai[0] += 1f;
			projectile.damage = player.statLife;
			if (projectile.ai[0] >= 240f && Main.myPlayer == projectile.owner)
			{
				int useMana = projectile.damage / 200;
				if (player.statMana < useMana && player.manaFlower)
				{
					player.QuickMana();
				}
				if (player.statMana >= useMana)
				{
					player.statMana -= useMana;
				}
				else
				{
					projectile.Kill();
				}
			}

			if (projectile.localAI[1] == 0f)
			{
				projectile.localAI[1] = 39f;
				projectile.localAI[0] = Main.rand.Next(xRange);
			}
			projectile.localAI[0] = Next(projectile.localAI[0]);
		}

		private float Next(float seed)
		{
			return (seed * projectile.localAI[1] + 97f) % (4 * xRange * yRange);
		}

		public override bool CanDamage()
		{
			return projectile.ai[0] > 120f;
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox.X -= xRange;
			hitbox.Width += 2 * xRange;
			hitbox.Y -= yRange;
			hitbox.Height += 2 * yRange;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			damage += target.defense / 2;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.penetrate++;
			target.AddBuff(BuffID.OnFire, 300);
			target.AddBuff(BuffID.Frostburn, 300);
			target.AddBuff(BuffID.Lovestruck, 300);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawCenter = projectile.Center - Main.screenPosition;
			Texture2D texture = mod.GetTexture("Blushie/BlushieCharmHeart");
			float scale = projectile.ai[0] / 60f;
			if (scale > 1f)
			{
				scale = 1f;
			}
			spriteBatch.Draw(texture, drawCenter, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(1.2f * scale, 1f), SpriteEffects.None, 0f);
			texture = mod.GetTexture("Blushie/BlushieCharmTop");
			Vector2 drawTop = drawCenter + new Vector2(0f, -yRange);
			spriteBatch.Draw(texture, drawTop, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(scale, 1f), SpriteEffects.None, 0f);

			float distance = (projectile.ai[0] - 60f) / 60f;
			if (distance < 0f)
			{
				distance = 0f;
			}
			if (distance > 1f)
			{
				distance = 1f;
			}
			Color color = new Color(160, 160, 160, 255);
			DrawLine(spriteBatch, drawTop + new Vector2(-150f, 0f), 160f * distance);
			DrawLine(spriteBatch, drawTop + new Vector2(-300f, 0f), 480f * distance);
			DrawLine(spriteBatch, drawTop + new Vector2(-450f, 0f), 320f * distance);
			DrawLine(spriteBatch, drawTop + new Vector2(150f, 0f), 160f * distance);
			DrawLine(spriteBatch, drawTop + new Vector2(300f, 0f), 480f * distance);
			DrawLine(spriteBatch, drawTop + new Vector2(450f, 0f), 320f * distance);
			texture = mod.GetTexture("Blushie/BlushieCharmCrystalBlue");
			spriteBatch.Draw(texture, drawTop + new Vector2(-150f, 160f * distance), null, Color.White * scale, 0f, new Vector2(texture.Width / 2, 0), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, drawTop + new Vector2(-300f, 480f * distance), null, Color.White * scale, 0f, new Vector2(texture.Width / 2, 0), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, drawTop + new Vector2(-450f, 32f + 320f * distance), null, Color.White * scale, 0f, new Vector2(texture.Width / 2, 0), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, drawTop + new Vector2(-450f, 320f * distance), null, Color.White * scale, 0f, new Vector2(texture.Width / 2, 0), 1f, SpriteEffects.None, 0f);
			texture = mod.GetTexture("Blushie/BlushieCharmCrystalPink");
			spriteBatch.Draw(texture, drawTop + new Vector2(150f, 160f * distance), null, Color.White * scale, 0f, new Vector2(texture.Width / 2, 0), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, drawTop + new Vector2(300f, 480f * distance), null, Color.White * scale, 0f, new Vector2(texture.Width / 2, 0), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, drawTop + new Vector2(450f, 32f + 320f * distance), null, Color.White * scale, 0f, new Vector2(texture.Width / 2, 0), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, drawTop + new Vector2(450f, 320f * distance), null, Color.White * scale, 0f, new Vector2(texture.Width / 2, 0), 1f, SpriteEffects.None, 0f);

			float seed = projectile.localAI[0];
			texture = mod.GetTexture("Blushie/BlushieCharmSparkle");
			Vector2 topLeft = drawTop + new Vector2(-xRange, 0f);
			for (int k = (int)projectile.ai[0] - 120; k < (int)projectile.ai[0]; k++)
			{
				if (k > 120)
				{
					spriteBatch.Draw(texture, topLeft + new Vector2(seed % (2 * xRange), seed % (2 * yRange)), null, Color.White, 0.1f * (projectile.ai[0] - k), new Vector2(texture.Width / 2, texture.Height / 2), 2f * (projectile.ai[0] - k) / 120f, SpriteEffects.None, 0f);
				}
				seed = Next(seed);
			}
			
			return false;
		}

		private void DrawLine(SpriteBatch spriteBatch, Vector2 start, float height)
		{
			spriteBatch.Draw(Main.blackTileTexture, start - new Vector2(2f, 0f), null, new Color(160, 160, 160), 0f, Vector2.Zero, new Vector2(0.25f, height / 16f), SpriteEffects.None, 0f);
		}
	}
}