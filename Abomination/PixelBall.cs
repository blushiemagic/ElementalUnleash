using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
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
            // DisplayName.SetDefault("Pixel Ball");
        }

        public override void CreateDust()
        {
            Color? color = GetColor();
            if (color.HasValue)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Mod.Find<ModDust>("Pixel").Type, 0f, 0f, 0, color.Value);
                Main.dust[dust].velocity += Projectile.velocity;
                Main.dust[dust].scale = 0.9f;
            }
        }

        public override void PlaySound()
        {
            SoundEngine.PlaySound(SoundID.Item33, Projectile.position);
        }

        public override string GetName()
        {
            if (Projectile.ai[0] == 24f)
            {
                return "Fire Sprite";
            }
            if (Projectile.ai[0] == 44f)
            {
                return "Frost Sprite";
            }
            if (Projectile.ai[0] == Mod.Find<ModBuff>("EtherealFlames").Type)
            {
                return "Spirit Sprite";
            }
            if (Projectile.ai[0] == 70f)
            {
                return "Infestation Sprite";
            }
            if (Projectile.ai[0] == 69f)
            {
                return "Ichor Sprite";
            }
            return "Doom Bubble";
        }
    }
}