using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Bluemagic
{
	public static class InterfaceHelper
	{
		public static void ModifyInterfaceLayers(List<MethodSequenceListItem> layers)
		{
			for (int k = 0; k < layers.Count; k++)
			{
				if (layers[k].Name == "Vanilla: Resource Bars")
				{
					layers.Insert(k + 1, new MethodSequenceListItem("Bluemagic: Purium Shield Bar", DrawPuriumShieldBar, layers[k]));
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
			BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>(mod);
			if (!modPlayer.puriumShield)
			{
				return true;
			}

			const int barSize = 128;
			const int padding = 4;
			const int chargeSize = barSize - 2 * padding;
			const int chargeHeight = 20;
			SpriteFont font = Main.fontMouseText;
			string chargeText = (int)modPlayer.puriumShieldCharge + "/" + (int)BluemagicPlayer.puriumShieldChargeMax;
			string maxText = "Purity Shield Charge: " + (int)BluemagicPlayer.puriumShieldChargeMax + "/" + (int)BluemagicPlayer.puriumShieldChargeMax;
			Vector2 maxTextSize = font.MeasureString(maxText);
			Color textColor = new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor);
			Main.spriteBatch.DrawString(font, "Purity Shield Charge:", new Vector2(anchorX + barSize / 2 - maxTextSize.X / 2f, 6f), textColor, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.DrawString(font, chargeText, new Vector2(anchorX + barSize / 2 + maxTextSize.X / 2f, 6f), textColor, 0f, new Vector2(font.MeasureString(chargeText).X, 0f), 1f, SpriteEffects.None, 0f);

			float fill = modPlayer.puriumShieldCharge / BluemagicPlayer.puriumShieldChargeMax;
			Main.spriteBatch.Draw(mod.GetTexture("PuriumShieldBar"), new Vector2(anchorX, 32f), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(mod.GetTexture("PuriumShieldCharge"), new Vector2(anchorX + padding, 32f + padding), new Rectangle(0, 0, (int)(fill * chargeSize), chargeHeight), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			return true;
		}
	}
}