using System;
using ReLogic.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace Bluemagic.Interface
{
	public static class CustomStatsGUI
	{
		private const bool testing = false;

		private static int selected = -1;
		private static MouseState oldMouse;
		private static MouseState curMouse;

		private static Vector2 chaosButtonPos = new Vector2(-1f, -1f);
		private static Vector2 cataclysmButtonPos = new Vector2(-1f, -1f);
		private static UIPanel panel = new UIPanel();
		private static CustomStats curStats = curStats;
		private static CustomStat[] visibleStats = new CustomStat[5];
		private static int scroll = 0;

		public static void Draw()
		{
			oldMouse = curMouse;
			curMouse = Mouse.GetState();
			chaosButtonPos = new Vector2(-1f, -1f);
			cataclysmButtonPos = new Vector2(-1f, -1f);
			for (int k = 0; k < visibleStats.Length; k++)
			{
				visibleStats[k] = null;
			}
			if (!Main.playerInventory)
			{
				selected = -1;
				scroll = 0;
				return;
			}

			DrawButtons();

			if (selected >= 0)
			{
				PositionElements();
				panel.Draw(Main.spriteBatch);
				DrawPanelExtras();
			}
		}

		private static void DrawButtons()
		{
			Mod mod = Bluemagic.Instance;
			CustomStats chaosStats = GetChaosStats();
			CustomStats cataclysmStats = GetCataclysmStats();
			if (testing || chaosStats.Points + chaosStats.UsedPoints > 0)
			{
				chaosButtonPos = new Vector2(Main.screenWidth / 2 + 16f, 70f);
			}
			if (testing || cataclysmStats.Points + cataclysmStats.UsedPoints > 0)
			{
				cataclysmButtonPos = new Vector2(Main.screenWidth / 2 + 80f, 70f);
			}
			if (chaosButtonPos.X >= 0f && chaosButtonPos.Y >= 0f)
			{
				Color color = Color.White;
				if (IsMouseOver(chaosButtonPos, 32, 32))
				{
					color = Color.Silver;
					Main.player[Main.myPlayer].mouseInterface = true;
				}
				Main.spriteBatch.Draw(mod.GetTexture("Interface/ChaosButton"), chaosButtonPos, color);
			}
			if (cataclysmButtonPos.X >= 0f && cataclysmButtonPos.Y >= 0f)
			{
				Color color = Color.White;
				if (IsMouseOver(cataclysmButtonPos, 32, 32))
				{
					color = Color.Silver;
					Main.player[Main.myPlayer].mouseInterface = true;
				}
				Main.spriteBatch.Draw(mod.GetTexture("Interface/CataclysmButton"), cataclysmButtonPos, color);
			}
			if (Clicking())
			{
				int oldSelected = selected;
				if (chaosButtonPos.X >= 0f && chaosButtonPos.Y >= 0f && IsMouseOver(chaosButtonPos, 32, 32))
				{
					selected = 0;
				}
				if (cataclysmButtonPos.X >= 0f && cataclysmButtonPos.Y >= 0f && IsMouseOver(cataclysmButtonPos, 32, 32))
				{
					selected = 1;
				}
				if (oldSelected != selected)
				{
					scroll = 0;
					Main.PlaySound(12, -1, -1, 1);
				}
			}
		}

		private static void PositionElements()
		{
			const float width = 430f;
			panel.Left.Set(Main.screenWidth / 2 - width / 2f, 0f);
			panel.Top.Set(Main.instance.invBottom + 60f, 0f);
			panel.Width.Set(width, 0f);
			panel.Height.Set(320f, 0f);
			panel.Recalculate();
		}

		private static void DrawPanelExtras()
		{
			Mod mod = Bluemagic.Instance;
			if (IsMouseOver(panel))
			{
				Main.player[Main.myPlayer].mouseInterface = true;
			}
			string text;
			if (selected == 1)
			{
				curStats = GetCataclysmStats();
				text = "Cataclysm Boosts";
			}
			else
			{
				curStats = GetChaosStats();
				text = "Chaos Boosts";
			}
			CalculatedStyle dim = panel.GetDimensions();
			DynamicSpriteFont font = Main.fontMouseText;
			Vector2 textSize = font.MeasureString(text);
			Vector2 drawPos = dim.Position() + new Vector2((dim.Width - textSize.X) / 2f, 10f);
			Utils.DrawBorderString(Main.spriteBatch, text, drawPos, Color.White);
			if (selected == 1)
			{
				text = "Expert Only";
				textSize = font.MeasureString(text);
				drawPos = dim.Position() + new Vector2((dim.Width - textSize.X) / 2f, 32f);
				Utils.DrawBorderString(Main.spriteBatch, text, drawPos, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB));
			}
			text = curStats.Points + " Point(s) Available";
			textSize = font.MeasureString(text);
			drawPos = dim.Position() + new Vector2((dim.Width - textSize.X) / 2f, 54f);
			Utils.DrawBorderString(Main.spriteBatch, text, drawPos, Color.White);

			for (int k = 0; k < visibleStats.Length; k++)
			{
				int index = scroll * visibleStats.Length + k;
				if (index >= curStats.Stats.Count)
				{
					break;
				}
				visibleStats[k] = curStats.Stats[index];
				DrawStat(visibleStats[k], GetStatOffset(k));
			}

			Vector2 arrowPos = dim.Position() + new Vector2(dim.Width / 2f, 278f);
			float arrowOffset = 16f;
			Texture2D texture = LeftArrowActive() ? mod.GetTexture("Interface/ArrowLeftActive") : mod.GetTexture("Interface/ArrowLeftInactive");
			Vector2 leftArrowPos = arrowPos + new Vector2(-arrowOffset - texture.Width, 0f);
			Vector2 rightArrowPos = arrowPos + new Vector2(arrowOffset, 0f);
			bool mouseOverLeftArrow = IsMouseOver(leftArrowPos, texture.Width, texture.Height);
			bool mouseOverRightArrow = IsMouseOver(rightArrowPos, texture.Width, texture.Height);
			Main.spriteBatch.Draw(texture, leftArrowPos, mouseOverLeftArrow ? Color.Silver : Color.White);
			texture = RightArrowActive() ? mod.GetTexture("Interface/ArrowRightActive") : mod.GetTexture("Interface/ArrowRightInactive");
			Main.spriteBatch.Draw(texture, rightArrowPos, mouseOverRightArrow ? Color.Silver : Color.White);
			if (LeftArrowActive() && mouseOverLeftArrow && Clicking())
			{
				scroll--;
				Main.PlaySound(12, -1, -1, 1);
			}
			else if (RightArrowActive() && mouseOverRightArrow && Clicking())
			{
				scroll++;
				Main.PlaySound(12, -1, -1, 1);
			}
		}

		private static Vector2 GetStatOffset(int k)
		{
			CalculatedStyle dim = panel.GetDimensions();
			return dim.Position() + new Vector2(10f, 78f + k * 40f);
		}

		private static void DrawStat(CustomStat stat, Vector2 offset)
		{
			Mod mod = Bluemagic.Instance;
			Vector2 textSize = Main.fontMouseText.MeasureString(stat.Name);
			Vector2 drawPos = offset + new Vector2(0f, 16f - textSize.Y / 2f);
			Utils.DrawBorderString(Main.spriteBatch, stat.Name, offset, Color.White);
			for (int k = 0; k < CustomStat.MaxPoints; k++)
			{
				Texture2D texture = stat.Points > k ? mod.GetTexture("Interface/BarFull") : mod.GetTexture("Interface/BarEmpty");
				drawPos = offset + new Vector2(200f + k * 24f, 0f);
				Main.spriteBatch.Draw(texture, drawPos, Color.White);
			}
			Texture2D buttonText = selected == 1 ? mod.GetTexture("Interface/CataclysmButton") : mod.GetTexture("Interface/ChaosButton");
			drawPos = offset + new Vector2(340f, 0f);
			bool mouseOverButton = IsMouseOver(drawPos, buttonText.Width, buttonText.Height);
			Color color = mouseOverButton ? Color.Silver : Color.White;
			Main.spriteBatch.Draw(buttonText, drawPos, color);
			if (curStats.Points > 0 && mouseOverButton && Clicking() && stat.CanUpgrade())
			{
				stat.Points++;
				curStats.Points--;
				Main.PlaySound(12, -1, -1, 1);
			}
			buttonText = stat.Inactive ? mod.GetTexture("Interface/BoxUnchecked") : mod.GetTexture("Interface/BoxChecked");
			drawPos = offset + new Vector2(380f, 0f);
			mouseOverButton = IsMouseOver(drawPos, buttonText.Width, buttonText.Height);
			color = mouseOverButton ? Color.Silver : Color.White;
			Main.spriteBatch.Draw(buttonText, drawPos, color);
			if (mouseOverButton && Clicking())
			{
				stat.Inactive = !stat.Inactive;
			}
		}

		private static bool IsMouseOver(int x, int y, int width, int height)
		{
			Rectangle rect = InterfaceHelper.GetFullRectangle(x, y, width, height);
			return curMouse.X > rect.Left && curMouse.X < rect.Right && curMouse.Y > rect.Top && curMouse.Y < rect.Bottom;
		}

		private static bool IsMouseOver(Vector2 pos, int width, int height)
		{
			return IsMouseOver((int)pos.X, (int)pos.Y, width, height);
		}

		private static bool IsMouseOver(UIElement element)
		{
			CalculatedStyle dim = element.GetDimensions();
			return IsMouseOver((int)dim.X, (int)dim.Y, (int)dim.Width, (int)dim.Height);
		}

		private static bool Clicking()
		{
			return oldMouse.LeftButton == ButtonState.Released && curMouse.LeftButton == ButtonState.Pressed;
		}

		private static bool LeftArrowActive()
		{
			return scroll > 0;
		}

		private static bool RightArrowActive()
		{
			return scroll < (curStats.Stats.Count - 1) / 5;
		}

		private static CustomStats GetChaosStats()
		{
			return Main.player[Main.myPlayer].GetModPlayer<BluemagicPlayer>(Bluemagic.Instance).chaosStats;
		}

		private static CustomStats GetCataclysmStats()
		{
			return Main.player[Main.myPlayer].GetModPlayer<BluemagicPlayer>(Bluemagic.Instance).cataclysmStats;
		}

		public static void DrawMouseOver()
		{
			if (Main.mouseText || selected < 0)
			{
				return;
			}
			Player player = Main.player[Main.myPlayer];
			if (player.ghost)
			{
				return;
			}
			Mod mod = Bluemagic.Instance;
			BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>(mod);

			for (int k = 0; k < 5; k++)
			{
				if (visibleStats[k] == null)
				{
					return;
				}
				Vector2 topLeft = GetStatOffset(k);
				if (IsMouseOver(topLeft, 372, 32))
				{
					Main.instance.MouseText(visibleStats[k].Tooltip);
					Main.mouseText = true;
					return;
				}
				else if (IsMouseOver(topLeft + new Vector2(380f, 0f), 32, 32))
				{
					Main.instance.MouseText(visibleStats[k].Inactive ? "Inactive" : "Active");
					Main.mouseText = true;
					return;
				}
			}
		}
	}
}