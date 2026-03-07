using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Dusts = Bluemagic.Dusts;

namespace Bluemagic.Items.Purium.Weapons.Projectiles
{
    public class PuriumSlice : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Purium Slicer");
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.scale = 1.4f;
            Projectile.alpha = 100;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 120;
        }

        public override void AI()
        {
            Lighting.AddLight((int)(Projectile.position.X / 16f), (int)(Projectile.position.Y / 16f), 0.3f, 0.6f, 0.2f);
            Projectile.rotation += (float)Projectile.direction * 0.5f;
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 30f)
            {
                if (Projectile.ai[0] < 100f && Projectile.velocity.Length() < 32f)
                {
                    Projectile.velocity *= 1.06f;
                }
                else
                {
                    Projectile.ai[0] = 200f;
                }
            }
            if (Main.rand.Next(2) == 0)
            {
                int dust = Dusts.PuriumSlice.Create(Projectile.position, Projectile.width, Projectile.height);
                Main.dust[dust].noGravity = true;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 12;
            height = 12;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int k = 0; k < 15; k++)
            {
                int dust = Dusts.PuriumSlice.Create(Projectile.position, Projectile.width, Projectile.height);
                Main.dust[dust].noGravity = true;
                Dusts.PuriumSlice.Create(Projectile.position, Projectile.width, Projectile.height);
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}