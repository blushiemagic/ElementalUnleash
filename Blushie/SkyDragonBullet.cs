using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Blushie
{
    public class SkyDragonBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sky Dragon");
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionShot[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 64;
            projectile.height = 64;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (projectile.localAI[0] == 0f && projectile.ai[0] == 0f)
            {
                Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 14);
                projectile.localAI[0] = 1f;
            }
            NPC npc = Main.npc[(int)projectile.ai[1]];
            if (!npc.active || !npc.CanBeChasedBy(projectile))
            {
                projectile.Kill();
                return;
            }
            Vector2 offset = npc.Center - projectile.Center;
            if (offset == Vector2.Zero)
            {
                offset = -Vector2.UnitY;
            }
            offset.Normalize();
            float speed = 32f;
            if (projectile.ai[0] == 1f)
            {
                speed = projectile.velocity.Length() + 0.1f;
            }
            projectile.velocity = speed * offset;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}