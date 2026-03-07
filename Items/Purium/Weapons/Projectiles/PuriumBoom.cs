using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons.Projectiles
{
    public class PuriumBoom : ModProjectile
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
            // DisplayName.SetDefault("Purium Breaker");
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            Projectile.localAI[0] = (Projectile.localAI[0] + 1f) % 4f;
            if (Projectile.localAI[0] == 0f)
            {
                Dust.NewDust(Projectile.position, 0, 0, Mod.Find<ModDust>("PuriumBullet").Type);
            }
            if (Projectile.ai[0] <= 0f)
            {
                Projectile.Kill();
            }
            Projectile.ai[0] -= 1f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.penetrate >= 0)
            {
                Projectile.penetrate++;
            }
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.position = Projectile.Center;
            for (int k = 0; k < 4; k++)
            {
                float scale = 0.4f * k;
                CreateGore(scale, 1f, 1f);
                CreateGore(scale, -1f, 1f);
                CreateGore(scale, 1f, -1f);
                CreateGore(scale, -1f, -1f);
            }
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            Projectile.width = 160;
            Projectile.height = 160;
            Projectile.position -= Projectile.Size / 2f;
            Projectile.damage *= 2;
            Projectile.penetrate = 1;
            Projectile.maxPenetrate = 1;
            Projectile.Damage();
        }

        private void CreateGore(float scale, float offX, float offY)
        {
            int gore = Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Vector2.Zero, Main.rand.Next(435, 438), 1f);
            Main.gore[gore].velocity *= scale;
            Main.gore[gore].velocity.X += offX;
            Main.gore[gore].velocity.Y += offY;
        }
    }
}