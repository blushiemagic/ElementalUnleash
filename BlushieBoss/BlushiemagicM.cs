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
			Vector2 pos = BlushieBoss.DragonPos;
			pos.Y -= 150f;
			if (pos.X == npc.Center.X)
			{
				pos.X += 2f;
			}
			Vector2 neck = (npc.Center + new Vector2(0f, -BlushieBoss.ArenaSize * 0.4f) + pos) / 2f;
			float slope = (pos.Y - npc.Center.Y) / (pos.X - npc.Center.X);
			Matrix left = new Matrix(pos.X * pos.X, pos.X, 1f, 0f, neck.X * neck.X, neck.X, 1f, 0f, 2f * neck.X, 1f, 0f, 0f, 0f, 0f, 0f, 1f);
			Matrix right = new Matrix(pos.Y, 0f, 0f, 0f, neck.Y, 0f, 0f, 0f, slope, 0f, 0f, 0f, 0f, 0f, 0f, 1f);
			Matrix solution = Matrix.Invert(left) * right;
			float a = solution.M11;
			float b = solution.M21;
			float c = solution.M31;
			Vector2 pos2 = npc.Center;
			left = new Matrix(pos2.X * pos2.X, pos2.X, 1f, 0f, neck.X * neck.X, neck.X, 1f, 0f, 2f * neck.X, 1f, 0f, 0f, 0f, 0f, 0f, 1f);
			right = new Matrix(pos2.Y, 0f, 0f, 0f, neck.Y, 0f, 0f, 0f, slope, 0f, 0f, 0f, 0f, 0f, 0f, 1f);
			solution = Matrix.Invert(left) * right;
			float a2 = solution.M11;
			float b2 = solution.M21;
			float c2 = solution.M31;
			float test = a * neck.X * neck.X + b * neck.X + c;
			float test2 = a2 * neck.X * neck.X + b2 * neck.X + c2;
			Texture2D neckTexture = mod.GetTexture("BlushieBoss/DragonNeck");
			Vector2 zero = Main.screenPosition + new Vector2(neckTexture.Width / 2, neckTexture.Height / 2);
			if (Math.Abs(test - test2) < 16f && Math.Abs(pos.X - npc.Center.X) >= 16f)
			{
				float start = pos.X;
				float end = neck.X;
				if (end < start)
				{
					start = neck.X;
					end = pos.X;
				}
				for (float x = start; x < end; x += 1f)
				{
					float yStart = a * x * x + b * x + c;
					float useEnd = x + 1f;
					if (useEnd > end)
					{
						useEnd = end;
					}
					float yEnd = a * useEnd * useEnd + b * useEnd + c;
					if (yEnd < yStart)
					{
						float temp = yEnd;
						yEnd = yStart;
						yStart = temp;
					}
					for (float y = yStart; y <= yEnd; y += 1f)
					{
						spriteBatch.Draw(neckTexture, new Vector2(x, y) - zero, Color.White);
					}
				}

				start = pos2.X;
				end = neck.X;
				if (end < start)
				{
					start = neck.X;
					end = pos2.X;
				}
				for (float x = start; x < end; x += 1f)
				{
					float yStart = a2 * x * x + b2 * x + c2;
					float useEnd = x + 1f;
					if (useEnd > end)
					{
						useEnd = end;
					}
					float yEnd = a * useEnd * useEnd + b * useEnd + c;
					if (yEnd < yStart)
					{
						float temp = yEnd;
						yEnd = yStart;
						yStart = temp;
					}
					for (float y = yStart; y <= yEnd; y += 1f)
					{
						spriteBatch.Draw(neckTexture, new Vector2(x, y) - zero, Color.White);
					}
				}
			}
			else if (Math.Abs(pos.Y - npc.Center.Y) > 32f)
			{
				Vector2 top = pos;
				Vector2 bottom = npc.Center;
				Vector2 d = top - bottom;
				float length = d.Length();
				d.Normalize();
				for (float k = 0; k <= length; k += 2f)
				{
					spriteBatch.Draw(neckTexture, bottom + k * d - zero, Color.White);
				}
			}
			pos.Y += 150f;
			Texture2D dragon = BlushieBoss.Phase3Attack > 1 && BlushieBoss.Timer < 2060 ? mod.GetTexture("BlushieBoss/DragonHead_Hurt") : mod.GetTexture("BlushieBoss/DragonHead");
			Vector2 draw = pos - Main.screenPosition - new Vector2(dragon.Width / 2, dragon.Height);
			spriteBatch.Draw(dragon, draw, Color.White);
		}

		private void DrawDragonArms(SpriteBatch spriteBatch, bool connect)
		{
			if (connect)
			{
				Texture2D arm = mod.GetTexture("BlushieBoss/DragonArm");
				Vector2 zero = Main.screenPosition + new Vector2(arm.Width / 2, arm.Height / 2);
				Vector2 direction = BlushieBoss.ArmLeftPos - npc.Center;
				float length = direction.Length();
				if (length > 0f)
				{
					direction.Normalize();
					for (float k = 0f; k <= length; k += 2f)
					{
						spriteBatch.Draw(arm, npc.Center + k * direction - zero, Color.White);
					}
				}
				direction = BlushieBoss.ArmRightPos - npc.Center;
				length = direction.Length();
				if (length > 0f)
				{
					direction.Normalize();
					for (float k = 0f; k <= length; k += 2f)
					{
						spriteBatch.Draw(arm, npc.Center + k * direction - zero, Color.White);
					}
				}
			}
			Texture2D claw = mod.GetTexture("BlushieBoss/DragonClaw");
			Vector2 origin = new Vector2(claw.Width / 2, claw.Height / 2);
			spriteBatch.Draw(claw, BlushieBoss.ArmLeftPos - Main.screenPosition, null, Color.White, (BlushieBoss.ArmLeftPos - npc.Center).ToRotation(), origin, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(claw, BlushieBoss.ArmRightPos - Main.screenPosition, null, Color.White, (BlushieBoss.ArmRightPos - npc.Center).ToRotation(), origin, 1f, SpriteEffects.None, 0f);
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