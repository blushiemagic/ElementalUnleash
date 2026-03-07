using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Bluemagic.Projectiles;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons.Projectiles
{
    public class PurityWisp : HoverShooter
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 24;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            inertia = 20f;
            shoot = Mod.Find<ModProjectile>("PurityBolt").Type;
            shootSpeed = 12f;
        }

        public override void CheckActive()
        {
            Player player = Main.player[Projectile.owner];
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            if (player.dead)
            {
                modPlayer.purityMinion = false;
            }
            if (modPlayer.purityMinion)
            {
                Projectile.timeLeft = 2;
            }
        }

        public override void CreateDust()
        {
            if (Projectile.ai[0] == 0f)
            {
                if (Main.rand.Next(5) == 0)
                {
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height / 2, Mod.Find<ModDust>("PuriumFlame").Type);
                    Main.dust[dust].velocity.Y -= 1.2f;
                }
            }
            else
            {
                if (Main.rand.Next(3) == 0)
                {
                    Vector2 dustVel = Projectile.velocity;
                    if (dustVel != Vector2.Zero)
                    {
                        dustVel.Normalize();
                    }
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Mod.Find<ModDust>("PuriumFlame").Type);
                    Main.dust[dust].velocity -= 1.2f * dustVel;
                }
            }
            Lighting.AddLight((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f), 0.6f, 0.9f, 0.3f);
        }

        public override void SelectFrame()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 8)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 3;
            }
        }
    }
}