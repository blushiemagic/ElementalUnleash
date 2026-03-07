using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Bluemagic.Gores
{
    public abstract class Star : ModGore
    {
        public override void OnSpawn(Gore gore, IEntitySource source)
        {
            gore.sticky = false;
            gore.alpha = 100;
            gore.scale = 0.7f;
            gore.light = 1f;
        }

        public override bool Update(Gore gore)
        {
            gore.velocity *= 0.98f;
            gore.scale -= 0.01f;
            if (gore.scale < 0.1f)
            {
                gore.scale = 0.1f;
                gore.active = false;
            }
            gore.rotation += gore.velocity.X * 0.1f;
            gore.position += gore.velocity;
            if (gore.light > 0f)
            {
                float light = gore.light * gore.scale;
                float r = light;
                float g = light;
                float b = light;
                LightColor(ref r, ref g, ref b);
                Lighting.AddLight((int)((gore.position.X + gore.scale * 11f) / 16f), (int)((gore.position.Y + gore.scale * 12f) / 16f), r, g, b);
            }
            return false;
        }

        public override Color? GetAlpha(Gore gore, Color lightColor)
        {
            int alpha = lightColor.A - gore.alpha;
            if (alpha < 0)
            {
                alpha = 0;
            }
            if (alpha > 255)
            {
                alpha = 255;
            }
            return new Color(lightColor.R, lightColor.G, lightColor.B, alpha);
        }

        public virtual void LightColor(ref float r, ref float g, ref float b)
        {
        }
    }
}