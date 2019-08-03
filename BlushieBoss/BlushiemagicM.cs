using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.BlushieBoss
{
	public class BlushiemagicM : BlushiemagicBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("blushiemagic (M)");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.takenDamageMultiplier = 5f;
			this.music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Phyrnna - Return of the Snow Queen");
		}

		public override void AI()
		{
		}

		public override bool CheckDead()
		{
			npc.life = 1;
			npc.active = true;
			if (BlushieBoss.SpawnedStars && BlushieBoss.crystalStars.Count == 0)
			{
				npc.life = npc.lifeMax;
				BlushieBoss.bullets.Clear();
				BlushieBoss.Phase3Attack++;
				BlushieBoss.SpawnedStars = false;
				BlushieBoss.Timer = 2000;
			}
			if (BlushieBoss.index[5] >= 0)
			{
				Main.npc[BlushieBoss.index[5]].life = npc.life;
			}
			return false;
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return Bluemagic.HealthBars != null ? (bool?)false : (bool?)null;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (BlushieBoss.Phase == 2 && BlushieBoss.Timer >= 480)
			{
				Texture2D texture = mod.GetTexture("BlushieBoss/GreenOrb");
				Vector2 draw = npc.Center - Main.screenPosition - new Vector2(texture.Width / 2, texture.Height / 2);
				float offset = BlushieBoss.Timer - 480;
				if (offset > 60f)
				{
					offset = 60f;
				}
				spriteBatch.Draw(texture, draw + new Vector2(-offset, 0f), Color.White);
				spriteBatch.Draw(texture, draw + new Vector2(offset, 0f), Color.White);
			}
			else if (BlushieBoss.Phase == 3 && BlushieBoss.Timer > 60 && BlushieBoss.Timer < 360)
			{
				Texture2D texture = mod.GetTexture("BlushieBoss/GreenOrb");
				Vector2 draw = npc.Center - Main.screenPosition - new Vector2(texture.Width / 2, texture.Height / 2);
				float offset = 60f + (BlushieBoss.ArenaSize * 0.6f - 60f) * (BlushieBoss.Timer - 60f) / 300f;
				spriteBatch.Draw(texture, draw + new Vector2(-offset, 0f), Color.White);
				spriteBatch.Draw(texture, draw + new Vector2(offset, 0f), Color.White);
			}
			else if (BlushieBoss.Phase == 3 && BlushieBoss.Timer >= 360 && BlushieBoss.Timer < 840)
			{
				Texture2D texture = mod.GetTexture("BlushieBoss/GreenOrb");
				Vector2 draw = npc.Center - Main.screenPosition;
				float offset = BlushieBoss.ArenaSize * 0.6f;
				Vector2 origin;
				if (BlushieBoss.Timer >= 600)
				{
					Texture2D lines = mod.GetTexture("BlushieBoss/PowerLines");
					origin = new Vector2(lines.Width / 2, lines.Height / 2);
					float rotate = BlushieBoss.Timer / 60f * MathHelper.TwoPi;
					spriteBatch.Draw(lines, draw + new Vector2(-offset, 0f), null, Color.White * 0.3f, rotate, origin, 6f, SpriteEffects.None, 0f);
					spriteBatch.Draw(lines, draw + new Vector2(offset, 0f), null, Color.White * 0.3f, rotate, origin, 6f, SpriteEffects.None, 0f);
					if (BlushieBoss.Timer >= 720)
					{
						rotate += MathHelper.Pi / 8f;
						spriteBatch.Draw(lines, draw + new Vector2(-offset, 0f), null, Color.White * 0.1f, rotate, origin, 24f, SpriteEffects.None, 0f);
					spriteBatch.Draw(lines, draw + new Vector2(offset, 0f), null, Color.White * 0.1f, rotate, origin, 24f, SpriteEffects.None, 0f);
					}
				}
				origin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, draw + new Vector2(-offset, 0f), null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, draw + new Vector2(offset, 0f), null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
				float scale = 2f - (BlushieBoss.Timer % 30f) / 15f;
				Texture2D circle = mod.GetTexture("BlushieBoss/PowerCircle");
				origin = new Vector2(circle.Width / 2, circle.Height / 2);
				spriteBatch.Draw(circle, draw + new Vector2(-offset, 0f), null, Color.White * 0.3f, 0f, origin, scale, SpriteEffects.None, 0f);
				spriteBatch.Draw(circle, draw + new Vector2(offset, 0f), null, Color.White * 0.3f, 0f, origin, scale, SpriteEffects.None, 0f);
			}
			else if (BlushieBoss.Phase == 3 && BlushieBoss.Timer >= 840 && BlushieBoss.Timer < 900)
			{
				float width = BlushieBoss.Timer - 840f;
				spriteBatch.Draw(mod.GetTexture("Pixel"), BlushieBoss.Origin - Main.screenPosition, null, new Color(0, 255, 0), 0f, new Vector2(0.5f, 0.5f), new Vector2(2f * BlushieBoss.ArenaSize, 2f * width), SpriteEffects.None, 0f);
				DrawDragonArms(spriteBatch, false);
			}
			else if (BlushieBoss.Phase == 3 && BlushieBoss.Timer >= 900 && BlushieBoss.Timer < 960)
			{
				DrawDragonArms(spriteBatch, true);
				float width = 960f - BlushieBoss.Timer;
				spriteBatch.Draw(mod.GetTexture("Pixel"), BlushieBoss.Origin - Main.screenPosition, null, new Color(0, 255, 0), 0f, new Vector2(0.5f, 0.5f), new Vector2(2f * BlushieBoss.ArenaSize, 2f * width), SpriteEffects.None, 0f);
				DrawDragonArms(spriteBatch, false);
			}
			else if (BlushieBoss.Phase == 3 && BlushieBoss.Timer >= 960 && BlushieBoss.Timer < 1080)
			{
				DrawDragonArms(spriteBatch, true);
				float width = BlushieBoss.Timer - 960f;
				spriteBatch.Draw(mod.GetTexture("Pixel"), BlushieBoss.Origin - Main.screenPosition, null, new Color(0, 255, 0), 0f, new Vector2(0.5f, 0.5f), new Vector2(2f * width, 2f * BlushieBoss.ArenaSize), SpriteEffects.None, 0f);
			}
			else if (BlushieBoss.Phase == 3 && BlushieBoss.Timer >= 1080 && BlushieBoss.Timer < 1200)
			{
				Texture2D lines = mod.GetTexture("BlushieBoss/PowerLines");
				Vector2 origin = new Vector2(lines.Width / 2, lines.Height / 2);
				float rotate = BlushieBoss.Timer / 60f * MathHelper.TwoPi;
				spriteBatch.Draw(lines, BlushieBoss.DragonPos - Main.screenPosition + new Vector2(0f, -100f), null, Color.White * 0.16f, rotate, origin, 24f, SpriteEffects.None, 0f);
				DrawDragonArms(spriteBatch, true);
				DrawDragonHead(spriteBatch);
				float width = 1200f - BlushieBoss.Timer;
				spriteBatch.Draw(mod.GetTexture("Pixel"), BlushieBoss.Origin - Main.screenPosition, null, new Color(0, 255, 0), 0f, new Vector2(0.5f, 0.5f), new Vector2(2f * width, 2f * BlushieBoss.ArenaSize), SpriteEffects.None, 0f);
			}
			else if (BlushieBoss.Phase == 3 && BlushieBoss.Timer >= 1200)
			{
				DrawDragonArms(spriteBatch, true);
				DrawDragonHead(spriteBatch);
			}
			return true;
		}

		private void DrawDragonHead(SpriteBatch spriteBatch)
		{
			DrawDragonHead(spriteBatch, npc.Center, BlushieBoss.DragonPos);
		}

		internal static void DrawDragonHead(SpriteBatch spriteBatch, Vector2 origin, Vector2 dragonPos)
		{
			Vector2 pos = dragonPos;
			pos.Y -= 150f;
			Texture2D neckTexture = Bluemagic.Instance.GetTexture("BlushieBoss/DragonNeck");
			Vector2 zero = Main.screenPosition + new Vector2(neckTexture.Width / 2, neckTexture.Height / 2);
			if (Math.Abs(pos.X - origin.X) <= 4f)
			{
				Vector2 direction = pos - origin;
				float length = direction.Length();
				if (length > 0f)
				{
					direction.Normalize();
					for (float k = 0f; k <= length; k += 2f)
					{
						spriteBatch.Draw(neckTexture, origin + k * direction - zero, Color.White);
					}
				}
			}
			else
			{
				float angle = (float)Math.Atan2(pos.Y - origin.Y, pos.X - origin.X);
				Vector2 midpoint = (pos + origin) / 2f;
				float targetAngle = (angle - MathHelper.PiOver2) / 2f;
				if (angle > 0f)
				{
					targetAngle += angle;
				}
				float normal = angle;
				if (angle < targetAngle)
				{
					normal += MathHelper.PiOver2;
				}
				else
				{
					normal -= MathHelper.PiOver2;
				}
				float translatedNormal = normal - targetAngle;
				Vector2 translatedPos = (midpoint - origin).RotatedBy(-targetAngle);
				float distanceTo = translatedPos.X - translatedPos.Y / (float)Math.Tan(translatedNormal);
				Vector2 neck = origin + distanceTo * new Vector2((float)Math.Cos(targetAngle), (float)Math.Sin(targetAngle));

				pos -= zero;
				neck -= zero;
				origin -= zero;
				Matrix left = new Matrix(pos.X * pos.X, pos.X, 1f, 0f, neck.X * neck.X, neck.X, 1f, 0f, origin.X * origin.X, origin.X, 1f, 0f, 0f, 0f, 0f, 1f);
				Matrix right = new Matrix(pos.Y, 0f, 0f, 0f, neck.Y, 0f, 0f, 0f, origin.Y, 0f, 0f, 0f, 0f, 0f, 0f, 1f);
				Matrix solution = Matrix.Invert(left) * right;
				float a = solution.M11;
				float b = solution.M21;
				float c = solution.M31;

				float start = pos.X;
				float end = origin.X;
				if (end < start)
				{
					start = origin.X;
					end = pos.X;
				}
				for (float x = start; x < end; x += 1f)
				{
					float yStart = (float)(a * x * x + b * x + c);
					float useEnd = x + 1f;
					if (useEnd > end)
					{
						useEnd = end;
					}
					float yEnd = (float)(a * useEnd * useEnd + b * useEnd + c);
					if (yEnd < yStart)
					{
						float temp = yEnd;
						yEnd = yStart;
						yStart = temp;
					}
					for (float y = yStart; y <= yEnd; y += 1f)
					{
						spriteBatch.Draw(neckTexture, new Vector2(x, y), Color.White);
					}
				}
				/* debug
				spriteBatch.Draw(neckTexture, pos - zero, Color.White);
				spriteBatch.Draw(neckTexture, neck - zero, Color.White);
				spriteBatch.Draw(neckTexture, origin - zero, Color.White);
				spriteBatch.Draw(Bluemagic.Instance.GetTexture("Pixel"), origin - Main.screenPosition, null, Color.White, targetAngle, new Vector2(1f, 0.5f), new Vector2(Main.screenWidth, 8f), SpriteEffects.None, 0f);
				spriteBatch.Draw(Bluemagic.Instance.GetTexture("Pixel"), midpoint - Main.screenPosition, null, Color.White, normal, new Vector2(1f, 0.5f), new Vector2(Main.screenWidth, 8f), SpriteEffects.None, 0f);
				spriteBatch.Draw(Bluemagic.Instance.GetTexture("Pixel"), origin - Main.screenPosition, null, Color.White, targetAngle, new Vector2(0f, 0.5f), new Vector2(Main.screenWidth, 16f), SpriteEffects.None, 0f);
				spriteBatch.Draw(Bluemagic.Instance.GetTexture("Pixel"), midpoint - Main.screenPosition, null, Color.White, normal, new Vector2(0f, 0.5f), new Vector2(Main.screenWidth, 16f), SpriteEffects.None, 0f);
				*/
				pos += zero;
			}

			pos.Y += 150f;
			Texture2D dragon = BlushieBoss.Phase3Attack > 1 && BlushieBoss.Timer < 2060 ? Bluemagic.Instance.GetTexture("BlushieBoss/DragonHead_Hurt") : Bluemagic.Instance.GetTexture("BlushieBoss/DragonHead");
			Vector2 draw = pos - Main.screenPosition - new Vector2(dragon.Width / 2, dragon.Height);
			spriteBatch.Draw(dragon, draw, Color.White);
		}

		private void DrawDragonArms(SpriteBatch spriteBatch, bool connect)
		{
			DrawDragonArms(spriteBatch, npc.Center, BlushieBoss.ArmLeftPos, BlushieBoss.ArmRightPos, connect);
		}

		internal static void DrawDragonArms(SpriteBatch spriteBatch, Vector2 origin, Vector2 armLeftPos, Vector2 armRightPos, bool connect)
		{
			if (connect)
			{
				Texture2D arm = Bluemagic.Instance.GetTexture("BlushieBoss/DragonArm");
				Vector2 zero = Main.screenPosition + new Vector2(arm.Width / 2, arm.Height / 2);
				Vector2 direction = armLeftPos - origin;
				float length = direction.Length();
				if (length > 0f)
				{
					direction.Normalize();
					for (float k = 0f; k <= length; k += 2f)
					{
						spriteBatch.Draw(arm, origin + k * direction - zero, Color.White);
					}
				}
				direction = armRightPos - origin;
				length = direction.Length();
				if (length > 0f)
				{
					direction.Normalize();
					for (float k = 0f; k <= length; k += 2f)
					{
						spriteBatch.Draw(arm, origin + k * direction - zero, Color.White);
					}
				}
			}
			Texture2D claw = Bluemagic.Instance.GetTexture("BlushieBoss/DragonClaw");
			Vector2 textOrigin = new Vector2(claw.Width / 2, claw.Height / 2);
			spriteBatch.Draw(claw, armLeftPos - Main.screenPosition, null, Color.White, (armLeftPos - origin).ToRotation(), textOrigin, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(claw, armRightPos - Main.screenPosition, null, Color.White, (armRightPos - origin).ToRotation(), textOrigin, 1f, SpriteEffects.None, 0f);
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (BlushieBoss.Phase == 2 && BlushieBoss.Timer < 480)
			{
				Texture2D texture = mod.GetTexture("ChaosSpirit/DissonanceOrb");
				Vector2 drawPos = npc.Center - Main.screenPosition;
				Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
				float rotation = 0.05f * BlushieBoss.Timer;
				spriteBatch.Draw(texture, drawPos, null, Color.White, rotation, origin, 0.5f, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, drawPos, null, Color.White, -rotation, origin, 0.5f, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, drawPos, null, Color.White, -rotation, origin, 0.25f, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, drawPos, null, Color.White, rotation, origin, 0.25f, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, drawPos, null, Color.White, -rotation, origin, 1f, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, drawPos, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0f);
			}
			else if (BlushieBoss.Phase != 3 || BlushieBoss.Phase3Attack == 0)
			{
				Texture2D shield = mod.GetTexture("Mounts/PurityShield");
				spriteBatch.Draw(shield, npc.Center - Main.screenPosition - new Vector2(shield.Width / 2, shield.Height / 2), null, Color.White * 0.5f);
			}
			BlushieBoss.DrawBullets(spriteBatch);
		}

		public override bool UseSpecialDamage()
		{
			return false;
		}
	}
}