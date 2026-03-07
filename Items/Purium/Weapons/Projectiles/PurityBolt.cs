using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons.Projectiles
{
    public class PurityBolt : ModProjectile
    {
        public override string Texture
        {
            get
            {
                return "Bluemagic/Items/Purium/Weapons/Projectiles/PuriumBullet";
            }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Purity Wisp");
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
                Projectile.localAI[0] = 1f;
            }
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowTorch, 0f, 0f, 100, new Color(0, 255, 0), 1.5f);
            Main.dust[dust].velocity *= 0.1f;
            if (Projectile.velocity == Vector2.Zero)
            {
                Main.dust[dust].velocity.Y -= 1f;
                Main.dust[dust].scale = 1.2f;
            }
            else
            {
                Main.dust[dust].velocity += Projectile.velocity * 0.2f;
            }
            Main.dust[dust].position.X = Projectile.Center.X + 4f + (float)Main.rand.Next(-2, 3);
            Main.dust[dust].position.Y = Projectile.Center.Y + (float)Main.rand.Next(-2, 3);
            Main.dust[dust].noGravity = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate = -1;
            Projectile.maxPenetrate = -1;
            Projectile.tileCollide = false;
            Projectile.position += Projectile.velocity;
            Projectile.velocity = Vector2.Zero;
            Projectile.timeLeft = 180;
            return false;
        }
    }
}