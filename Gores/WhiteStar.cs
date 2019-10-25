using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Gores
{
    public class WhiteStar : Star
    {
        public override void LightColor(ref float r, ref float g, ref float b)
        {
            r *= 0.9f;
            g *= 0.9f;
            b *= 0.9f;
        }
    }
}