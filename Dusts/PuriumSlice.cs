using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Dusts
{
    public class PuriumSlice : ModDust
    {
        public static int Create(Vector2 pos, int width, int height)
        {
            Color color = new Color(100, 255, 120);
            if (Main.rand.Next(3) == 0)
            {
                color = new Color(200, 255, 120);
            }
            return Dust.NewDust(pos, width, height, Bluemagic.Instance.Find<ModDust>("PuriumSlice").Type, 0f, 0f, 100, color, 1f);
        }

        public override bool Update(Dust dust)
        {
            Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 0.3f, 0.6f, 0.2f);
            return true;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return Color.White;
        }
    }
}