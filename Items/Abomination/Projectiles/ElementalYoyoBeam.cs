using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination.Projectiles
{
    public class ElementalYoyoBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Elemental Yoyo");
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.extraUpdates = 0;
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.scale = 1.2f;
        }

        public override void AI()
        {
            projectile.frame = (int)projectile.ai[0];
            switch ((int)projectile.ai[0])
            {
            case 0:
                Lighting.AddLight(projectile.position, 0.25f, 0.05f, 0.05f);
                break;
            case 1:
                Lighting.AddLight(projectile.position, 0.05f, 0.2f, 0.25f);
                break;
            case 2:
                Lighting.AddLight(projectile.position, 0.05f, 0.05f, 0.25f);
                break;
            case 3:
                Lighting.AddLight(projectile.position, 0.15f, 0.2f, 0.2f);
                break;
            case 4:
                Lighting.AddLight(projectile.position, 0.05f, 0.2f, 0.05f);
                break;
            case 5:
                Lighting.AddLight(projectile.position, 0.25f, 0.25f, 0.05f);
                break;
            }
            projectile.velocity *= 0.985f;
            projectile.rotation += projectile.velocity.X * 0.2f;
            if (projectile.velocity.X > 0f)
            {
                projectile.rotation += 0.08f;
            }
            else
            {
                projectile.rotation -= 0.08f;
            }
            projectile.ai[1] += 1f;
            if (projectile.ai[1] > 45f)
            {
                projectile.alpha += 10;
                if (projectile.alpha >= 255)
                {
                    projectile.alpha = 255;
                    projectile.Kill();
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.ai[0] == 3f)
            {
                damage += 20;
            }
        }

        public override void ModifyHitPvp(Player target, ref int damage, ref bool crit)
        {
            if (projectile.ai[0] == 3f)
            {
                damage += 20;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Main.player[projectile.owner].Counterweight(target.Center, projectile.damage, projectile.knockBack);
            projectile.friendly = false;
            projectile.ai[1] = 1000f;
            int debuff = GetDebuff();
            if (debuff > 0)
            {
                target.AddBuff(debuff, GetDebuffTime());
            }
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            int debuff = GetDebuff();
            if (debuff > 0)
            {
                target.AddBuff(debuff, GetDebuffTime() / 2);
            }
        }

        public int GetDebuff()
        {
            switch ((int)projectile.ai[0])
            {
            case 0:
                return BuffID.OnFire;
            case 1:
                return BuffID.Frostburn;
            case 2:
                return mod.BuffType("EtherealFlames");
            case 3:
                return 0;
            case 4:
                return BuffID.Venom;
            case 5:
                return BuffID.Ichor;
            default:
                return 0;
            }
        }

        public int GetDebuffTime()
        {
            switch ((int)projectile.ai[0])
            {
            case 0:
                return 600;
            case 1:
                return 400;
            case 2:
                return 300;
            case 3:
                return 0;
            case 4:
                return 400;
            case 5:
                return 900;
            default:
                return 0;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.velocity.X = -oldVelocity.X;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            int value = 255 - projectile.alpha;
            return new Color(value, value, value, 0);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int count = (int)projectile.ai[1] + 1;
            if (count > 7)
            {
                count = 7;
            }
            Texture2D texture = Main.projectileTexture[projectile.type];
            Vector2 origin = new Vector2(texture.Width * 0.5f, projectile.height / 2);
            Vector2 drawPos = projectile.position - Main.screenPosition + origin;
            drawPos.Y += projectile.gfxOffY;
            int frameHeight = texture.Height / Main.projFrames[projectile.type];
            Rectangle frame = new Rectangle(0, projectile.frame * frameHeight, texture.Width, frameHeight);
            Color color = GetAlpha(lightColor).Value;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (projectile.spriteDirection == -1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            for (int k = 1; k < count; k++)
            {
                Vector2 drawOff = projectile.velocity * k * 1.5f;
                float strength = 0.4f - k * 0.06f;
                strength *= 1f - projectile.alpha / 255f;
                Color drawColor = color;
                drawColor.R = (byte)(color.R * strength);
                drawColor.G = (byte)(color.G * strength);
                drawColor.B = (byte)(color.B * strength);
                drawColor.A = (byte)(color.A * strength / 2f);
                float scale = projectile.scale;
                scale -= k * 0.1f;
                Main.spriteBatch.Draw(texture, drawPos - drawOff, frame, drawColor, projectile.rotation, origin, scale, spriteEffects, 0f);
            }
        }
    }
}
