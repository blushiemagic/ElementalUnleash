using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Bluemagic.Interface
{
	public static class InterfaceHelper
	{
		private static FieldInfo mHInfo;
		private static FieldInfo _itemIconCacheTimeInfo;

		public static void Initialize()
		{
			mHInfo = typeof(Main).GetField("mH", BindingFlags.NonPublic | BindingFlags.Static);
			_itemIconCacheTimeInfo = typeof(Main).GetField("_itemIconCacheTime", BindingFlags.NonPublic | BindingFlags.Static);
		}

		public static int GetMH()
		{
			return (int)mHInfo.GetValue(null);
		}

		public static void SetMH(int height)
		{
			mHInfo.SetValue(null, height);
		}

		public static void HideItemIconCache()
		{
			_itemIconCacheTimeInfo.SetValue(null, 0);
		}

		public static void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			for (int k = 0; k < layers.Count; k++)
			{
				if (layers[k].Name == "Vanilla: Resource Bars")
				{
					layers.Insert(k + 1, new LegacyGameInterfaceLayer("Bluemagic: Purium Shield Bar", DrawPuriumShieldBar, InterfaceScaleType.UI));
				}
				else if (layers[k].Name == "Vanilla: Inventory")
				{
					layers.Insert(k, new LegacyGameInterfaceLayer("Bluemagic: Accessory Slot Fix", FixAccessorySlots, InterfaceScaleType.None));
					k++;
					layers.Insert(k + 1, new LegacyGameInterfaceLayer("Bluemagic: Custom Stats", DrawCustomStats, InterfaceScaleType.UI));
				}
				else if (layers[k].Name == "Vanilla: Mouse Over")
				{
					layers.Insert(k, new LegacyGameInterfaceLayer("Bluemagic: Mouse Over", DrawMouseOver, InterfaceScaleType.UI));
					k++;
				}
			}
		}

		private static bool DrawPuriumShieldBar()
		{
			int anchorX = Main.screenWidth / 2;
			Player player = Main.player[Main.myPlayer];
			if (player.ghost)
			{
				return true;
			}
			Mod mod = Bluemagic.Instance;
			BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
			if (modPlayer.puriumShieldChargeMax <= 0f)
			{
				return true;
			}

			const int barSize = 128;
			const int padding = 4;
			const int chargeSize = barSize - 2 * padding;
			const int chargeHeight = 20;
			DynamicSpriteFont font = Main.fontMouseText;
			float puriumShieldCharge = Math.Min(modPlayer.puriumShieldCharge, modPlayer.puriumShieldChargeMax);
			string chargeText = (int)puriumShieldCharge + "/" + (int)modPlayer.puriumShieldChargeMax;
			string maxText = "Purity Shield Charge: " + (int)modPlayer.puriumShieldChargeMax + "/" + (int)modPlayer.puriumShieldChargeMax;
			Vector2 maxTextSize = font.MeasureString(maxText);
			Color textColor = new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor);
			Main.spriteBatch.DrawString(font, "Purity Shield Charge:", new Vector2(anchorX + barSize / 2 - maxTextSize.X / 2f, 6f), textColor, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.DrawString(font, chargeText, new Vector2(anchorX + barSize / 2 + maxTextSize.X / 2f, 6f), textColor, 0f, new Vector2(font.MeasureString(chargeText).X, 0f), 1f, SpriteEffects.None, 0f);

			float fill = puriumShieldCharge / modPlayer.puriumShieldChargeMax;
			Main.spriteBatch.Draw(mod.GetTexture("PuriumShieldBar"), new Vector2(anchorX, 32f), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(mod.GetTexture("PuriumShieldCharge"), new Vector2(anchorX + padding, 32f + padding), new Rectangle(0, 0, (int)(fill * chargeSize), chargeHeight), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			return true;
		}

		private static bool FixAccessorySlots()
		{
			Player player = Main.player[Main.myPlayer];
			if (Main.mapEnabled && !Main.mapFullscreen && Main.mapStyle == 1 && player.GetModPlayer<BluemagicPlayer>().extraAccessory2)
			{
				SetMH(GetMH() - 50);
			}
			return true;
		}

		private static bool DrawCustomStats()
		{
			CustomStatsGUI.Draw();
			return true;
		}

		private static bool DrawMouseOver()
		{
			DrawPuriumShieldMouseOver();
			CustomStatsGUI.DrawMouseOver();
			return true;
		}

		private static void DrawPuriumShieldMouseOver()
		{
			if (Main.mouseText)
			{
				return;
			}
			Player player = Main.player[Main.myPlayer];
			if (player.ghost)
			{
				return;
			}
			Mod mod = Bluemagic.Instance;
			BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
			if (modPlayer.puriumShieldChargeMax <= 0f)
			{
				return;
			}

			int screenAnchorX = Main.screenWidth / 2;
			const int barSize = 128;
			const int barHeight = 28;
			Rectangle rect = new Rectangle(screenAnchorX, 32, (int)(barSize * Main.UIScale), (int)(barHeight * Main.UIScale));
			if (Main.mouseX > rect.Left && Main.mouseX < rect.Right && Main.mouseY > rect.Top && Main.mouseY < rect.Bottom)
			{
				Main.player[Main.myPlayer].showItemIcon = false;
				float enduranceCap = (int)(20 * modPlayer.puriumShieldEnduranceMult);
				string text = "Damaging enemies powers a purity shield around you";
				text += "\nShield also regenerates over time";
				text += "\nReduces damage by up to " + enduranceCap + "% depending on charge";
				text += "\nTaking damage consumes shield depending on damage";
				text += "\nShield can consume 50 charge to protect you from debuffs";
				text += "\nShield can consume 1,000 charge to protect you from fatal damage";
				Main.instance.MouseText(text);
				Main.mouseText = true;
			}
		}

		public static Rectangle GetFullRectangle(UIElement element)
		{
			CalculatedStyle dim = element.GetDimensions();
			return GetFullRectangle((int)dim.X, (int)dim.Y, (int)dim.Width, (int)dim.Height);
		}

		public static Rectangle GetFullRectangle(int x, int y, int width, int height)
		{
			Vector2 vector = new Vector2(x, y);
			Vector2 position = new Vector2(width, height) + vector;
			vector = Vector2.Transform(vector, Main.UIScaleMatrix);
			position = Vector2.Transform(position, Main.UIScaleMatrix);
			Rectangle result = new Rectangle((int)vector.X, (int)vector.Y, (int)(position.X - vector.X), (int)(position.Y - vector.Y));
			int sWidth = Main.spriteBatch.GraphicsDevice.Viewport.Width;
			int sHeight = Main.spriteBatch.GraphicsDevice.Viewport.Height;
			result.X = Utils.Clamp<int>(result.X, 0, sWidth);
			result.Y = Utils.Clamp<int>(result.Y, 0, sHeight);
			result.Width = Utils.Clamp<int>(result.Width, 0, sWidth - result.X);
			result.Height = Utils.Clamp<int>(result.Height, 0, sHeight - result.Y);
			return result;
		}
	}
}