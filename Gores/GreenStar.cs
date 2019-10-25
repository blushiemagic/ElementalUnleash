using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Gores
{
    public class GreenStar : Star
    {
        public override void LightColor(ref float r, ref float g, ref float b)
        {
            r *= 0.2f;
            g *= 0.4f;
        }
    }
}