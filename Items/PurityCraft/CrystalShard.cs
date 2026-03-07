using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
    public class CrystalShard : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.alpha = 50;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.2f;
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - (float)Math.PI / 4f;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int k = 0; k < 6; k++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Mod.Find<ModDust>("CrystalStar").Type, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default(Color), 1.2f);
            }
            for (int k = 0; k < 2; k++)
            {
                int goreType = Mod.Find<ModGore>(Main.rand.Next(2) == 0 ? "GreenStar" : "WhiteStar").Type;
                Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, 0.05f * Projectile.velocity, goreType, 1f);
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 0);
        }
    }
}