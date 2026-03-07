using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit.Projectiles
{
    public class CleanserBeam : ModProjectile
    {
        private const int maxTime = 50;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 80;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[0] < 2f * maxTime)
            {
                Projectile.ai[0] += 1f;
            }
            if (Main.myPlayer == Projectile.owner)
            {
                if (Projectile.ai[0] == maxTime)
                {
                    Vector2 direction = Vector2.Normalize(Projectile.velocity);
                    if (float.IsNaN(direction.X) || float.IsNaN(direction.Y))
                    {
                        direction = -Vector2.UnitY;
                    }
                    int laser = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, direction.X, direction.Y, Mod.Find<ModProjectile>("CleanserLaser").Type, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, Projectile.whoAmI);
                    Projectile.ai[1] = Main.projectile[laser].identity;
                    Projectile.netUpdate = true;
                }
                if (!player.channel || player.noItems || player.CCed)
                {
                    Projectile.Kill();
                }
            }
            Projectile.soundDelay--;
            if (Projectile.ai[0] >= maxTime && Projectile.soundDelay <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item15, Projectile.Center);
                Projectile.soundDelay = 40;
            }

            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter, true);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.timeLeft = 2;
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * Projectile.direction, Projectile.velocity.X * Projectile.direction);

            if (Projectile.ai[0] < maxTime && Projectile.velocity != Vector2.Zero)
            {
                Vector2 dustTarget = Projectile.Center + Vector2.Normalize(Projectile.velocity) * 50f;
                for (int k = 0; k < 1; k++)
                {
                    int dust = Dust.NewDust(dustTarget - new Vector2(24f, 24f), 48, 48, Mod.Find<ModDust>("CleanserBeamCharge").Type, 0f, 0f, 70);
                    Main.dust[dust].customData = dustTarget;
                }
            }
        }

        public override bool? CanDamage()
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            int byUUID = Projectile.GetByUUID(Projectile.owner, Projectile.ai[1]);
            if (byUUID >= 0 && Projectile.ai[0] > maxTime && Main.projectile[byUUID].type == Mod.Find<ModProjectile>("CleanserLaser").Type)
            {
                Main.instance.DrawProj(byUUID);
            }
            return true;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("Bluemagic/Items/PuritySpirit/Projectiles/CleanserBeam_Glow").Value;
            SpriteEffects spriteEffects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, Color.White, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, spriteEffects, 0f);
        }
    }
}