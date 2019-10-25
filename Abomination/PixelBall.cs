using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Abomination
{
    public class PixelBall : ElementBall
    {
        public override string Texture
        {
            get
            {
                return "Bluemagic/Abomination/ElementBall";
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pixel Ball");
        }

        public override void CreateDust()
        {
            Color? color = GetColor();
            if (color.HasValue)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("Pixel"), 0f, 0f, 0, color.Value);
                Main.dust[dust].velocity += projectile.velocity;
                Main.dust[dust].scale = 0.9f;
            }
        }

        public override void PlaySound()
        {
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 33);
        }

        public override string GetName()
        {
            if (projectile.ai[0] == 24f)
            {
                return "Fire Sprite";
            }
            if (projectile.ai[0] == 44f)
            {
                return "Frost Sprite";
            }
            if (projectile.ai[0] == mod.BuffType("EtherealFlames"))
            {
                return "Spirit Sprite";
            }
            if (projectile.ai[0] == 70f)
            {
                return "Infestation Sprite";
            }
            if (projectile.ai[0] == 69f)
            {
                return "Ichor Sprite";
            }
            return "Doom Bubble";
        }
    }
}