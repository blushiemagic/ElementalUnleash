using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace Bluemagic.BlushieBoss
{
    public class BlushieSky : CustomSky
    {
        private bool isActive = false;

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= 0 && minDepth < 0)
            {
                if (BlushieBoss.Phase >= 3)
                {
                    float alpha = 1f;
                    if (BlushieBoss.Phase == 3 && BlushieBoss.Timer < 300)
                    {
                        alpha = BlushieBoss.Timer / 300f;
                        Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black);
                    }
                    Color color1 = new Color(0, 127, 255);
                    Color color2 = new Color(200, 0, 0);
                    Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight / 3), color1 * alpha);
                    Main.spriteBatch.Draw(Terraria.ModLoader.ModContent.Request<Texture2D>("Bluemagic/BlushieBoss/SkyGradient").Value, new Rectangle(0, Main.screenHeight / 3, Main.screenWidth, Main.screenHeight / 3), Color.White * alpha);
                    Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, Main.screenHeight * 2 / 3, Main.screenWidth, Main.screenHeight / 3), color2 * alpha);
                }
                else
                {
                    Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black);
                }
            }
        }

        public override float GetCloudAlpha()
        {
            return 0f;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            isActive = true;
        }

        public override void Deactivate(params object[] args)
        {
            isActive = false;
        }

        public override void Reset()
        {
            isActive = false;
        }

        public override bool IsActive()
        {
            return isActive;
        }
    }
}