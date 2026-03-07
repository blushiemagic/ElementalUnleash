using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit.Projectiles
{
    public class DanceOfBlades : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dance of Blades");
        }

        public override void SetDefaults()
        {
            Projectile.width = 220;
            Projectile.height = 220;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
            Projectile.ownerHitCheck = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.alpha = 30;
        }

        public override void AI()
        {
            Projectile.soundDelay--;
            if (Projectile.soundDelay <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item15, Projectile.Center);
                Projectile.soundDelay = 45;
            }
            Player player = Main.player[Projectile.owner];
            if (Main.myPlayer == Projectile.owner)
            {
                if (!player.channel || player.noItems || player.CCed)
                {
                    Projectile.Kill();
                }
                else
                {
                    Projectile.ai[0] -= 1f;
                    if (Projectile.ai[0] <= 0f)
                    {
                        CreateBlade();
                        Projectile.ai[0] = 3f;
                    }
                }
            }
            Lighting.AddLight(Projectile.Center, 0.3f, 1f, 0.3f);
            Projectile.Center = player.MountedCenter;
            Projectile.position.X += player.width / 2 * player.direction;
            Projectile.spriteDirection = player.direction;
            Projectile.timeLeft = 2;
            Projectile.rotation += 0.3f * player.direction;
            if (Projectile.rotation > MathHelper.TwoPi)
            {
                Projectile.rotation -= MathHelper.TwoPi;
            }
            else if (Projectile.rotation < 0)
            {
                Projectile.rotation += MathHelper.TwoPi;
            }
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = Projectile.rotation;
        }

        private void CreateBlade()
        {
            Player player = Main.player[Projectile.owner];
            float x = player.Center.X + 2f * Main.rand.Next(-300, 301);
            float y = player.Center.Y - 400f;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), x, y, 0f, 12f, Mod.Find<ModProjectile>("BladeRain").Type, Projectile.damage, Projectile.knockBack, Projectile.owner);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 5;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            return false;
        }
    }
}