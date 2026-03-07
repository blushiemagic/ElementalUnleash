using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Blushie
{
    public class SkyDragonBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sky Dragon");
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f && Projectile.ai[0] == 0f)
            {
                SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
                Projectile.localAI[0] = 1f;
            }
            NPC npc = Main.npc[(int)Projectile.ai[1]];
            if (!npc.active || !npc.CanBeChasedBy(Projectile))
            {
                Projectile.Kill();
                return;
            }
            Vector2 offset = npc.Center - Projectile.Center;
            if (offset == Vector2.Zero)
            {
                offset = -Vector2.UnitY;
            }
            offset.Normalize();
            float speed = 32f;
            if (Projectile.ai[0] == 1f)
            {
                speed = Projectile.velocity.Length() + 0.1f;
            }
            Projectile.velocity = speed * offset;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}