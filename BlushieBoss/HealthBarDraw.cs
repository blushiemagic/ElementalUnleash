using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.UI.Chat;

namespace Bluemagic.BlushieBoss
{
    public static class HealthBarDraw
    {
        public static void DrawHealthBarDefault(SpriteBatch spriteBatch, float Alpha)
        {
            int width = Main.screenWidth / 2;
            int x = Main.screenWidth / 4;
            int y = Main.screenHeight - Bluemagic.HealthBars.GetTexture("UI/HealthBarMiddle").Height - 32;
            NPC npc = Main.npc[BlushieBoss.index[4]];

            DrawHealthBar(spriteBatch, x, y, width, Alpha, npc.life, npc.lifeMax, npc);
        }

        /// <summary>
        /// Draw the health bar. Don't forget to override the methods if you want to change settings.
        /// </summary>
        /// <param name="spriteBatch">Pass the spritebatch pls</param>
        /// <param name="XLeft">The left of the bar, NOT the bar side frame.</param>
        /// <param name="yTop">Top of the bar frame (including sides).</param>
        /// <param name="BarLength">Length of the middle section of the bar.</param>
        /// <param name="Alpha">Alpha between 1f and 0f</param>
        /// <param name="life">npc.life</param>
        /// <param name="lifeMax">npc.lifeMax</param>
        /// <param name="npc">npc itself for handling certain internal methods</param>
        public static void DrawHealthBar(SpriteBatch spriteBatch, int XLeft, int yTop, int BarLength, float Alpha, int life, int lifeMax, NPC npc)
        {
            string displayName = "blushiemagic (M/J)";;

            // Get variables
            Color frameColour = Color.White;
            Color barColour = GetHealthColor(BlushieBoss.Phase3Attack);
            Color backColour = GetHealthColor(BlushieBoss.Phase3Attack + 1);
            frameColour *= Alpha;
            barColour *= Alpha;
            backColour *= Alpha;
            Texture2D fill, barL, barM, barR;
            fill = Bluemagic.HealthBars.GetTexture("UI/HealthBarFill");
            barL = Bluemagic.HealthBars.GetTexture("UI/HealthBarStart" + (Main.expertMode ? "_Exp" : ""));
            barM = Bluemagic.HealthBars.GetTexture("UI/HealthBarMiddle" + (Main.expertMode ? "_Exp" : ""));
            barR = Bluemagic.HealthBars.GetTexture("UI/HealthBarEnd" + (Main.expertMode ? "_Exp" : ""));

            int midXOffset = -30;
            int midYOffset = 10;

            // The very far left where the side frames start
            Vector2 FrameTopLeft = new Vector2(XLeft - barL.Width, yTop);

            // Draw Back
            drawHealthBarFill(spriteBatch, lifeMax, lifeMax, backColour, fill, BarLength, XLeft, midXOffset, midYOffset, yTop);

            // Draw Fill
            int realLength = drawHealthBarFill(spriteBatch, life, lifeMax, barColour, fill, BarLength, XLeft, midXOffset, midYOffset, yTop);

            // Draw Frame
            drawHealthBarFrame(spriteBatch, frameColour, barL, barM, barR, BarLength, XLeft, midYOffset, yTop, FrameTopLeft);

            string text = string.Concat(displayName, ": ", life, "/", lifeMax);
            DynamicSpriteFontExtensionMethods.DrawString(
                spriteBatch,
                Main.fontMouseText,
                text,
                new Vector2(XLeft + BarLength / 2, yTop + midYOffset + barM.Height / 2),
                frameColour, 0f,
                ChatManager.GetStringSize(Main.fontMouseText, text, Vector2.One, BarLength) / 2,
                1.1f, SpriteEffects.None, 0f);
        }

        internal static Color GetHealthColor(int phase)
        {
            switch (phase)
            {
            case 1:
                return new Color(255, 0, 255);
            case 2:
                return new Color(127, 0, 255);
            case 3:
                return new Color(0, 0, 255);
            case 4:
                return new Color(0, 255, 255);
            case 5:
                return new Color(0, 255, 0);
            case 6:
                return new Color(255, 255, 0);
            case 7:
                return new Color(255, 127, 0);
            case 8:
                return new Color(255, 0, 0);
            default:
                return new Color(0, 0, 0, 0);
            }
        }

        private static int drawHealthBarFill(SpriteBatch spriteBatch, int life, int lifeMax, Color barColour, Texture2D fill, int barLength, int XLeft, int fillXOffset, int fillYOffset, int yTop)
        {
            int decoOffset = 10;
            int decoWidth = fill.Width - decoOffset;
            // real length is the screen size for bars, plus any extra inset by side frame graphics
            // friendly reminder fillXOffset is usually negative
            int realLength = 1 + (int)((barLength - fillXOffset * 2 - 1) * ((float)life / lifeMax));
            if (life <= 0) realLength = 0;
            if (realLength > 0)
            {
                // Calculate the scale factor to stretch the featureless side of the bar
                int fillBarLength = realLength - decoWidth;
                if (fillBarLength > 0)
                {
                    float fillXStretch = 1f / (decoOffset - 1) * (realLength - decoWidth);

                    // Draw stretched bar
                    spriteBatch.Draw(
                        fill,
                        new Vector2(XLeft + fillXOffset,
                            yTop + fillYOffset),
                        new Rectangle(0, 0, decoOffset - 1, fill.Height),
                        barColour,
                        0f,
                        Vector2.Zero,
                        new Vector2(fillXStretch, 1f),
                        SpriteEffects.None,
                        0f);
                }

                if (fillBarLength > 0) fillBarLength = 0;
                try
                {
                    // Draw the decoartion side of the bar
                    spriteBatch.Draw(
                        fill,
                        new Vector2(XLeft + fillXOffset + realLength - decoWidth - fillBarLength,
                            yTop + fillYOffset),
                        new Rectangle(decoOffset - fillBarLength, 0, decoWidth + fillBarLength, fill.Height),
                        barColour,
                        0f,
                        Vector2.Zero,
                        1f,
                        SpriteEffects.None,
                        0f);
                }
                catch { }
            }

            return realLength;
        }

        private static void drawHealthBarFrame(SpriteBatch spriteBatch, Color frameColour, Texture2D barL, Texture2D barM, Texture2D barR, int barLength, int XLeft, int midYOffset, int yTop, Vector2 FrameTopLeft)
        {
            spriteBatch.Draw(
                barM,
                new Vector2(XLeft, yTop + midYOffset),
                null,
                frameColour,
                0f,
                Vector2.Zero,
                new Vector2(1f / barM.Width * barLength, 1f),
                SpriteEffects.None,
                0f);
            //Draw side frames
            spriteBatch.Draw(
                barL,
                FrameTopLeft,
                frameColour
                );
            spriteBatch.Draw(
                barR,
                new Vector2(XLeft + barLength, yTop),
                frameColour
                );
        }
    }
}